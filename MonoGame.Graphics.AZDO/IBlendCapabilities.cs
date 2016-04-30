using Magnesium;

namespace MonoGame.Graphics.AZDO
{
	public interface IBlendCapabilities
	{	
		bool IsEnabled { get; }

		void Initialise();

		void EnableBlending (bool value);

		void SetColorMask (DrawItemBitFlags colorMask);

		void ApplyBlendSeparateFunction (
			MgBlendFactor colorSource,
			MgBlendFactor colorDest,
			MgBlendFactor alphaSource,
			MgBlendFactor alphaDest);
	}
}

