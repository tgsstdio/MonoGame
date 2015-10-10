using System;
using DryIoc;
using BirdNest.MonoGame.FileSystem;
using BirdNest.MonoGame.FileSystem.Dirs;
using BirdNest.MonoGame;
using BirdNest.MonoGame.Graphics;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Blocks.JsonNET;

namespace ShadowMapping
{
	class MainClass
	{
		[STAThread]
		public static void Main()
		{
			using (var container = new Container ())
			{
				container.Register<IAssetLocator, AssetLocator> (Reuse.InCurrentScope);
				container.Register<IBlockFileSerializer, JsonNETSerializer> (Reuse.InCurrentScope);
				container.Register<IShaderInfoLookup, ShaderInfoLookup> (Reuse.InCurrentScope);
				container.Register<IShaderLoader, GLSLTextShaderLoader> (Reuse.InCurrentScope);
				container.Register<IFileSystem, DirectoryFileSystem> (Reuse.InCurrentScope);
				container.Register<IShaderRegistry, GLShaderRegistry> (Reuse.InCurrentScope);

				container.Register<ITexturePageLookup, NullTexturePageLookup> (Reuse.InCurrentScope); // NOT IN USE

				container.Register<IAssetManager, AssetManager> (Reuse.InCurrentScope);
				container.Register<GameWorld> ();

				using (var scope = container.OpenScope ())
				{
					var fs = container.Resolve<IFileSystem> ();
					fs.Initialise ("Media");

					using (var game = container.Resolve<GameWorld> ())
					{
						// Run the game at 60 updates per second
						game.Run (60.0);
					}
				}
			}
		}
	}
}
