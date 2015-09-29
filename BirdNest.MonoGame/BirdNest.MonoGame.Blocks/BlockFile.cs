using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Blocks
{
	public class BlockFile
	{
		public BlockIdentifier Identifier { get; set; }
		public TextureChapterInfo[] Chapters {get;set;}
		public ShaderInfo[] Shaders {get;set;}
	}
}

