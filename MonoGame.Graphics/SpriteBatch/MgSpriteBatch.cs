using Magnesium;
using Microsoft.Xna.Framework;
using MonoGame.Core;
using System;
using System.Diagnostics;

namespace MonoGame.Graphics
{
    public class MgSpriteBatch
    {
        private bool _beginCalled = false;
        public void Begin(uint index)
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");

            _beginCalled = true;
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="effects">Modificators for drawing. Can be combined.</param>
        /// <param name="depth">A depth of the layer of this sprite.</param>
        public void DrawInternal(MgImageSource texture,
            Vector4 destinationRectangle,
            Rectangle? sourceRect,
            Vector4 color,
            float rotation,
            Vector2 origin,
            SpriteEffects effect,
            float depth)
        {
            var item = new MgSpriteVertexDataItem();

            // item.Depth = depth;
            //item.Texture = texture;

            Rectangle _tempRect;

            if (sourceRect.HasValue)
            {
                _tempRect = sourceRect.Value;
            }
            else
            {
                _tempRect.X = 0;
                _tempRect.Y = 0;
                _tempRect.Width = (int)texture.Width;
                _tempRect.Height = (int)texture.Height;
            }

            Vector2 _texCoordTL = new Vector2(0, 0);
            Vector2 _texCoordBR = new Vector2(0, 0);

            _texCoordTL.X = _tempRect.X / (float)texture.Width;
            _texCoordTL.Y = _tempRect.Y / (float)texture.Height;
            _texCoordBR.X = (_tempRect.X + _tempRect.Width) / (float)texture.Width;
            _texCoordBR.Y = (_tempRect.Y + _tempRect.Height) / (float)texture.Height;

            if ((effect & SpriteEffects.FlipVertically) != 0)
            {
                var temp = _texCoordBR.Y;
                _texCoordBR.Y = _texCoordTL.Y;
                _texCoordTL.Y = temp;
            }
            if ((effect & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = _texCoordBR.X;
                _texCoordBR.X = _texCoordTL.X;
                _texCoordTL.X = temp;
            }

            item.Set(destinationRectangle.X,
                    destinationRectangle.Y,
                    depth,
                    -origin.X,
                    -origin.Y,
                    destinationRectangle.Z,
                    destinationRectangle.W,
                    (float)Math.Sin(rotation),
                    (float)Math.Cos(rotation),
                    color,
                    _texCoordTL,
                    _texCoordBR);

            //if (autoFlush)
            //{
            //    FlushIfNeeded();
            //}
        }

        public void DrawInternal(MgSpriteBatchCreateInfo parameter)
        {
            var item = new MgSpriteVertexDataItem();

           // item.Depth = parameter.Depth;
           // item.Texture = texture;

            IMgImage currentTex = null;
            IMgSampler sampler = null;
            MgImageSource imageSrc = new MgImageSource();

            Rectangle _tempRect;

            if (parameter.SourceRect.HasValue)
            {
                _tempRect = parameter.SourceRect.Value;
            }
            else
            {
                _tempRect.X = 0;
                _tempRect.Y = 0;
                _tempRect.Width = (int) imageSrc.Width;
                _tempRect.Height = (int) imageSrc.Height;
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

            var quad = new MgBatchQuad();           

            // TODO : indexes

            //item.Transform = transform;

            quad.DepthBias = parameter.DepthBias;
            quad.DepthBounds = parameter.DepthBounds;

            quad.Linewidth = parameter.Linewidth;
            quad.BlendConstants = parameter.BlendConstants;

            quad.FrontCompareMask = parameter.FrontCompareMask;
            quad.BackCompareMask = parameter.BackCompareMask;

            quad.FrontWriteMask = parameter.FrontWriteMask;
            quad.BackWriteMask = parameter.BackWriteMask;
            quad.FrontStencilReference = parameter.FrontStencilReference;
            quad.BackStencilReference = parameter.BackStencilReference;

            // TODO : data copy

        }    


        public void End()
        {
            _beginCalled = false;

            DrawBatch();
        }

        private void DrawBatch()
        {
            throw new NotImplementedException();
        }

        // FIGURE WHERE THIS BELONG
        static void DrawQuads(IMgCommandBuffer cmdBuf, MgBatchQuad[] quads, MgSpriteBatchEffect effect, MgSpriteBatchBuffer batchBuffer, IMgGraphicsDevice graphics, uint frameNumber)
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
            foreach (var quadInfo in quads)
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

            cmdBuf.CmdBindIndexBuffer(batchBuffer.Buffer, batchBuffer.Indices.Offset, batchBuffer.IndexType);

            cmdBuf.CmdBindVertexBuffers(0, new[] { batchBuffer.Buffer, batchBuffer.Buffer }, new[] { batchBuffer.Vertices.Offset, batchBuffer.Instances.Offset });

            // QUADS
            cmdBuf.CmdDrawIndexed(quadInfo.IndexCount, quadInfo.InstanceCount, quadInfo.FirstIndex, quadInfo.FirstVertex, quadInfo.FirstInstance);
        }
    }
}
