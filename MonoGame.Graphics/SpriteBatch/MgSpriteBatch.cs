using Magnesium;
using Microsoft.Xna.Framework;
using MonoGame.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    public class MgSpriteBatch
    {
        private List<MgSpriteVertexDataItem> mItems;
        private List<MgSpriteMaterialData> mMaterials;
        private List<MgBatchQuad> mSprites;
        private IMgThreadPartition mPartition;
        private MgSpriteBatchEffect mEffect;
        private bool _beginCalled = false;
        private MgSpriteBatchBuffer mBatchBuffer;
        private MgSpriteBatchPool mPool;

        public MgSpriteBatch(IMgThreadPartition partition, MgSpriteBatchEffect effect, MgSpriteBatchBuffer batchBuffer, MgSpriteBatchPool pool)
        {
            mItems = new List<MgSpriteVertexDataItem>();
            mSprites = new List<MgBatchQuad>();
            mMaterials = new List<MgSpriteMaterialData>();

            mPartition = partition;
            mEffect = effect;
            mBatchBuffer = batchBuffer;
            mPool = pool;
        }


        public void Begin(uint index)
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");

            mBatchBuffer.Reset();
            mPool.Reset();

            _beginCalled = true;
        }

        public void DrawInternal(MgSpriteBatchCreateInfo parameter)
        {
            var imageSrc = parameter.ImageSource;

            Rectangle _tempRect;

            if (parameter.SourceRect.HasValue)
            {
                _tempRect = parameter.SourceRect.Value;
            }
            else
            {
                _tempRect.X = 0;
                _tempRect.Y = 0;
                _tempRect.Width = (int)imageSrc.Width;
                _tempRect.Height = (int)imageSrc.Height;
            }

            Vector2 _texCoordTL = new Vector2(0, 0);
            Vector2 _texCoordBR = new Vector2(0, 0);

            _texCoordTL.X = _tempRect.X / (float)imageSrc.Width;
            _texCoordTL.Y = _tempRect.Y / (float)imageSrc.Height;
            _texCoordBR.X = (_tempRect.X + _tempRect.Width) / (float)imageSrc.Width;
            _texCoordBR.Y = (_tempRect.Y + _tempRect.Height) / (float)imageSrc.Height;

            if ((parameter.SpriteEffect & SpriteEffects.FlipVertically) != 0)
            {
                var temp = _texCoordBR.Y;
                _texCoordBR.Y = _texCoordTL.Y;
                _texCoordTL.Y = temp;
            }
            if ((parameter.SpriteEffect & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = _texCoordBR.X;
                _texCoordBR.X = _texCoordTL.X;
                _texCoordTL.X = temp;
            }

            var destinationRectangle = parameter.DestinationRect;

            var item = new MgSpriteVertexDataItem();
            item.Set(destinationRectangle.X,
                    destinationRectangle.Y,
                    parameter.Depth,
                    -parameter.Origin.X,
                    -parameter.Origin.Y,
                    destinationRectangle.Z,
                    destinationRectangle.W,
                    (float)Math.Sin(parameter.Rotation),
                    (float)Math.Cos(parameter.Rotation),
                    parameter.Color,
                    _texCoordTL,
                    _texCoordBR);

            // CommandBuffer


            var materialIndex = mBatchBuffer.Materials.NoOfItems;
            var descriptorSet = mPool.DescriptorSets[materialIndex];

            var writes = new MgWriteDescriptorSet[]
            {
                new MgWriteDescriptorSet
                {
                    DstSet = descriptorSet,
                    DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                    DstBinding = 0,
                    DescriptorCount = 1,
                    ImageInfo = new []
                    {
                        new MgDescriptorImageInfo
                        {
                            ImageLayout = MgImageLayout.SHADER_READ_ONLY_OPTIMAL,
                            ImageView = parameter.ImageView,
                            Sampler = parameter.Sampler,
                        },
                    }
                },
                new MgWriteDescriptorSet
                {
                    DstSet = descriptorSet,
                    DescriptorType = MgDescriptorType.STORAGE_BUFFER,
                    DstBinding = 1,
                    DescriptorCount = 1,
                    BufferInfo = new MgDescriptorBufferInfo[]
                    {
                        new MgDescriptorBufferInfo
                        {
                            Buffer = mBatchBuffer.Buffer,
                            Offset = mBatchBuffer.Materials.Offset,
                            Range = mBatchBuffer.Materials.ArraySize,
                        }
                    },
                },
            };
            mPartition.Device.UpdateDescriptorSets(writes, null);

            var quad = new MgBatchQuad
            {
                DescriptorSet = descriptorSet,
                IndexCount = 6,
                InstanceCount = 1,
                FirstIndex = mBatchBuffer.Indices.NoOfItems,
                FirstVertex = (int)mBatchBuffer.Vertices.NoOfItems,
                FirstInstance = materialIndex,
                DepthBias = parameter.DepthBias,
                DepthBounds = parameter.DepthBounds,
                Linewidth = parameter.Linewidth,
                BlendConstants = parameter.BlendConstants,
                FrontCompareMask = parameter.FrontCompareMask,
                BackCompareMask = parameter.BackCompareMask,
                FrontWriteMask = parameter.FrontWriteMask,
                BackWriteMask = parameter.BackWriteMask,
                FrontStencilReference = parameter.FrontStencilReference,
                BackStencilReference = parameter.BackStencilReference,
            };
            mBatchBuffer.Vertices.NoOfItems += 4;
            mBatchBuffer.Indices.NoOfItems += 6;
            mBatchBuffer.Materials.NoOfItems += 1;

            // materials
            var material = new MgSpriteMaterialData
            {
                Color = parameter.Color,
                Transform = GenerateTransform(parameter),
            };

            mItems.Add(item);
            mSprites.Add(quad);
            mMaterials.Add(material);
        }

        private static Matrix GenerateTransform(MgSpriteBatchCreateInfo parameter)
        {
            var viewport = parameter.Viewport;
            // item.Transform = parameter.Transform;

            Matrix projection;
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 0, out projection);

            // GL requires a half pixel offset to match DX.
            projection.M41 += -0.5f * projection.M11;
            projection.M42 += -0.5f * projection.M22;

            Matrix globalTransform = parameter.Transform;
            Matrix transform;
            Matrix.Multiply(ref globalTransform, ref projection, out transform);
            return transform;
        }
                
        public void End()
        {
            SetBufferData(mPartition, mBatchBuffer, mItems, mMaterials);

            _beginCalled = false;
        }

        private static void SetBufferData
        (
            IMgThreadPartition partition,
            MgSpriteBatchBuffer batchBuffer,
            List<MgSpriteVertexDataItem> items,
            List<MgSpriteMaterialData> materials
        )
        {
            // var quads = new MgSpriteVertexDataItem[1];

            IntPtr bufferDst = IntPtr.Zero;
            var err = batchBuffer.DeviceMemory.MapMemory(partition.Device, 0, batchBuffer.BufferSize, 0, out bufferDst);
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");

            // INDEX DATA

            {
                var totalBytes = 6 * batchBuffer.Indices.Stride;
                var indices = new byte[totalBytes];

                // ALWAYS 6 INDICES - TRIANGLE ORDER IS SAME
                if (batchBuffer.IndexType == MgIndexType.UINT16)
                {
                    var uShorts = new ushort[6];
                    uShorts[0] = 0;
                    uShorts[0] = 1;
                    uShorts[0] = 2;

                    uShorts[0] = 1;
                    uShorts[0] = 3;
                    uShorts[0] = 2;

                    Buffer.BlockCopy(uShorts, 0, indices, 0, totalBytes);
                }
                else
                {
                    var uShorts = new uint[6];
                    uShorts[0] = 0;
                    uShorts[0] = 1;
                    uShorts[0] = 2;

                    uShorts[0] = 1;
                    uShorts[0] = 3;
                    uShorts[0] = 2;

                    Buffer.BlockCopy(uShorts, 0, indices, 0, totalBytes);
                }

                // FYI - OFFSET is always 0
                IntPtr indicesDst = IntPtr.Add(bufferDst, (int)batchBuffer.Indices.Offset);

                var offset = 0;
                foreach (var item in items)
                {
                    IntPtr dest = IntPtr.Add(indicesDst, offset);
                    Marshal.Copy(indices, 0, dest, totalBytes);
                    offset += totalBytes;
                }
            }

            // VERTEX DATA
            {
                IntPtr verticesDst = IntPtr.Add(bufferDst, (int)batchBuffer.Vertices.Offset);

                var stride = batchBuffer.Vertices.Stride;
                var offset = 0;
                foreach (var quad in items)
                {
                    IntPtr dest = IntPtr.Add(verticesDst, offset);
                    Marshal.StructureToPtr(quad, dest, false);
                    offset += stride;
                }
            }

            //var materials = new MgSpriteMaterialData[1];

            // MATERIAL DATA 
            {
                IntPtr materialDst = IntPtr.Add(bufferDst, (int)batchBuffer.Materials.Offset);

                var stride = batchBuffer.Materials.Stride;
                var offset = 0;
                foreach (var material in materials)
                {
                    IntPtr dest = IntPtr.Add(materialDst, offset);
                    Marshal.StructureToPtr(material, dest, false);
                    offset += stride;
                }
            }


            // INSTANCE DATA - BASED ON THE NUMBER OF MATERIALS

            {
                IntPtr instanceDst = IntPtr.Add(bufferDst, (int)batchBuffer.Instances.Offset);

                var instances = new uint[materials.Count];
                var byteLength = sizeof(uint) * instances.Length;
                var byteData = new byte[byteLength];

                for (var i = 0U; i < instances.Length; ++i)
                {
                    instances[i] = i;
                }

                Buffer.BlockCopy(instances, 0, byteData, 0, byteLength);
                Marshal.Copy(byteData, 0, instanceDst, byteLength);
            }

            // INDIRECT DATA - IGNORE           

            batchBuffer.DeviceMemory.UnmapMemory(partition.Device);
        }

        public void DrawBatch(IMgGraphicsDevice graphics, uint frameNumber)
        {
            IMgCommandBuffer cmdBuf = mPool.CommandBuffer;
            DrawQuads(cmdBuf, mSprites, mEffect, mBatchBuffer, graphics, frameNumber);
        }

        // FIGURE WHERE THIS BELONG
        static void DrawQuads(IMgCommandBuffer cmdBuf, List<MgBatchQuad> sprites, MgSpriteBatchEffect effect, MgSpriteBatchBuffer batchBuffer, IMgGraphicsDevice graphics, uint frameNumber)
        {
            var renderPass = new MgRenderPassBeginInfo
            {
                RenderPass = graphics.Renderpass,
                Framebuffer = graphics.Framebuffers[frameNumber],
            };


            var beginInfo = new MgCommandBufferBeginInfo
            {

            };

            var err = cmdBuf.BeginCommandBuffer(beginInfo);
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");

            cmdBuf.CmdBeginRenderPass(renderPass, MgSubpassContents.INLINE);

            IMgPipelineLayout layout = effect.PipelineLayout;
            foreach (var quadInfo in sprites)
            {
                IMgPipeline currentPipeline = effect.GraphicsPipelines[quadInfo.PipelineIndex];
                DrawIndexedQuad(cmdBuf, quadInfo, batchBuffer, layout, currentPipeline);
            }

            cmdBuf.CmdEndRenderPass();

            err = cmdBuf.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
        }

        static void DrawIndexedQuad(IMgCommandBuffer cmdBuf, MgBatchQuad quadInfo, MgSpriteBatchBuffer batchBuffer, IMgPipelineLayout layout, IMgPipeline currentPipeline)
        {
            cmdBuf.CmdSetViewport(0, new[] { quadInfo.Viewport });
            cmdBuf.CmdSetLineWidth(quadInfo.Linewidth);
            cmdBuf.CmdSetBlendConstants(quadInfo.BlendConstants);
            cmdBuf.CmdSetDepthBias(quadInfo.DepthBias.DepthBiasConstantFactor, quadInfo.DepthBias.DepthBiasClamp, quadInfo.DepthBias.DepthBiasSlopeFactor);
            cmdBuf.CmdSetDepthBounds(quadInfo.DepthBounds.MinDepthBounds, quadInfo.DepthBounds.MaxDepthBounds);

            cmdBuf.CmdSetStencilCompareMask(MgStencilFaceFlagBits.FRONT_BIT, quadInfo.FrontCompareMask);
            cmdBuf.CmdSetStencilCompareMask(MgStencilFaceFlagBits.BACK_BIT, quadInfo.BackCompareMask);
            cmdBuf.CmdSetStencilReference(MgStencilFaceFlagBits.FRONT_BIT, quadInfo.FrontStencilReference);
            cmdBuf.CmdSetStencilReference(MgStencilFaceFlagBits.BACK_BIT, quadInfo.BackStencilReference);
            cmdBuf.CmdSetStencilWriteMask(MgStencilFaceFlagBits.FRONT_BIT, quadInfo.FrontWriteMask);
            cmdBuf.CmdSetStencilWriteMask(MgStencilFaceFlagBits.BACK_BIT, quadInfo.BackWriteMask);

            var descriptorSets = new[] { quadInfo.DescriptorSet };
            // IMgPipelineLayout layout = effect.PipelineLayout;
            cmdBuf.CmdBindDescriptorSets(MgPipelineBindPoint.GRAPHICS, layout, 0, 1, descriptorSets, new uint[] { 0 });

            //IMgPipeline currentPipeline = effect.GraphicsPipelines[quadInfo.PipelineIndex];
            cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, currentPipeline);

            // NO INDEX REQUIRED - UNIQUE VERTICES
            cmdBuf.CmdBindIndexBuffer(batchBuffer.Buffer, batchBuffer.Indices.Offset, batchBuffer.IndexType);

            cmdBuf.CmdBindVertexBuffers(0, new[] { batchBuffer.Buffer, batchBuffer.Buffer }, new[] { batchBuffer.Vertices.Offset, batchBuffer.Instances.Offset });

            // QUADS
            cmdBuf.CmdDrawIndexed (quadInfo.IndexCount, quadInfo.InstanceCount, quadInfo.FirstIndex, quadInfo.FirstVertex, quadInfo.FirstInstance);
        }
    }
}
