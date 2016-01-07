namespace MonoGame.Graphics.AZDO
{
	public interface IBlendCapabilities
	{	
		bool IsEnabled { get; }

		void Initialise();

		void EnableBlending (bool value);

		void SetColorMask (DrawItemBitFlags colorMask);

		void ApplyBlendSeparateFunction (
			Blend colorSource,
			Blend colorDest,
			Blend alphaSource,
			Blend alphaDest);
	}
}

