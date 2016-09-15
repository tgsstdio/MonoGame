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
        private MgSpriteBatchBuffer mBatchBuffer;
        public MgSpriteBatchPool(IMgThreadPartition partition, MgSpriteBatchBuffer batchBuffer, MgSpriteBatchPoolCreateInfo createInfo, IMgAllocationCallbacks allocator)
        {
            if (partition == null)
                throw new ArgumentNullException(nameof(partition));

            if (batchBuffer == null)
                throw new ArgumentNullException(nameof(batchBuffer));

            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            if (createInfo.IndicesCount % 6 != 0)
                throw new ArgumentException(nameof(createInfo.IndicesCount) + " is not a multiple of 6");

            if (createInfo.VerticesCount % 4 != 0)
                throw new ArgumentException(nameof(createInfo.VerticesCount) + " is not a multiple of 4");

            if (createInfo.MaterialsCount <= 0)
                throw new ArgumentException(nameof(createInfo.MaterialsCount) + " must be > 0");

            if (createInfo.InstancesCount <= 0)
                throw new ArgumentException(nameof(createInfo.InstancesCount) + " must be > 0");

            mPartition = partition;
            mAllocator = allocator;
            mBatchBuffer = batchBuffer;

            {
                var descPoolCreateInfo = new MgDescriptorPoolCreateInfo {                    
                    MaxSets = createInfo.MaterialsCount,
                    PoolSizes = new MgDescriptorPoolSize[]
                    {
                        new MgDescriptorPoolSize
                        {
                            Type = MgDescriptorType.STORAGE_BUFFER,
                            DescriptorCount = createInfo.MaterialsCount,
                        },
                        new MgDescriptorPoolSize
                        {
                            Type = MgDescriptorType.STORAGE_BUFFER,
                            DescriptorCount = createInfo.MaterialsCount,
                        },
                    },
                };

                IMgDescriptorPool pDescriptorPool;
                var err = mPartition.Device.CreateDescriptorPool(descPoolCreateInfo, mAllocator, out pDescriptorPool);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                mDescriptorPool = pDescriptorPool;
            }

            {
                var cmdPoolCreateInfo = new MgCommandPoolCreateInfo {
                    
                };

                IMgCommandPool pCommandPool;
                var err = mPartition.Device.CreateCommandPool(cmdPoolCreateInfo, mAllocator, out pCommandPool);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                mCommandPool = pCommandPool;
            }


            MgMemoryRequirements memReqs;
            mPartition.Device.GetBufferMemoryRequirements(mBatchBuffer.Buffer, out memReqs);

            uint memoryTypeIndex;
            var memoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT;
            mPartition.GetMemoryType(memReqs.MemoryTypeBits, memoryPropertyFlags, out memoryTypeIndex);

            {
                var memAlloc = new MgMemoryAllocateInfo
                {
                    MemoryTypeIndex = memoryTypeIndex,
                    AllocationSize = memReqs.Size,
                };

                IMgDeviceMemory deviceMemory;
                var err = mPartition.Device.AllocateMemory(memAlloc, mAllocator, out deviceMemory);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                mDeviceMemory = deviceMemory;
            }

            mBatchBuffer.Buffer.BindBufferMemory(mPartition.Device, mDeviceMemory, 0UL);
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
        private IMgDeviceMemory mDeviceMemory;

        private bool mIsDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            if (mDescriptorPool != null)
                mDescriptorPool.DestroyDescriptorPool(mPartition.Device, mAllocator);

            if (mCommandPool != null)
                mCommandPool.DestroyCommandPool(mPartition.Device, mAllocator);

            if (mDeviceMemory != null)
                mDeviceMemory.FreeMemory(mPartition.Device, mAllocator);

            if (mBatchBuffer != null)
                mBatchBuffer.Dispose();

            mIsDisposed = true;
        }
    }
}
