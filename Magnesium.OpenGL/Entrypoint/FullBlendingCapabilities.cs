using OpenTK.Graphics.OpenGL;
using Magnesium;

namespace Magnesium.OpenGL
{
	public class FullBlendingCapabilities : IBlendCapabilities
	{
		#region IBlendCapabilities implementation

		private static BlendingFactorDest GetBlendFactorDest (MgBlendFactor blend)
		{
			switch (blend) {
			case MgBlendFactor.DST_ALPHA:
				return BlendingFactorDest.DstAlpha;
				//			case Blend.DestinationColor:
				//				return BlendingFactorDest.DstColor;
			case MgBlendFactor.ONE_MINUS_DST_ALPHA:
				return BlendingFactorDest.OneMinusDstAlpha;
				//			case Blend.InverseDestinationColor:
				//				return BlendingFactorDest.OneMinusDstColor;
			case MgBlendFactor.ONE_MINUS_SRC_ALPHA:
				return BlendingFactorDest.OneMinusSrcAlpha;
			case MgBlendFactor.ONE_MINUS_SRC_COLOR:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.OneMinusSrcColor;
				#else
				return BlendingFactorDest.OneMinusSrcColor;
				#endif
			case MgBlendFactor.ONE:
				return BlendingFactorDest.One;
			case MgBlendFactor.SRC_ALPHA:
				return BlendingFactorDest.SrcAlpha;
				//			case Blend.SourceAlphaSaturation:
				//				return BlendingFactorDest.SrcAlphaSaturate;
			case MgBlendFactor.SRC_COLOR:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.SrcColor;
				#else
				return BlendingFactorDest.SrcColor;
				#endif
			case MgBlendFactor.ZERO:
				return BlendingFactorDest.Zero;
			default:
				return BlendingFactorDest.One;
			}
		}

		private bool mIsEnabled;
		public bool IsEnabled {
			get {
				return mIsEnabled;
			}
		}

		public void Initialise ()
		{
			EnableBlending (false);
			SetColorMask (0);
		}

		public void EnableBlending (bool blendEnabled)
		{
			if (blendEnabled)
				GL.Enable (EnableCap.Blend);
			else
				GL.Disable (EnableCap.Blend);
			mIsEnabled = blendEnabled;
		}

		public void SetColorMask (QueueDrawItemBitFlags colorMask)
		{
			GL.ColorMask (
				(colorMask & QueueDrawItemBitFlags.RedColorWriteChannel) != 0,
				(colorMask & QueueDrawItemBitFlags.GreenColorWriteChannel) != 0,
				(colorMask & QueueDrawItemBitFlags.BlueColorWriteChannel) != 0,
				(colorMask & QueueDrawItemBitFlags.AlphaColorWriteChannel) != 0);
		}

		private BlendingFactorSrc GetBlendFactorSrc (MgBlendFactor blend)
		{
			switch (blend) {
			case MgBlendFactor.DST_ALPHA:
				return BlendingFactorSrc.DstAlpha;
			case MgBlendFactor.DST_COLOR:
				return BlendingFactorSrc.DstColor;
			case MgBlendFactor.ONE_MINUS_DST_ALPHA:
				return BlendingFactorSrc.OneMinusDstAlpha;
			case MgBlendFactor.ONE_MINUS_DST_COLOR:
				return BlendingFactorSrc.OneMinusDstColor;
			case MgBlendFactor.ONE_MINUS_SRC_ALPHA:
				return BlendingFactorSrc.OneMinusSrcAlpha;
			case MgBlendFactor.ONE_MINUS_SRC_COLOR:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.OneMinusSrcColor;
				#else
				return BlendingFactorSrc.OneMinusSrcColor;
				#endif
			case MgBlendFactor.ONE:
				return BlendingFactorSrc.One;
			case MgBlendFactor.SRC_ALPHA:
				return BlendingFactorSrc.SrcAlpha;
			case MgBlendFactor.SRC_ALPHA_SATURATE:
				return BlendingFactorSrc.SrcAlphaSaturate;
			case MgBlendFactor.SRC_COLOR:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.SrcColor;
				#else
				return BlendingFactorSrc.SrcColor;
				#endif
			case MgBlendFactor.ZERO:
				return BlendingFactorSrc.Zero;
			default:
				return BlendingFactorSrc.One;
			}
		}

		public void ApplyBlendSeparateFunction (MgBlendFactor colorSource, MgBlendFactor colorDest, MgBlendFactor alphaSource, MgBlendFactor alphaDest)
		{
			GL.BlendFuncSeparate(
				GetBlendFactorSrc(colorSource),
				GetBlendFactorDest(colorDest), 
				GetBlendFactorSrc(alphaSource), 
				GetBlendFactorDest(alphaDest));
		}

		#endregion
	}
}

