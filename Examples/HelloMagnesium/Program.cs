using System;
using DryIoc;
using Microsoft.Xna.Framework;
using MonoGame.Audio.OpenAL;
using OpenTK;
using MonoGame.Platform.DesktopGL;
using Magnesium;

namespace HelloMagnesium
{
    class MainClass
	{
		/// <summary>
		/// REQUIRES FREEIMAGE BINARY TO RUN
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			//Console.ReadKey ();

			try 
			{
				using (var container = new Container ())
				{
                    // GAME
                    container.Register<Microsoft.Xna.Framework.Game, HelloMagnesiumGame>(Reuse.InCurrentScope);

                    // Magnesium IN SCOPE RESOLUTION
                    container.Register<Magnesium.IMgPresentationLayer, Magnesium.MgPresentationLayer>(Reuse.InCurrentScope);
                    container.Register<Magnesium.IMgPresentationBarrierEntrypoint, Magnesium.MgPresentationBarrierEntrypoint>(Reuse.InCurrentScope);

                    // MONOGAME
                    container.Register<MonoGame.Graphics.IGraphicsDeviceManager, MonoGame.Platform.DesktopGL.MgDesktopGLGraphicsDeviceManager>(Reuse.InCurrentScope);
                    container.Register<Microsoft.Xna.Framework.IGameBackbone, Microsoft.Xna.Framework.GameBackbone>(Reuse.InCurrentScope);

                    // Magnesium
                    container.Register<Magnesium.MgDriverContext>(Reuse.Singleton);
                    container.Register<Magnesium.IMgGraphicsConfiguration, Magnesium.MgDefaultGraphicsConfiguration>(Reuse.Singleton);
                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(Reuse.Singleton);


                    // TODO: fix shader functions
                    SetupOpenGL(container);
                    //SetupVulkan(container);

                    //// AUDIO
                    container.Register<MonoGame.Audio.OpenAL.IOpenALSoundContext, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOpenALSoundContext>(Reuse.Singleton);
					container.Register<MonoGame.Audio.OpenAL.IOpenALSoundController, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundController>(Reuse.Singleton);
					container.Register<MonoGame.Audio.OpenAL.IOALSourceArray, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSourcesArray>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePoolPlatform, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLSoundEffectInstancePoolPlatform>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePool, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundEffectInstancePool>(Reuse.Singleton);
					//container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

					// RUNTIME
					container.Register<Microsoft.Xna.Framework.Content.IContentManager, Microsoft.Xna.Framework.Content.NullContentManager>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Content.IContentTypeReaderManager, Microsoft.Xna.Framework.Content.NullContentTypeReaderManager>(Reuse.Singleton);

					// MonoGame.Platform.DesktopGL
					container.Register<Microsoft.Xna.Framework.IGamePlatform, MonoGame.Platform.DesktopGL.OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IPlatformActivator, Microsoft.Xna.Framework.PlatformActivator>(Reuse.Singleton);
					container.Register<MonoGame.Core.IThreadSleeper, MonoGame.Platform.DesktopGL.DesktopGLThreadSleeper>(Reuse.Singleton);
					container.Register<MonoGame.Platform.DesktopGL.IWindowExitStrategy, MonoGame.Platform.DesktopGL.DesktopGLExitStrategy>(Reuse.Singleton);
                   
                    // OPENTK BACKBUFFER STUFF
                    container.Register<MonoGame.Platform.DesktopGL.IOpenTKWindowResetter, MonoGame.Platform.DesktopGL.DesktopGLWindowResetter>(Reuse.Singleton);

                    container.Register<MonoGame.Platform.DesktopGL.IOpenTKGameWindow, MonoGame.Platform.DesktopGL.OpenTKGameWindow>(Reuse.Singleton);
					container.RegisterMapping<Microsoft.Xna.Framework.IGameWindow, MonoGame.Platform.DesktopGL.IOpenTKGameWindow>();
					container.Register<Microsoft.Xna.Framework.Input.IMouseListener, MonoGame.Platform.DesktopGL.Input.DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.Input.IKeyboardInputListener, Microsoft.Xna.Framework.Input.KeyboardInputListener>(Reuse.Singleton);

					// MonoGame
					container.Register<Microsoft.Xna.Framework.Graphics.IPresentationParameters, MonoGame.Core.Graphics.DefaultPresentationParameters>(Reuse.Singleton);
					container.Register<Microsoft.Xna.Framework.IBackBufferPreferences, MonoGame.Platform.DesktopGL.DesktopGLBackBufferPreferences>();
					container.Register<Microsoft.Xna.Framework.Graphics.IGraphicsAdapterCollection, MonoGame.Platform.DesktopGL.Graphics.DesktopGLGraphicsAdapterCollection>(Reuse.Singleton);
					container.Register<MonoGame.Core.IGraphicsProfiler, MonoGame.Core.DefaultGraphicsDeviceProfiler>(Reuse.Singleton);
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


                    using (var outerScope = container.OpenScope())
                    {
                        using (var window = new NativeWindow())
                        {
                            outerScope.RegisterInstance<INativeWindow>(window);
                            using (var driver = outerScope.Resolve<MgDriverContext>())
                            {
                                var errorCode = driver.Initialize(
                                    new MgApplicationInfo
                                    {
                                        ApplicationName = "HelloMagnesium",
                                        EngineName = "Magnesium",
                                        ApplicationVersion = 1,
                                        EngineVersion = 1,
                                        ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                                    },
                                    MgInstanceExtensionOptions.ALL
                                    );

                                if (errorCode != Result.SUCCESS)
                                {
                                    throw new InvalidOperationException("mDriverContext error : " + errorCode);
                                }

                                using (var graphicsConfiguration = outerScope.Resolve<IMgGraphicsConfiguration>())
                                using (var innerScope = container.OpenScope())
                                {
                                    using (var audioContext = innerScope.Resolve<IOpenALSoundContext>())
                                    {
                                        audioContext.Initialize();
                                        using (var backbone = innerScope.Resolve<IGameBackbone>())
                                        {
                                            var exitStrategy = innerScope.Resolve<IWindowExitStrategy>();
                                            exitStrategy.Initialize();

                                            backbone.Run();
                                        }
                                    }
                                }
                            }
                        }
                    }                    
                }
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}

