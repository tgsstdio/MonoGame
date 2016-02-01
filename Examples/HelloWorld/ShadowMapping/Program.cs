using System;
using DryIoc;
using BirdNest.MonoGame;
using MonoGame.Content.Blocks;
using MonoGame.Content.Blocks.JsonNET;
using MonoGame.Content;
using MonoGame.Content.Dirs;
using MonoGame.Textures;
using MonoGame.Shaders;
using MonoGame.Shaders.GLSL.DesktopGL;

namespace ShadowMapping
{
	class MainClass
	{
		[STAThread]
		public static void Main()
		{

			ulong MINIMUM = 10000;
			Func<InstanceIdentifier> identityGen = () => new InstanceIdentifier{ InstanceId = MINIMUM++ };

			using (var container = new Container ())
			{
				container.Register<IAssetLocator, AssetLocator> (Reuse.InCurrentScope);
				container.Register<IBlockFileSerializer, JsonNETSerializer> (Reuse.InCurrentScope);
				container.Register<IShaderInfoLookup, ShaderInfoLookup> (Reuse.InCurrentScope);
				container.Register<IShaderLoader, GLSLTextShaderLoader> (Reuse.InCurrentScope);
				container.Register<IFileSystem, DirectoryFileSystem> (Reuse.InCurrentScope);
				container.Register<IShaderRegistry, GLShaderRegistry> (Reuse.InCurrentScope);

				container.Register<ITexturePageLookup, NullTexturePageLookup> (Reuse.InCurrentScope); // NOT IN USE

				container.Register<IRenderTargetRange, RenderTargetRange>(Reuse.InCurrentScope);
				container.Register<IAssetManager, AssetManager> (Reuse.InCurrentScope);
				container.Register<GameWorld> ();

				container.RegisterInstance < Func<InstanceIdentifier> >(identityGen);

				using (var scope = container.OpenScope ())
				{
					var fs = container.Resolve<IFileSystem> ();
					fs.Initialise ("Media");

					using (var range = container.Resolve<IRenderTargetRange>())
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
