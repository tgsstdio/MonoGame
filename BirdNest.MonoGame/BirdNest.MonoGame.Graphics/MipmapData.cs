namespace BirdNest.MonoGame.Graphics
{
	public class MipmapData
	{
		public int FaceIndex {
			get;
			set;
		}

		public int NumberOfFaces { get; set; }

		public AtlasTextureTarget Target { get; set; }

		public uint TextureDimensions {
			get;
			set;
		}

		public bool IsCompressed {
			get;
			set;
		}

		public int PixelDepth {
			get;
			set;
		}

		public int PixelHeight {
			get;
			set;
		}

		public int PixelWidth {
			get;
			set;
		}

		public int Size {
			get;
			set;
		}

		//public ErrorCode GLError {get;set;}

		public int ArrayIndex {get;set;}
		public int Level {get;set;}
		public byte[] Data {get;set;}
	}
}

