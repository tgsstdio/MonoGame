using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchBuffer : IDisposable
    {
        public MgSpriteBatchBufferDomain Indices { get; private set; }
        public MgSpriteBatchBufferDomain Vertices { get; private set; }
        public MgSpriteBatchBufferDomain Materials { get; private set; }
        public MgSpriteBatchBufferDomain Instances { get; private set; }
        //public MgSpriteBatchBufferDomain Indirects { get; private set; }

        public MgIndexType IndexType { get; private set; }
        public UInt64 BufferSize { get; private set; }

        public IMgBuffer Buffer {
            get;
            internal set;
        }

        private IMgThreadPartition mPartition;
        private IMgAllocationCallbacks mAllocator;

        public IMgDeviceMemory DeviceMemory { get; private set; }

        public MgSpriteBatchBuffer(IMgThreadPartition partition, MgSpriteBatchBufferCreateInfo createInfo, IMgAllocationCallbacks allocator)
        {
            if (partition == null)
                throw new ArgumentNullException(nameof(partition));

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

            IndexType = createInfo.IndexType;

            Indices = new MgSpriteBatchBufferDomain(
                offset: 0UL,
                stride: (createInfo.IndexType == MgIndexType.UINT32) ? sizeof(uint) : sizeof(ushort),
                limit: createInfo.IndicesCount
            );

            Vertices = new MgSpriteBatchBufferDomain(
                offset: (ulong)Indices.ArraySize,
                stride: Marshal.SizeOf(typeof(MgSpriteVertexData)),
                limit: createInfo.VerticesCount
            );

            Materials = new MgSpriteBatchBufferDomain(
                offset: (ulong)(Vertices.Offset + Vertices.ArraySize),
                stride: Marshal.SizeOf(typeof(MgSpriteMaterialData)),
                limit: createInfo.MaterialsCount
            );

            Instances = new MgSpriteBatchBufferDomain(
                offset: (ulong)(Materials.Offset + Materials.ArraySize),
                stride: sizeof(uint), // FiXED SIZE UNLESS FRAG/VERT SHADER CHANGES 
                limit: createInfo.InstancesCount
            );

            //Indirects = new MgSpriteBatchBufferDomain(
            //    offset: (ulong)(Instances.Offset + Instances.ArraySize),
            //    stride: Marshal.SizeOf(createInfo.IndirectType),
            //    limit: createInfo.IndirectCount
            //);

            var lastSliceOffset = Instances.Offset;
            var lastSliceArrayLength = Instances.ArraySize;

            BufferSize = (ulong)(lastSliceOffset + lastSliceArrayLength);

            var usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT | MgBufferUsageFlagBits.VERTEX_BUFFER_BIT | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;

            //if (createInfo.IndirectCount > 0)
            //{
            //    usage |= MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT;
            //}

            var bufferCreateInfo = new MgBufferCreateInfo
            {
                Usage = usage,
                Size = BufferSize,
            };

            {
                IMgBuffer buffer;
                var err = mPartition.Device.CreateBuffer(bufferCreateInfo, mAllocator, out buffer);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                Buffer = buffer;
            }

            MgMemoryRequirements memReqs;
            mPartition.Device.GetBufferMemoryRequirements(Buffer, out memReqs);

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
                DeviceMemory = deviceMemory;
            }

            Buffer.BindBufferMemory(mPartition.Device, DeviceMemory, 0UL);

        }

        public void Reset()
        {
            Indices.Reset();
            Vertices.Reset();
            Materials.Reset();
            Instances.Reset();
            //Indirects.Reset();            
        }

        ~MgSpriteBatchBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool mIsDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            if (DeviceMemory != null)
                DeviceMemory.FreeMemory(mPartition.Device, mAllocator);


            if (Buffer != null)
                Buffer.DestroyBuffer(mPartition.Device, mAllocator);

            mIsDisposed = true;
        }
    }
}
