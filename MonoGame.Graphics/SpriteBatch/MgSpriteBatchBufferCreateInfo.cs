using Magnesium;
using System;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchBufferCreateInfo
    {
        public MgIndexType IndexType { get; set; }
        public uint IndicesCount { get; set; }
        public uint VerticesCount { get; set; }
        public uint MaterialsCount { get; set; }
        public uint InstancesCount { get; set; }
        //public Type IndirectType { get; set; }
        //public uint IndirectCount { get; set; }
    }
}
