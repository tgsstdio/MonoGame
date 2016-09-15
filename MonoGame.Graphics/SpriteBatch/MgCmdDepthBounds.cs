using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgCmdDepthBounds
    {
        public float MinDepthBounds { get; set; }
        public float MaxDepthBounds { get; set; }
    }
}
