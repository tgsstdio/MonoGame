
namespace BirdNest.MonoGame.Graphics.Ktx
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

