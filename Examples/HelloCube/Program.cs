using DryIoc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Platform.DesktopGL;
using System;
using MonoGame.Platform.DesktopGL.Graphics;
using Microsoft.Xna.Framework.Graphics;
using OpenTK;
using Microsoft.Xna.Framework.Input;
using MonoGame.Platform.DesktopGL.Input;

namespace HelloCube
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try 
			{
				using (var container = new Container ())
				{
					// GAME
					container.Register<Game, HelloCubeGame> (Reuse.Singleton);

					// DESKTOPGL SPECIFIC
					container.Register<BaseOpenTKGameWindow, OpenTKGameWindow>(Reuse.Singleton);
					container.Register<GamePlatform, OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<IGraphicsDeviceManager, DesktopGLGraphicsDeviceManager>(Reuse.Singleton);
					container.Register<IPlatformActivator, PlatformActivator>(Reuse.Singleton);
					container.Register<IWindowExitStrategy, DesktopGLExitStrategy>(Reuse.Singleton);
					container.Register<IOpenTKWindowResetter, DesktopGLWindowResetter>(Reuse.Singleton);
					container.Register<IMouseListener, DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<IGraphicsDeviceQuery, DesktopGLGraphicsDeviceQuery>(Reuse.Singleton);

					// MOCK 
					container.Register<IGraphicsDevice, NullGraphicsDevice> (Reuse.Singleton);
					container.Register<IContentManager, NullContentManager> (Reuse.Singleton);
					container.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
					container.Register<ISamplerStateCollectionPlatform, NullSamplerStateCollectionPlatform>(Reuse.Singleton);
					container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);
					container.Register<IGraphicsDevicePlatform, FullDesktopGLGraphicsDevicePlatform>(Reuse.Singleton);

					container.Register<IBackBufferPreferences, DefaultBackBufferPreferences>(Reuse.Singleton);
					container.Register<IPresentationParameters, PresentationParameters>(Reuse.Singleton);

					// RUNTIME
					container.Register<IDrawSuppressor, DrawSupressor>();
					container.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);
					using (var scope = container.OpenScope ())
					{
						using (var window = new NativeWindow())
						{
							container.RegisterInstance<INativeWindow>(window);
							using (var view = container.Resolve<BaseOpenTKGameWindow>())
							{
								container.RegisterInstance<Microsoft.Xna.Framework.GameWindow>(view);
								using (var backbone = container.Resolve<IGameBackbone> ())
								{
									var exitStrategy = container.Resolve<IWindowExitStrategy>();
									exitStrategy.Initialise();

									backbone.Run ();
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex.Message);
			}
		}
	}
}
