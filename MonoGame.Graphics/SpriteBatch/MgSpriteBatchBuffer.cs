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
        public MgSpriteBatchBufferDomain Indirects { get; private set; }

        public MgIndexType IndexType { get; private set; }
        private UInt64 mBufferSize;

        public IMgBuffer Buffer {
            get;
            internal set;
        }

        private IMgThreadPartition mPartition;
        private IMgAllocationCallbacks mAllocator;

        public MgSpriteBatchBuffer(IMgThreadPartition partition, IMgAllocationCallbacks allocator, MgSpriteBatchPoolCreateInfo createInfo)
        {
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

            Indirects = new MgSpriteBatchBufferDomain(
                offset: (ulong)(Instances.Offset + Instances.ArraySize),
                stride: Marshal.SizeOf(createInfo.IndirectType),
                limit: createInfo.IndirectCount
            );

            mBufferSize = (ulong)(Indirects.Offset + Indirects.ArraySize);

            var usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT | MgBufferUsageFlagBits.VERTEX_BUFFER_BIT | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;

            if (createInfo.IndirectCount > 0)
            {
                usage |= MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT;
            }

            var bufferCreateInfo = new MgBufferCreateInfo
            {
                Usage = usage,
                Size = mBufferSize,
            };

            IMgBuffer buffer;
            var err = mPartition.Device.CreateBuffer(bufferCreateInfo, mAllocator, out buffer);
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
            Buffer = buffer;

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

            if (Buffer != null)
                Buffer.DestroyBuffer(mPartition.Device, mAllocator);

            mIsDisposed = true;
        }
    }
}
