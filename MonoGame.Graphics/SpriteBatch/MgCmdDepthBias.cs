using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgCmdDepthBias
    {
        public float DepthBiasConstantFactor { get; set; }
        public float DepthBiasClamp { get; set; }
        public float DepthBiasSlopeFactor { get; set; }
    }
}
