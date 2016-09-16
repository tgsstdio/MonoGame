using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{

    [StructLayout(LayoutKind.Sequential)]
    public struct DrawArraysIndirectCommand
    {
        uint count;
        uint primCount;
        uint first;
        uint baseInstance;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DrawElementsIndirectCommand
    {
        public uint Count { get; set; }
        public uint InstanceCount { get; set; }
        public uint FirstIndex { get; set; }
        public uint BaseVertex { get; set; }
        public uint BaseInstance { get; set; }
    };

    [StructLayout(LayoutKind.Sequential)]
    struct VkDrawIndirectCommand
    {
        uint vertexCount;
        uint instanceCount;
        uint firstVertex;
        uint firstInstance;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct VkDrawIndexedIndirectCommand
    {
        uint indexCount;
        uint instanceCount;
        uint firstIndex;
        uint vertexOffset;
        uint firstInstance;
    };


    public class MgSpriteBatchPool : IDisposable
    {
        private IMgThreadPartition mPartition;
        private IMgAllocationCallbacks mAllocator;
        public MgSpriteBatchPool(IMgThreadPartition partition, MgSpriteBatchPoolCreateInfo createInfo, IMgAllocationCallbacks allocator)
        {
            if (partition == null)
                throw new ArgumentNullException(nameof(partition));

            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            if (createInfo.SetLayout == null)
                throw new ArgumentNullException(nameof(createInfo.SetLayout));


            mPartition = partition;
            mAllocator = allocator;

            {
                var descPoolCreateInfo = new MgDescriptorPoolCreateInfo {                    
                    MaxSets = createInfo.DescriptorSetCount,
                    PoolSizes = new MgDescriptorPoolSize[]
                    {
                        new MgDescriptorPoolSize
                        {
                            Type = MgDescriptorType.STORAGE_BUFFER,
                            DescriptorCount = createInfo.DescriptorSetCount,
                        },
                        new MgDescriptorPoolSize
                        {
                            Type = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                            DescriptorCount = createInfo.DescriptorSetCount,
                        },
                    },
                };

                IMgDescriptorPool pDescriptorPool;
                var err = mPartition.Device.CreateDescriptorPool(descPoolCreateInfo, mAllocator, out pDescriptorPool);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                mDescriptorPool = pDescriptorPool;

                var descSets = new IMgDescriptorSet[createInfo.DescriptorSetCount];

                var setLayouts = new IMgDescriptorSetLayout[createInfo.DescriptorSetCount];
                for (var i = 0; i < createInfo.DescriptorSetCount; ++i)
                {
                    setLayouts[i] = createInfo.SetLayout;
                }

                var allocateInfo = new MgDescriptorSetAllocateInfo
                {
                    DescriptorPool = mDescriptorPool,
                    DescriptorSetCount = createInfo.DescriptorSetCount,
                    SetLayouts = setLayouts,
                };
                mPartition.Device.AllocateDescriptorSets(allocateInfo, out descSets);

                mDescriptorSets = descSets;
            }            

            {
                var cmdPoolCreateInfo = new MgCommandPoolCreateInfo {
                    
                };

                IMgCommandPool pCommandPool;
                var err = mPartition.Device.CreateCommandPool(cmdPoolCreateInfo, mAllocator, out pCommandPool);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                mCommandPool = pCommandPool;

                var buffers = new IMgCommandBuffer[1];

                var allocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = 1,
                    CommandPool = mCommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };
                mPartition.Device.AllocateCommandBuffers(allocateInfo, buffers);
                mCommandBuffer = buffers[0];
            }
        }

        private IMgDescriptorSet[] mDescriptorSets;
        public IMgDescriptorSet[] DescriptorSets
        {
            get
            {
                return mDescriptorSets;
            }
        }

        public IMgCommandBuffer CommandBuffer
        {
            get
            {
                return mCommandBuffer;
            }
        }

        public void Reset()
        {
            Debug.Assert(!mIsDisposed);
            mDescriptorPool.ResetDescriptorPool(mPartition.Device, 0);
            mCommandPool.ResetCommandPool(mPartition.Device, 0);
        }

        ~MgSpriteBatchPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IMgDescriptorPool mDescriptorPool;
        private IMgCommandPool mCommandPool;
        private IMgCommandBuffer mCommandBuffer;

        private bool mIsDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            if (mDescriptorPool != null)
            {
                if (mDescriptorSets != null)
                {
                    mPartition.Device.FreeDescriptorSets(mDescriptorPool, mDescriptorSets);
                }

                mDescriptorPool.DestroyDescriptorPool(mPartition.Device, mAllocator);
            }

            if (mCommandPool != null)
            {
                if (mCommandBuffer != null)
                {
                    mPartition.Device.FreeCommandBuffers(mCommandPool, new[] { mCommandBuffer });
                }

                mCommandPool.DestroyCommandPool(mPartition.Device, mAllocator);
            }

            mIsDisposed = true;
        }
    }
}
