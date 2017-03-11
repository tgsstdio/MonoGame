using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgSpriteMaterialData
    {
        public uint Texture { get; set; }
        public uint Unused_0 { get; set; }
        public uint Unused_1 { get; set; }
        public uint Unused_2 { get; set; }
        public Vector4 Color { get; set; }
        public Matrix Transform { get; set; }
    }
}
