using System;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class IgnoreETCUnpacking : IETCUnpacker 
	{
		public IgnoreETCUnpacking ()
		{
			Profile = new GLContextCapabilities ();	
		}

		#region IETCUnpacker implementation

		public bool IsRequired (KTXLoadInstructions instructions, ErrorCode error)
		{
			return false;
		}

		public KTXError UnpackCompressedTexture (KTXLoadInstructions instructions, int level, int face, int pixelWidth, int pixelHeight, byte[] data)
		{
			throw new NotSupportedException ();
		}

		public GLContextCapabilities Profile {
			get;
			private set;
		}

		#endregion


	}
}

