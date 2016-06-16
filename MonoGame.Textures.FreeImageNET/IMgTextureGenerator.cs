using Magnesium;
using System.IO;

namespace MonoGame.Textures.FreeImageNET
{
	public interface IMgTextureGenerator
	{
		MgTexture Load(Stream fs, MgImageSource source);
	}

}

