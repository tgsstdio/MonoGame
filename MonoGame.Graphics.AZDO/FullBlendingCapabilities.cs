using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	public class FullBlendingCapabilities : IBlendCapabilities
	{
		#region IBlendCapabilities implementation

		private static BlendingFactorDest GetBlendFactorDest (Blend blend)
		{
			switch (blend) {
			case Blend.DestinationAlpha:
				return BlendingFactorDest.DstAlpha;
				//			case Blend.DestinationColor:
				//				return BlendingFactorDest.DstColor;
			case Blend.InverseDestinationAlpha:
				return BlendingFactorDest.OneMinusDstAlpha;
				//			case Blend.InverseDestinationColor:
				//				return BlendingFactorDest.OneMinusDstColor;
			case Blend.InverseSourceAlpha:
				return BlendingFactorDest.OneMinusSrcAlpha;
			case Blend.InverseSourceColor:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.OneMinusSrcColor;
				#else
				return BlendingFactorDest.OneMinusSrcColor;
				#endif
			case Blend.One:
				return BlendingFactorDest.One;
			case Blend.SourceAlpha:
				return BlendingFactorDest.SrcAlpha;
				//			case Blend.SourceAlphaSaturation:
				//				return BlendingFactorDest.SrcAlphaSaturate;
			case Blend.SourceColor:
				#if MONOMAC || WINDOWS
				return (BlendingFactorDest)All.SrcColor;
				#else
				return BlendingFactorDest.SrcColor;
				#endif
			case Blend.Zero:
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

		public void SetColorMask (DrawItemBitFlags colorMask)
		{
			GL.ColorMask (
				(colorMask & DrawItemBitFlags.RedColorWriteChannel) != 0,
				(colorMask & DrawItemBitFlags.GreenColorWriteChannel) != 0,
				(colorMask & DrawItemBitFlags.BlueColorWriteChannel) != 0,
				(colorMask & DrawItemBitFlags.AlphaColorWriteChannel) != 0);
		}

		private BlendingFactorSrc GetBlendFactorSrc (Blend blend)
		{
			switch (blend) {
			case Blend.DestinationAlpha:
				return BlendingFactorSrc.DstAlpha;
			case Blend.DestinationColor:
				return BlendingFactorSrc.DstColor;
			case Blend.InverseDestinationAlpha:
				return BlendingFactorSrc.OneMinusDstAlpha;
			case Blend.InverseDestinationColor:
				return BlendingFactorSrc.OneMinusDstColor;
			case Blend.InverseSourceAlpha:
				return BlendingFactorSrc.OneMinusSrcAlpha;
			case Blend.InverseSourceColor:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.OneMinusSrcColor;
				#else
				return BlendingFactorSrc.OneMinusSrcColor;
				#endif
			case Blend.One:
				return BlendingFactorSrc.One;
			case Blend.SourceAlpha:
				return BlendingFactorSrc.SrcAlpha;
			case Blend.SourceAlphaSaturation:
				return BlendingFactorSrc.SrcAlphaSaturate;
			case Blend.SourceColor:
				#if MONOMAC || WINDOWS || DESKTOPGL
				return (BlendingFactorSrc)All.SrcColor;
				#else
				return BlendingFactorSrc.SrcColor;
				#endif
			case Blend.Zero:
				return BlendingFactorSrc.Zero;
			default:
				return BlendingFactorSrc.One;
			}
		}

		public void ApplyBlendSeparateFunction (Blend colorSource, Blend colorDest, Blend alphaSource, Blend alphaDest)
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

