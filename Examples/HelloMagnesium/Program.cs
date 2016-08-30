using System;
using DryIoc;
using Microsoft.Xna.Framework;
using MonoGame.Graphics;
using MonoGame.Audio.OpenAL;
using MonoGame.Audio.OpenAL.DesktopGL;
using OpenTK;
using MonoGame.Platform.DesktopGL;
using Magnesium;
using Microsoft.Xna.Framework.Content;

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

					container.Register<HelloMagnesium.IMgPresentationLayer, HelloMagnesium.MgPresentationLayer>(Reuse.Singleton);

					// Magnesium
					container.Register<Magnesium.IMgDriver, Magnesium.MgDriver>(Reuse.Singleton);
					container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(Reuse.Singleton);

					container.Register<Magnesium.IMgEntrypoint, Magnesium.OpenGL.GLEntrypoint>(Reuse.Singleton);
					container.Register<Magnesium.IMgTextureGenerator, Magnesium.MgLinearImageOptimizer>(Reuse.Singleton);

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

					// GLSL 
					container.Register<Magnesium.OpenGL.IGLGraphicsPipelineCompiler, Magnesium.OpenGL.GLSLGraphicsPipelineCompilier>(Reuse.Singleton);

					//// AUDIO
					container.Register<MonoGame.Audio.OpenAL.IOpenALSoundContext, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOpenALSoundContext>(Reuse.Singleton);
					container.Register<MonoGame.Audio.OpenAL.IOpenALSoundController, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundController>(Reuse.Singleton);
					container.Register<MonoGame.Audio.OpenAL.IOALSourceArray, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSourcesArray>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePoolPlatform, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLSoundEffectInstancePoolPlatform>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePool, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundEffectInstancePool>(Reuse.Singleton);
					//container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

					// RUNTIME
					container.Register<Microsoft.Xna.Framework.IGameBackbone, Microsoft.Xna.Framework.GameBackbone>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Content.IContentManager, Microsoft.Xna.Framework.Content.NullContentManager>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Content.IContentTypeReaderManager, Microsoft.Xna.Framework.Content.NullContentTypeReaderManager>(Reuse.Singleton);

					// MonoGame.Platform.DesktopGL
					container.Register<Microsoft.Xna.Framework.IGamePlatform, MonoGame.Platform.DesktopGL.OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IPlatformActivator, Microsoft.Xna.Framework.PlatformActivator>(Reuse.Singleton);
					container.Register<MonoGame.Core.IThreadSleeper, MonoGame.Platform.DesktopGL.DesktopGLThreadSleeper>(Reuse.Singleton);
					container.Register<MonoGame.Platform.DesktopGL.IWindowExitStrategy, MonoGame.Platform.DesktopGL.DesktopGLExitStrategy>(Reuse.Singleton);

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

					// MAGNESIUM TEXTURES 
					container.Register<MonoGame.Graphics.IMgBaseTextureLoader, MonoGame.Textures.FreeImageNET.FITexture2DLoader>(Reuse.Singleton);
					container.Register<MonoGame.Content.IContentStreamer, MonoGame.Content.ContentStreamer>(Reuse.Singleton);
					container.Register<MonoGame.Content.IBlockLocator, MonoGame.Content.MaskedBlockLocator>(Reuse.Singleton);
					container.Register<MonoGame.Content.IFileSystem, MonoGame.Content.Dirs.DirectoryFileSystem>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.ITitleContainer, MonoGame.Platform.DesktopGL.DesktopGLTitleContainer>(Reuse.Singleton);
					container.Register<MonoGame.Core.ITextureSortingKeyGenerator, MonoGame.Core.DefaultTextureSortingKeyGenerator>(Reuse.Singleton);




					//// DESKTOPGL SPECIFIC






					//container.Register<IGraphicsDeviceService, NullGraphicsDeviceService>(Reuse.Singleton);



					//container.Register<IGraphicsCapabilities, GraphicsCapabilities>(Reuse.Singleton);
					////container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					////container.Register<IGraphicsDevicePlatform, MagnesiumGraphicsDevicePlatform> (Reuse.Singleton);

					////container.Register<IGraphicsCapabilitiesLookup, FullGLSpecificGraphicsCapabilitiesLookup>(Reuse.Singleton);


					//container.Register<IGraphicsDevicePreferences, MockGraphicsDevicePreferences>(Reuse.Singleton);

					////container.Register<IGraphicsDeviceLogger, MockGraphicsDeviceLogger>(Reuse.Singleton);




					//// TOUCH 
					//container.Register<ITouchListener, MockTouchListener>(Reuse.Singleton);

					//// WINDOW EXIT



					//container.Register<IOpenTKDeviceQuery, OpenTKDeviceQuery>(Reuse.Singleton);



					//container.Register<IWindowOrientationListener, DefaultWindowOrientationListener> (Reuse.Singleton);




					//// MOCK 



					////container.Register<ISamplerStateCollectionPlatform, MockSamplerStateCollectionPlatform>(Reuse.Singleton);
					////container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);






					//// MAGNESIUM


					////container.Register<IMgPresentationSurface, Win32PresentationSurface>(Reuse.Singleton);










					//container.Register<IMgDeviceQuery, MgDeviceQuery>(Reuse.Singleton);












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
