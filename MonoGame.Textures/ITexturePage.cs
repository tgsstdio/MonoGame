namespace MonoGame.Textures
{
	public interface ITexturePage
	{
		ITextureChapter Chapter {get;}
		void Initialize(MipmapData mipmaps);
		void Clear();

		void Load();
		void Unload();
	}

}

