using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class KTXMipmapData
	{
		public KTXMipmapData ()
		{
			Common = new MipmapData ();
		}

		public MipmapData Common { get; private set; }
		public TextureTarget Target { get; set; }
		public ErrorCode GLError {get;set;}
		public int SizeRounded {
			get;
			set;
		}
	}
}

