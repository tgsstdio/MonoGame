using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgSpriteVertexData
    {
        public Vector3 Position { get; set; }
        public Vector2 TexCoords { get; set; }
        public Vector4 Color { get; set; }
    }
}
