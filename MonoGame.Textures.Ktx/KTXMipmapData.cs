namespace MonoGame.Textures.Ktx
{
	public class KTXMipmapData
	{
		public KTXMipmapData ()
		{
			Common = new MipmapData ();
		}

		public MipmapData Common { get; private set; }
		public int Target { get; set; }
		public int GLError {get;set;}
		public int SizeRounded {
			get;
			set;
		}
	}
}

