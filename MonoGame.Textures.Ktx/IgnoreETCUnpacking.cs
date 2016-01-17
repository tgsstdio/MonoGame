using System;

namespace MonoGame.Textures.Ktx
{
	public class IgnoreETCUnpacking : IETCUnpacker 
	{
		public IgnoreETCUnpacking (IGLContextCapabilities profile)
		{
			Profile = profile;	
		}

		#region IETCUnpacker implementation

		public bool IsRequired (KTXLoadInstructions instructions, int error)
		{
			return false;
		}

		public KTXError UnpackCompressedTexture (KTXLoadInstructions instructions, int level, int face, int pixelWidth, int pixelHeight, byte[] data)
		{
			throw new NotSupportedException ();
		}

		public IGLContextCapabilities Profile {
			get;
			private set;
		}

		#endregion


	}
}

