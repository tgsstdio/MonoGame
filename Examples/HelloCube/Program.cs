using System;
using DryIoc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Audio.OpenAL;
using MonoGame.Audio.OpenAL.DesktopGL;
using MonoGame.Platform.DesktopGL;
using MonoGame.Platform.DesktopGL.Graphics;
using MonoGame.Platform.DesktopGL.Input;
using OpenTK;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Graphics;
using MonoGame.Core;
using Microsoft.Xna.Framework.Input.Touch;

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
					container.Register<IOpenTKGameWindow, OpenTKGameWindow>(Reuse.Singleton);
					container.RegisterMapping<Microsoft.Xna.Framework.IGameWindow, IOpenTKGameWindow>();

					container.Register<IGamePlatform, OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<IGraphicsDeviceManager, DesktopGLGraphicsDeviceManager>(Reuse.Singleton);
					container.Register<IGraphicsDeviceService, NullGraphicsDeviceService>(Reuse.Singleton);
					container.Register<IPlatformActivator, PlatformActivator>(Reuse.Singleton);

					container.Register<IGraphicsAdapterCollection, DesktopGLGraphicsAdapterCollection>(Reuse.Singleton);
					container.Register<IGraphicsCapabilities, GraphicsCapabilities>(Reuse.Singleton);
					//container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					container.Register<IGraphicsDevicePlatform, DesktopGLGraphicsDevicePlatform> (Reuse.Singleton);
					container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					container.Register<IGraphicsCapabilitiesLookup, FullGLSpecificGraphicsCapabilitiesLookup>(Reuse.Singleton);
					container.Register<IGLDevicePlatform, FullGLDevicePlatform>(Reuse.Singleton);

					container.Register<IGraphicsDevicePreferences, MockGraphicsDevicePreferences>(Reuse.Singleton);
					container.Register<IBackBufferPreferences, DesktopGLBackBufferPreferences>();
					container.Register<IGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);
					container.Register<IGLFramebufferHelperSelector, FullGLFramebufferHelperSelector>(Reuse.Singleton);

					container.Register<IKeyboardInputListener, KeyboardInputListener>(Reuse.Singleton);
					container.Register<ITouchListener, TouchPanelState>(Reuse.Singleton);

					// WINDOW EXIT
					container.Register<IDrawSuppressor, DrawSupressor>(Reuse.Singleton);
					container.Register<IWindowExitStrategy, DesktopGLExitStrategy>(Reuse.Singleton);

					container.Register<IOpenTKWindowResetter, DesktopGLWindowResetter>(Reuse.Singleton);
					container.Register<IMouseListener, DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<IGraphicsDeviceQuery, DefaultGraphicsDeviceQuery>(Reuse.Singleton);

						// AUDIO
					container.Register<IOpenALSoundController, DesktopGLOALSoundController>(Reuse.Singleton);
					container.Register<IOpenALSoundContext, DesktopGLOpenALSoundContext>(Reuse.Singleton);
					container.Register<IOALSourceArray, DesktopGLOALSourcesArray>(Reuse.Singleton);
					container.Register<ISoundEffectInstancePoolPlatform, DesktopGLSoundEffectInstancePoolPlatform>(Reuse.Singleton);
					container.Register<ISoundEffectInstancePool, DesktopGLOALSoundEffectInstancePool>(Reuse.Singleton);
					container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

					// MOCK 

					container.Register<IContentManager, NullContentManager> (Reuse.Singleton);
					container.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
					container.Register<ISamplerStateCollectionPlatform, MockSamplerStateCollectionPlatform>(Reuse.Singleton);
					container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);

					container.Register<IPresentationParameters, PresentationParameters>(Reuse.Singleton);

					// RUNTIME
					container.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);
					container.Register<IThreadSleeper, SystemThreadSleeper>(Reuse.Singleton);

					using (var scope = container.OpenScope ())
					{
						using (var window = new NativeWindow())
						{							
							container.RegisterInstance<INativeWindow>(window);

							using (var audioContext = container.Resolve<IOpenALSoundContext>())
							using (var platform = container.Resolve<IGraphicsDevicePlatform>())
							{
								audioContext.Initialize();

								platform.Setup();

								var capabilities = container.Resolve<IGraphicsCapabilities>();
								capabilities.Initialize();

								platform.Initialize();

								using (var backbone = container.Resolve<IGameBackbone> ())
								{
									var exitStrategy = container.Resolve<IWindowExitStrategy>();
									exitStrategy.Initialize();

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
