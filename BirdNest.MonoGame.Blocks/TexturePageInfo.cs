namespace MonoGame.Content.Blocks
{
	public class TexturePageInfo
	{
		public AssetInfo Asset{ get; set;}
		public string RelativePath {get;set;}
		// ignore in serialization
		public TextureCatalog Catalog {get;set;}
	}
}

