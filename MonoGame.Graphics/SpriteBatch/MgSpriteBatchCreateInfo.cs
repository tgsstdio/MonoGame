using Magnesium;
using Microsoft.Xna.Framework;
using MonoGame.Core;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchCreateInfo
    {
        public uint PipelineIndex { get; set; }

        public IMgImageView ImageView { get; set; }
        public IMgSampler Sampler { get; set; }
        public MgImageSource ImageSource { get; set; }

        public MgViewport Viewport { get; set; }
        public float Depth { get; set; }
        public Vector4 Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Matrix Transform { get; set; }
        public Vector4 DestinationRect { get; set; }
        public Rectangle? SourceRect { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        public MgCmdDepthBias DepthBias { get; set; }
        public MgCmdDepthBounds DepthBounds { get; set; }

        public float Linewidth { get; set; }
        public MgColor4f BlendConstants { get; set; }

        public uint FrontCompareMask { get; set; }
        public uint BackCompareMask { get; set; }

        public uint FrontWriteMask { get; set; }
        public uint BackWriteMask { get; set; }

        public uint FrontStencilReference { get; set; }
        public uint BackStencilReference { get; set; }
    }
}
