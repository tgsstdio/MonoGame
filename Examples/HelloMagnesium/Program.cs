using System;
using DryIoc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Platform.DesktopGL;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;
using MonoGame.Core;
using MonoGame.Audio.OpenAL;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Audio.OpenAL.DesktopGL;
using MonoGame.Platform.DesktopGL.Input;
using Microsoft.Xna.Framework.Content;
using OpenTK;
using Magnesium;
using MonoGame.Textures.FreeImageNET;
using Microsoft.Core.Graphics;
using MonoGame.Content;
using MonoGame.Content.Dirs;
using Magnesium.OpenGL;

namespace HelloMagnesium
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Console.ReadKey ();

			try 
			{
				using (var container = new Container ())
				{
					// GAME
					container.Register<Game, HelloMagnesiumGame> (Reuse.Singleton);

					// DESKTOPGL SPECIFIC
					container.Register<IOpenTKGameWindow, OpenTKGameWindow>(Reuse.Singleton);
					container.RegisterMapping<Microsoft.Xna.Framework.IGameWindow, IOpenTKGameWindow>();

					container.Register<IGamePlatform, OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<IGraphicsDeviceManager, MgGraphicsDeviceManager>(Reuse.Singleton);

					container.Register<IGraphicsProfiler, DefaultGraphicsDeviceProfiler> (Reuse.Singleton);
					container.Register<IGraphicsDeviceService, NullGraphicsDeviceService>(Reuse.Singleton);
					container.Register<IPlatformActivator, PlatformActivator>(Reuse.Singleton);

					container.Register<IGraphicsAdapterCollection, DesktopGLGraphicsAdapterCollection>(Reuse.Singleton);
					container.Register<IGraphicsCapabilities, GraphicsCapabilities>(Reuse.Singleton);
					//container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					//container.Register<IGraphicsDevicePlatform, MagnesiumGraphicsDevicePlatform> (Reuse.Singleton);
					container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					//container.Register<IGraphicsCapabilitiesLookup, FullGLSpecificGraphicsCapabilitiesLookup>(Reuse.Singleton);
					container.Register<IGLDevicePlatform, FullGLDevicePlatform>(Reuse.Singleton);

					container.Register<IGraphicsDevicePreferences, MockGraphicsDevicePreferences>(Reuse.Singleton);
					container.Register<IBackBufferPreferences, DesktopGLBackBufferPreferences>();
					//container.Register<IGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);
					container.Register<IGLFramebufferHelperSelector, FullGLFramebufferHelperSelector>(Reuse.Singleton);

					container.Register<IKeyboardInputListener, KeyboardInputListener>(Reuse.Singleton);

					// TOUCH 
					container.Register<ITouchListener, MockTouchListener>(Reuse.Singleton);

					// WINDOW EXIT
					container.Register<IDrawSuppressor, DrawSupressor>(Reuse.Singleton);
					container.Register<IWindowExitStrategy, DesktopGLExitStrategy>(Reuse.Singleton);

					container.Register<IOpenTKDeviceQuery, OpenTKDeviceQuery>(Reuse.Singleton);
					container.Register<IOpenTKWindowResetter, DesktopGLWindowResetter>(Reuse.Singleton);
					container.Register<IMouseListener, DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<IGraphicsDeviceQuery, DefaultGraphicsDeviceQuery>(Reuse.Singleton);
					container.Register<IWindowOrientationListener, DefaultWindowOrientationListener> (Reuse.Singleton);
					container.Register<IClientWindowBounds, DefaultClientWindowBounds> (Reuse.Singleton);

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
					//container.Register<ISamplerStateCollectionPlatform, MockSamplerStateCollectionPlatform>(Reuse.Singleton);
					//container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);

					container.Register<IPresentationParameters, DefaultPresentationParameters>(Reuse.Singleton);

					// RUNTIME
					container.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);
					container.Register<IThreadSleeper, DesktopGLThreadSleeper>(Reuse.Singleton);

					// MAGNESIUM
					container.Register<IMgDriver, MgDriver>(Reuse.Singleton);
					container.Register<IMgEntrypoint, Magnesium.OpenGL.GLEntrypoint>(Reuse.Singleton);
					container.Register<IGLFramebufferSupport, Magnesium.OpenGL.FullGLFramebufferSupport>(Reuse.Singleton);
					container.Register<IGLErrorHandler, Magnesium.OpenGL.FullGLErrorHandler>(Reuse.Singleton);
					container.Register<IMgGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);
					//container.Register<IMgPresentationSurface, Win32PresentationSurface>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLQueue, Magnesium.OpenGL.GLQueue>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLQueueRenderer, Magnesium.OpenGL.GLQueueRenderer>(Reuse.Singleton);

					container.Register<Magnesium.OpenGL.IBlendCapabilities, Magnesium.OpenGL.FullBlendingCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IRasterizerCapabilities, Magnesium.OpenGL.FullRasterizerCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IDepthCapabilities, Magnesium.OpenGL.FullDepthCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IStencilCapabilities, Magnesium.OpenGL.FullStencilCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IScissorsCapabilities, Magnesium.OpenGL.FullScissorsCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.ICmdDrawCapabilities, Magnesium.OpenGL.FullCmdDrawCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.ICmdVBOEntrypoint, Magnesium.OpenGL.FullCmdVBOCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdImageCapabilities, Magnesium.OpenGL.FullGLCmdImageCapabilities>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IShaderProgramCache, FullShaderProgramCache>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLSemaphoreEntrypoint, MockGLSemaphoreGenerator >(Reuse.Singleton);
					container.Register<IMgDeviceQuery, MgDeviceQuery>(Reuse.Singleton);
					container.Register<IMgImageTools, MgImageTools>(Reuse.Singleton);

					container.Register<IMgGraphicsDevice, OpenTKGraphicsDevice>(Reuse.Singleton);

					container.Register<IMgSwapchainCollection, OpenTKSwapchainCollection>(Reuse.Singleton);
					container.Register<IMgPresentationLayer, MgPresentationLayer>(Reuse.Singleton);
					container.Register<IOpenTKSwapchainKHR, GLSwapchainKHR>(Reuse.Singleton);

					// MAGNESIUM TEXTURES 
					container.Register<IMgBaseTextureLoader, FITexture2DLoader>(Reuse.Singleton);
					container.Register<ITextureSortingKeyGenerator, DefaultTextureSortingKeyGenerator>(Reuse.Singleton);
					container.Register<IMgTextureGenerator, MgLinearImageOptimizer>(Reuse.Singleton);
					container.Register<IContentStreamer, ContentStreamer>(Reuse.Singleton);
					container.Register<IBlockLocator, MaskedBlockLocator>(Reuse.Singleton);
					container.Register<IFileSystem, DirectoryFileSystem>(Reuse.Singleton);
					container.Register<ITitleContainer, DesktopGLTitleContainer>(Reuse.Singleton);

					using (var scope = container.OpenScope ())
					{					
						using (var window = new NativeWindow())						
						using (var driver = container.Resolve<IMgDriver>())								
						{							
							container.RegisterInstance<INativeWindow>(window);
							driver.Initialize(new MgApplicationInfo{
								ApplicationName = "HelloMagnesium",
								ApplicationVersion = 1,
								EngineName = "MonoGame",
								EngineVersion = 1,
								ApiVersion = 1,
							});

							using (var device = driver.CreateLogicalDevice())
							using (var partition = device.Queues[0].CreatePartition())
							{
								container.RegisterInstance<IMgThreadPartition>(partition);							
								using (var audioContext = container.Resolve<IOpenALSoundContext>())														
								using (var manager = container.Resolve<IGraphicsDeviceManager>())
								{
									audioContext.Initialize();

//									var capabilities = container.Resolve<IGraphicsCapabilities>();
//									capabilities.Initialize();

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
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.StackTrace);
			}
		}
	}
}
