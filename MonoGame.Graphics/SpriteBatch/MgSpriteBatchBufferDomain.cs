using System;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchBufferDomain
    {
        public MgSpriteBatchBufferDomain(UInt64 offset, int stride, uint limit)
        {
            Offset = offset;
            Stride = stride;
            ArrayLimit = limit;
            ArraySize = (ulong) (Stride * ArrayLimit);
            NoOfItems = 0;
        }

        public uint NoOfItems { get; set; }
        public uint ArrayLimit { get; internal set; }
        public ulong ArraySize { get; internal set; }
        public ulong Offset { get; internal set; }
        public int Stride { get; internal set; }

        public void Reset()
        {
            NoOfItems = 0;
        }
    }
}