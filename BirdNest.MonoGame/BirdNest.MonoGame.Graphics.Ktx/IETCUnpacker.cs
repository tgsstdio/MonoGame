using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface IETCUnpacker
	{
		GLContextCapabilities Profile {get;}
		bool IsRequired (KTXLoadInstructions instructions, ErrorCode error);
		KTXError UnpackCompressedTexture (KTXLoadInstructions instructions, int level, int face, int pixelWidth, int pixelHeight, byte[] data);
	}
}

