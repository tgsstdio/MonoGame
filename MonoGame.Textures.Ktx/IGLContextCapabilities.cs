namespace MonoGame.Textures.Ktx
{
	public interface IGLContextCapabilities
	{
		bool HasExtension(string ex);
		GLSizedFormats SizedFormats { get; }
		GLR16Formats R16Formats { get; }
		bool SupportsSRGB { get; }
	}
}

