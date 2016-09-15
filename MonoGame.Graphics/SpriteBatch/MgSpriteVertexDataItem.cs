using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MgSpriteVertexDataItem
    {
        public MgSpriteVertexData vertexTL;
        public MgSpriteVertexData vertexTR;
        public MgSpriteVertexData vertexBL;
        public MgSpriteVertexData vertexBR;

        public void Set(float x, float y, float depth, float dx, float dy, float w, float h, float sin, float cos, Vector4 color, Vector2 texCoordTL, Vector2 texCoordBR)
        {
            // TODO, Should we be just assigning the Depth Value to Z?
            // According to http://blogs.msdn.com/b/shawnhar/archive/2011/01/12/spritebatch-billboards-in-a-3d-world.aspx
            // We do.
            vertexTL.Position = new Vector3
            {
                X = x + dx * cos - dy * sin,
                Y = y + dx * sin + dy * cos,
                Z = depth,
            };
            vertexTL.Color = color;
            vertexTL.TexCoords = new Vector2 {
                X = texCoordTL.X,
                Y = texCoordTL.Y,
            };

            vertexTR.Position = new Vector3 {
                X = x + (dx + w) * cos - dy * sin,
                Y = y + (dx + w) * sin + dy * cos,
                Z = depth,
            };

            vertexTR.Color = color;
            vertexTR.TexCoords = new Vector2 {
                X = texCoordBR.X,
                Y = texCoordTL.Y,
            };

            vertexBL.Position = new Vector3 {                 
                X = x + dx * cos - (dy + h) * sin,
                Y = y + dx * sin + (dy + h) * cos,
                Z = depth,
            };

            vertexBL.Color = color;
            vertexBL.TexCoords = new Vector2
            {
                X = texCoordTL.X,
                Y = texCoordBR.Y,
            };

            vertexBR.Position = new Vector3
            {
                X = x + (dx + w) * cos - (dy + h) * sin,
                Y = y + (dx + w) * sin + (dy + h) * cos,
                Z = depth,
            };

            vertexBR.Color = color;

            vertexBR.TexCoords = new Vector2
            {
                X = texCoordBR.X,
                Y = texCoordBR.Y,
            };
        }
    }
}