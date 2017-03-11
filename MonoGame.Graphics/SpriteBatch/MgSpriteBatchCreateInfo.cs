using Microsoft.Xna.Framework;
using MonoGame.Core;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchCreateInfo
    {
        public uint TextureSlotId { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public float Depth { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }

        public Vector4 Color { get; set; }
        public Vector2 Origin { get; set; }
        public Matrix Transform { get; set; }
        public Vector4 DestinationRect { get; set; }

        public Rectangle? SourceRect { get; set; }
        public Vector2 TopLeft { get; set; }
        public Vector2 BottomRight { get; set; }

        public SpriteEffects SpriteEffect { get; set; }
    }
}
