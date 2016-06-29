using System.IO;

namespace MonoGame.Textures.FreeImageNET
{
	public interface IMgTextureGenerator
	{
		MgTexture Load(byte[] imageData, MgImageSource source);
	}

}

