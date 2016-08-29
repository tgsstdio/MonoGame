using System;
using DryIoc;
using Microsoft.Xna.Framework;
using MonoGame.Graphics;
using MonoGame.Audio.OpenAL;
using MonoGame.Audio.OpenAL.DesktopGL;
using OpenTK;
using MonoGame.Platform.DesktopGL;
using Magnesium;

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

					// Magnesium
					container.Register<Magnesium.IMgDriver, Magnesium.MgDriver>(Reuse.Singleton);

					container.Register<Magnesium.IMgEntrypoint, Magnesium.OpenGL.GLEntrypoint>(Reuse.Singleton);
					// Magnesium.OpenGL
					container.Register<Magnesium.OpenGL.IGLQueue, Magnesium.OpenGL.GLQueue>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLQueueRenderer, Magnesium.OpenGL.GLQueueRenderer>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdBlendEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdBlendEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdStencilEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdStencilEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdRasterizationEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdRasterizationEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLErrorHandler, Magnesium.OpenGL.DesktopGL.FullGLErrorHandler>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdDepthEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdDepthEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdShaderProgramCache, Magnesium.OpenGL.DesktopGL.FullGLCmdShaderProgramCache>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdScissorsEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdScissorsEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdDrawEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdDrawEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdClearEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdClearEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLSemaphoreEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLSemaphoreEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdImageEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdImageEntrypoint>(Reuse.Singleton);

					container.Register<Magnesium.OpenGL.IGLDeviceEntrypoint, Magnesium.OpenGL.DefaultGLDeviceEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLCmdVBOEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdVBOEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLSamplerEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLSamplerEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLDeviceImageEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLDeviceImageEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLDeviceImageViewEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLDeviceImageViewEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLImageFormatEntrypoint, Magnesium.OpenGL.DesktopGL.DesktopGLImageFormatEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLImageDescriptorEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLImageDescriptorEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLShaderModuleEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLShaderModuleEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLDescriptorPoolEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLDescriptorPoolEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLBufferEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLBufferEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLDeviceMemoryEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLDeviceMemoryEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLGraphicsPipelineEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLGraphicsPipelineEntrypoint>(Reuse.Singleton);

					//// AUDIO
					container.Register<IOpenALSoundContext, DesktopGLOpenALSoundContext>(Reuse.Singleton);
					//container.Register<IOpenALSoundController, DesktopGLOALSoundController>(Reuse.Singleton);
					//container.Register<IOALSourceArray, DesktopGLOALSourcesArray>(Reuse.Singleton);
					//container.Register<ISoundEffectInstancePoolPlatform, DesktopGLSoundEffectInstancePoolPlatform>(Reuse.Singleton);
					//container.Register<ISoundEffectInstancePool, DesktopGLOALSoundEffectInstancePool>(Reuse.Singleton);
					//container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

					// MonoGame.Platform.DesktopGL
					container.Register<IGraphicsDeviceManager, MonoGame.Platform.DesktopGL.MgDesktopGLGraphicsDeviceManager>(Reuse.Singleton);
					container.Register<MonoGame.Platform.DesktopGL.IOpenTKWindowResetter, MonoGame.Platform.DesktopGL.DesktopGLWindowResetter>(Reuse.Singleton);
					container.Register<MonoGame.Platform.DesktopGL.IOpenTKGameWindow, MonoGame.Platform.DesktopGL.OpenTKGameWindow>(Reuse.Singleton);
					container.RegisterMapping<Microsoft.Xna.Framework.IGameWindow, MonoGame.Platform.DesktopGL.IOpenTKGameWindow>();
					container.Register<Microsoft.Xna.Framework.Input.IMouseListener, MonoGame.Platform.DesktopGL.Input.DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Input.IKeyboardInputListener, Microsoft.Xna.Framework.Input.KeyboardInputListener>(Reuse.Singleton);

					// IMgGraphicsDevice
					container.Register<Magnesium.IMgGraphicsDevice, Magnesium.OpenGL.DesktopGL.OpenTKGraphicsDevice>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLFramebufferHelperSelector, Magnesium.OpenGL.DesktopGL.FullGLFramebufferHelperSelector>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLFramebufferSupport, Magnesium.OpenGL.DesktopGL.FullGLFramebufferSupport>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.IGLExtensionLookup, Magnesium.OpenGL.DesktopGL.FullGLExtensionLookup>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.DesktopGL.IGLDevicePlatform, Magnesium.OpenGL.DesktopGL.FullGLDevicePlatform>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.DesktopGL.IMgGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);

					// IMgSwapchainCollection
					container.Register<Magnesium.IMgSwapchainCollection, Magnesium.OpenGL.DesktopGL.OpenTKSwapchainCollection>(Reuse.Singleton);
					container.Register<Magnesium.OpenGL.DesktopGL.IOpenTKSwapchainKHR, Magnesium.OpenGL.DesktopGL.GLSwapchainKHR>(Reuse.Singleton);

					// MonoGame
					container.Register<Microsoft.Xna.Framework.Graphics.IPresentationParameters, MonoGame.Core.Graphics.DefaultPresentationParameters>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IBackBufferPreferences, MonoGame.Platform.DesktopGL.DesktopGLBackBufferPreferences>();
					container.Register<Microsoft.Xna.Framework.Graphics.IGraphicsAdapterCollection, MonoGame.Platform.DesktopGL.Graphics.DesktopGLGraphicsAdapterCollection>(Reuse.Singleton);
					container.Register<MonoGame.Core.IGraphicsProfiler, MonoGame.Core.DefaultGraphicsDeviceProfiler> (Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IDrawSuppressor, Microsoft.Xna.Framework.DrawSupressor>(Reuse.Singleton);
					container.Register<MonoGame.Core.IClientWindowBounds, MonoGame.Core.DefaultClientWindowBounds>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IGraphicsDeviceQuery, MonoGame.Core.DefaultGraphicsDeviceQuery>(Reuse.Singleton);

					// RUNTIME
					container.Register<Microsoft.Xna.Framework.IGameBackbone, Microsoft.Xna.Framework.GameBackbone> (Reuse.Singleton);


					//// DESKTOPGL SPECIFIC


					//container.Register<IGamePlatform, OpenTKGamePlatform>(Reuse.Singleton);



					//container.Register<IGraphicsDeviceService, NullGraphicsDeviceService>(Reuse.Singleton);
					//container.Register<IPlatformActivator, PlatformActivator>(Reuse.Singleton);


					//container.Register<IGraphicsCapabilities, GraphicsCapabilities>(Reuse.Singleton);
					////container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					////container.Register<IGraphicsDevicePlatform, MagnesiumGraphicsDevicePlatform> (Reuse.Singleton);

					////container.Register<IGraphicsCapabilitiesLookup, FullGLSpecificGraphicsCapabilitiesLookup>(Reuse.Singleton);


					//container.Register<IGraphicsDevicePreferences, MockGraphicsDevicePreferences>(Reuse.Singleton);

					////container.Register<IGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);




					//// TOUCH 
					//container.Register<ITouchListener, MockTouchListener>(Reuse.Singleton);

					//// WINDOW EXIT

					//container.Register<IWindowExitStrategy, DesktopGLExitStrategy>(Reuse.Singleton);

					//container.Register<IOpenTKDeviceQuery, OpenTKDeviceQuery>(Reuse.Singleton);



					//container.Register<IWindowOrientationListener, DefaultWindowOrientationListener> (Reuse.Singleton);




					//// MOCK 

					//container.Register<IContentManager, NullContentManager> (Reuse.Singleton);
					//container.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
					////container.Register<ISamplerStateCollectionPlatform, MockSamplerStateCollectionPlatform>(Reuse.Singleton);
					////container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);




					//container.Register<IThreadSleeper, DesktopGLThreadSleeper>(Reuse.Singleton);

					//// MAGNESIUM


					////container.Register<IMgPresentationSurface, Win32PresentationSurface>(Reuse.Singleton);










					//container.Register<IMgDeviceQuery, MgDeviceQuery>(Reuse.Singleton);
					//container.Register<IMgImageTools, MgImageTools>(Reuse.Singleton);


					//container.Register<IMgPresentationLayer, MgPresentationLayer>(Reuse.Singleton);


					// MAGNESIUM TEXTURES 
					container.Register<IMgBaseTextureLoader, FITexture2DLoader>(Reuse.Singleton);
					//container.Register<ITextureSortingKeyGenerator, DefaultTextureSortingKeyGenerator>(Reuse.Singleton);
					//container.Register<IMgTextureGenerator, MgLinearImageOptimizer>(Reuse.Singleton);
					//container.Register<IContentStreamer, ContentStreamer>(Reuse.Singleton);
					//container.Register<IBlockLocator, MaskedBlockLocator>(Reuse.Singleton);
					//container.Register<IFileSystem, DirectoryFileSystem>(Reuse.Singleton);
					//container.Register<ITitleContainer, DesktopGLTitleContainer>(Reuse.Singleton);

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