		static void SetupVulkan(Container container)
		{
			container.Register<Magnesium.IMgTextureGenerator, Magnesium.MgStagingBufferOptimizer>(Reuse.Singleton);

			// Magnesium.VUlkan
			container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(Reuse.Singleton);

			// IMgGraphicsDevice
			container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(Reuse.InCurrentScope);

			// IMgSwapchainCollection
			container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(Reuse.InCurrentScope);

            container.Register<MonoGame.Graphics.IShaderContentStreamer, MonoGame.Graphics.SPIRVShaderContentStreamer>(Reuse.Singleton);

            // WINDOW 
            container.Register<Magnesium.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(Reuse.Singleton);
        }

        static void SetupOpenGL(Container container)
        {
            // IMPORTANT
            container.Register<Magnesium.IMgGraphicsDevice, Magnesium.OpenGL.DesktopGL.OpenTKGraphicsDevice>(Reuse.InCurrentScope);
            container.Register<Magnesium.IMgSwapchainCollection, Magnesium.OpenGL.DesktopGL.OpenTKSwapchainCollection>(Reuse.InCurrentScope);
            container.Register<Magnesium.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.DesktopGLPresentationSurface>(Reuse.Singleton);

            container.Register<Magnesium.IMgTextureGenerator, Magnesium.MgLinearImageOptimizer>(Reuse.Singleton);

            // Magnesium.OpenGL
            container.Register<Magnesium.IMgEntrypoint, Magnesium.OpenGL.GLEntrypoint>(Reuse.Singleton);

            // Magnesium.OpenGL INTERNALS
            container.Register<Magnesium.OpenGL.IGLGraphicsPipelineCompiler, Magnesium.OpenGL.GLSLGraphicsPipelineCompilier>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLQueue, Magnesium.OpenGL.GLCmdQueue>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLRenderer, Magnesium.OpenGL.GLCmdRenderer>(Reuse.Singleton);

            container.Register<Magnesium.OpenGL.IGLCmdStateRenderer, Magnesium.OpenGL.GLCmdStateRenderer>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLDeviceEntrypoint, Magnesium.OpenGL.DefaultGLDeviceEntrypoint>(Reuse.Singleton);

            container.Register<Magnesium.OpenGL.IGLCmdBlendEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdBlendEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdStencilEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdStencilEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdRasterizationEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdRasterizationEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLErrorHandler, Magnesium.OpenGL.DesktopGL.FullGLErrorHandler>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdDepthEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdDepthEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLNextCmdShaderProgramCache, Magnesium.OpenGL.GLNextCmdShaderProgramCache>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdScissorsEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdScissorsEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdDrawEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdDrawEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdClearEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdClearEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLSemaphoreEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLSemaphoreEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdImageEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdImageEntrypoint>(Reuse.Singleton);
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
            container.Register<Magnesium.OpenGL.IGLFramebufferHelperSelector, Magnesium.OpenGL.DesktopGL.FullGLFramebufferHelperSelector>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLFramebufferSupport, Magnesium.OpenGL.DesktopGL.FullGLFramebufferSupport>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLExtensionLookup, Magnesium.OpenGL.DesktopGL.FullGLExtensionLookup>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLFenceEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLFullFenceEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLBlitOperationEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLBlitOperationEntrypoint>(Reuse.Singleton);

            // DESCRIPTOR SET
            container.Register<Magnesium.OpenGL.IGLUniformBlockEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLUniformBlockEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLUniformBlockNameParser, Magnesium.OpenGL.DefaultGLUniformBlockNameParser>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLCmdShaderProgramEntrypoint, Magnesium.OpenGL.DesktopGL.FullGLCmdShaderProgramEntrypoint>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.IGLDescriptorSetEntrypoint, Magnesium.OpenGL.DefaultGLDescriptorSetEntrypoint>(Reuse.Singleton);

            // Magnesium.OpenGL.DesktopGL INTERNALS
            container.Register<Magnesium.OpenGL.DesktopGL.IOpenTKSwapchainKHR, Magnesium.OpenGL.DesktopGL.GLSwapchainKHR>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.DesktopGL.IGLBackbufferContext, Magnesium.OpenGL.DesktopGL.OpenTKBackbufferContext>(Reuse.Singleton);
            container.Register<Magnesium.OpenGL.DesktopGL.IMgGraphicsDeviceLogger, Magnesium.OpenGL.DesktopGL.NullMgGraphicsDeviceLogger>(Reuse.Singleton);

            container.Register<MonoGame.Graphics.IShaderContentStreamer, MonoGame.Graphics.GLSLShaderContentStreamer>(Reuse.Singleton);
        }
    }
}
