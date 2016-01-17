
namespace MonoGame.Textures.Ktx
{
	public interface IKtxPlatform
	{
		uint GetTexTargetTexture2DArrayEnumValue ();

		uint GetTexTargetTexture1DArrayEnumValue ();

		uint GetTexTargetTexture1DEnumValue ();

		uint GetTexTargetTexture2DEnumValue ();

		uint GetTexTargetTexture3DEnumValue ();

		bool IsErrorFound (int gLError);

		int GetError ();

		int GetNoErrorEnumValue ();

		uint GetTextureCubeMapFirstFace ();

		uint GetTexTargetCubeMapEnumValue ();

		AtlasTextureTarget ConvertTargetType (uint glTarget);

		bool CheckInternalFormat (KTXHeader header);

		int GetUnpackAlignment ();

		void SetUnpackedAlignment (int alignment);
	}
}

