namespace MonoGame.Textures.Ktx
{
	public interface IETCUnpacker
	{
		IGLContextCapabilities Profile {get;}
		bool IsRequired (KTXLoadInstructions instructions, int error);
		KTXError UnpackCompressedTexture (KTXLoadInstructions instructions, int level, int face, int pixelWidth, int pixelHeight, byte[] data);
	}
}

