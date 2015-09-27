namespace BirdNest.MonoGame.Graphics
{
	public interface ITexturePage
	{
		ITextureChapter Chapter {get;}
		void Initialise(MipmapData mipmaps);
		void Clear();

		void Load();
		void Unload();
	}

}

