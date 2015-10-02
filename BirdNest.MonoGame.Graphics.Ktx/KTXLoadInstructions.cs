using System;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class KTXLoadInstructions
	{
		public KTXError Result;
		public UInt32 TextureDimensions;
		public UInt32 GlTarget;
		public uint GlInternalFormat;
		public uint GlFormat;
		public bool IsCompressed;
		public bool GenerateMipmaps;
	};
}

