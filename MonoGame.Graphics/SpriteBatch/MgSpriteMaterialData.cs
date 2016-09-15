using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public class MgSpriteMaterialData
    {
        public Vector4 Color { get; set; }
        public Matrix Transform { get; set; }
    }
}
