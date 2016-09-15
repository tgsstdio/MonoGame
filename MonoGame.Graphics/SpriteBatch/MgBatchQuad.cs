using Magnesium;

namespace MonoGame.Graphics
{
    internal class MgBatchQuad
    {
        public uint PipelineIndex { get; set; }
        public IMgDescriptorSet DescriptorSet { get; set; }

        // 1 quad => 6, f(n) => 6 * n
        public uint IndexCount { get; internal set; }
        // 1 quad => 1, f(n) => n
        public uint InstanceCount { get; internal set; }
        public uint FirstIndex { get; internal set; }
        public int FirstVertex { get; set; }
        public uint FirstInstance { get; internal set; }

        // vkCmdBuf
        public MgViewport Viewport { get; internal set; }
        public float Linewidth { get; internal set; }
        public MgColor4f BlendConstants { get; internal set; }
        public MgCmdDepthBias DepthBias { get; internal set; }
        public MgCmdDepthBounds DepthBounds { get; internal set; }
        public uint FrontCompareMask { get; internal set; }
        public uint BackCompareMask { get; internal set; }
        public uint FrontStencilReference { get; internal set; }
        public uint BackStencilReference { get; internal set; }
        public uint FrontWriteMask { get; internal set; }
        public uint BackWriteMask { get; internal set; }
    }
}
