using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgSpriteInstanceData
    {
        public uint Instance { get; set; }
        //      public Vector4 Color { get; set; }
    }
}
