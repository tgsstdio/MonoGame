using Magnesium;
using Microsoft.Xna.Framework;
using MonoGame.Audio.OpenAL;
using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL;
using OpenTK;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using System;

namespace HelloMgSprites
{
    class Program
    {
        /// <summary>
        /// REQUIRES FREEIMAGE BINARY TO RUN
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            //Console.ReadKey ();

            try
            {
                using (var container = new SimpleInjector.Container())
                {
                    container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();

                    // GAME
                    container.Register<Game, HelloMgSpriteGame>(Lifestyle.Scoped);

                    container.Register<IMgPresentationLayer, MgPresentationLayer>(Lifestyle.Singleton);

                    // Magnesium
                    container.Register<Magnesium.MgDriver>(Lifestyle.Singleton);
                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(Lifestyle.Singleton);

                    //SetupOpenGL(container);
                    SetupVulkan(container);

                    //// AUDIO
                    container.Register<MonoGame.Audio.OpenAL.IOpenALSoundContext, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOpenALSoundContext>(Lifestyle.Singleton);
                    container.Register<MonoGame.Audio.OpenAL.IOpenALSoundController, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundController>(Lifestyle.Singleton);
                    container.Register<MonoGame.Audio.OpenAL.IOALSourceArray, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSourcesArray>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePoolPlatform, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLSoundEffectInstancePoolPlatform>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.Audio.ISoundEffectInstancePool, MonoGame.Audio.OpenAL.DesktopGL.DesktopGLOALSoundEffectInstancePool>(Lifestyle.Singleton);
                    //container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

                    // RUNTIME
                    container.Register<Microsoft.Xna.Framework.IGameBackbone, Microsoft.Xna.Framework.GameBackbone>(Lifestyle.Scoped);
                    container.Register<Microsoft.Xna.Framework.Content.IContentManager, Microsoft.Xna.Framework.Content.NullContentManager>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.Content.IContentTypeReaderManager, Microsoft.Xna.Framework.Content.NullContentTypeReaderManager>(Lifestyle.Singleton);

                    // MonoGame.Platform.DesktopGL
                    container.Register<Microsoft.Xna.Framework.IGamePlatform, MonoGame.Platform.DesktopGL.OpenTKGamePlatform>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.IPlatformActivator, Microsoft.Xna.Framework.PlatformActivator>(Lifestyle.Singleton);
                    container.Register<MonoGame.Core.IThreadSleeper, MonoGame.Platform.DesktopGL.DesktopGLThreadSleeper>(Lifestyle.Singleton);
                    container.Register<MonoGame.Platform.DesktopGL.IWindowExitStrategy, MonoGame.Platform.DesktopGL.DesktopGLExitStrategy>(Lifestyle.Singleton);

                    container.Register<MonoGame.Graphics.IGraphicsDeviceManager, MonoGame.Platform.DesktopGL.MgDesktopGLGraphicsDeviceManager>(Lifestyle.Scoped);

                    // OPENTK BACKBUFFER STUFF
                    container.Register<MonoGame.Platform.DesktopGL.IOpenTKWindowResetter, MonoGame.Platform.DesktopGL.DesktopGLWindowResetter>(Lifestyle.Singleton);
                    container.Register<Magnesium.OpenGL.DesktopGL.IMgGraphicsDeviceLogger, Magnesium.OpenGL.DesktopGL.NullMgGraphicsDeviceLogger>(Lifestyle.Singleton);

                    {
                        var registration = Lifestyle.Singleton.CreateRegistration<MonoGame.Platform.DesktopGL.OpenTKGameWindow>(container);
                        container.AddRegistration(typeof(MonoGame.Platform.DesktopGL.IOpenTKGameWindow), registration);
                        container.AddRegistration(typeof(Microsoft.Xna.Framework.IGameWindow), registration);
                    }
                    container.Register<Microsoft.Xna.Framework.Input.IMouseListener, MonoGame.Platform.DesktopGL.Input.DesktopGLMouseListener>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.Input.IKeyboardInputListener, Microsoft.Xna.Framework.Input.KeyboardInputListener>(Lifestyle.Singleton);

                    // MonoGame
                    container.Register<Microsoft.Xna.Framework.Graphics.IPresentationParameters, MonoGame.Core.Graphics.DefaultPresentationParameters>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.IBackBufferPreferences, MonoGame.Platform.DesktopGL.DesktopGLBackBufferPreferences>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.Graphics.IGraphicsAdapterCollection, MonoGame.Platform.DesktopGL.Graphics.DesktopGLGraphicsAdapterCollection>(Lifestyle.Singleton);
                    container.Register<MonoGame.Core.IGraphicsProfiler, MonoGame.Core.DefaultGraphicsDeviceProfiler>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.IDrawSuppressor, Microsoft.Xna.Framework.DrawSupressor>(Lifestyle.Singleton);
                    container.Register<MonoGame.Core.IClientWindowBounds, MonoGame.Core.DefaultClientWindowBounds>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.IGraphicsDeviceQuery, MonoGame.Core.DefaultGraphicsDeviceQuery>(Lifestyle.Singleton);

                    // MAGNESIUM TEXTURES 
                    container.Register<MonoGame.Graphics.IMgBaseTextureLoader, MonoGame.Textures.FreeImageNET.FITexture2DLoader>(Lifestyle.Singleton);
                    container.Register<MonoGame.Content.IContentStreamer, MonoGame.Content.ContentStreamer>(Lifestyle.Singleton);
                    container.Register<MonoGame.Content.IBlockLocator, MonoGame.Content.MaskedBlockLocator>(Lifestyle.Singleton);
                    container.Register<MonoGame.Content.IFileSystem, MonoGame.Content.Dirs.DirectoryFileSystem>(Lifestyle.Singleton);
                    container.Register<Microsoft.Xna.Framework.ITitleContainer, MonoGame.Platform.DesktopGL.DesktopGLTitleContainer>(Lifestyle.Singleton);
                    container.Register<MonoGame.Core.ITextureSortingKeyGenerator, MonoGame.Core.DefaultTextureSortingKeyGenerator>(Lifestyle.Singleton);


                    using (var scope = container.BeginLifetimeScope())
                    {
                        using (var window = new NativeWindow())
                        using (var driver = container.GetInstance<MgDriver>())
                        {
                            container.RegisterSingleton<INativeWindow>(window);
                            driver.Initialize(new MgApplicationInfo
                            {
                                ApplicationName = "HelloMgSprite",
                                ApplicationVersion = 1,
                                EngineName = "MonoGame",
                                EngineVersion = 1,
                                ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                            },
                            MgEnableExtensionsOption.ALL);

                            using (var presentationSurface = container.GetInstance<IMgPresentationSurface>())
                            {
                                presentationSurface.Initialize();
                                var device = driver.CreateLogicalDevice(presentationSurface.Surface, MgEnableExtensionsOption.ALL);
                                var partition = device.Queues[0].CreatePartition(0,
                                        new MgDescriptorPoolCreateInfo
                                        {
                                            PoolSizes = new MgDescriptorPoolSize[]
                                           {
                                                    new MgDescriptorPoolSize { Type = MgDescriptorType.SAMPLER, DescriptorCount = 10 },
                                           },
                                            MaxSets = 100,
                                        });

                                //container.RegisterInstance<IMgLogicalDevice>(device, Reuse.Singleton);
                                container.RegisterSingleton<IMgThreadPartition>(partition);

                                using (var scopedContext = container.BeginLifetimeScope())
                                {
                                    using (var audioContext = container.GetInstance<IOpenALSoundContext>())
                                    using (var manager = container.GetInstance<IGraphicsDeviceManager>())
                                    {
                                        audioContext.Initialize();
                                        //									var capabilities = container.Resolve<IGraphicsCapabilities>();
                                        //									capabilities.Initialize();     
                                        using (var backbone = scopedContext.GetInstance<IGameBackbone>())
                                        {
                                            var exitStrategy = scopedContext.GetInstance<IWindowExitStrategy>();
                                            exitStrategy.Initialize();

                                            backbone.Run();
                                        }

                                    }
                                }
                                partition.Dispose();
                                device.Dispose();
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
            container.Register<Magnesium.IMgTextureGenerator, Magnesium.MgStagingBufferOptimizer>(Lifestyle.Singleton);

            // Magnesium.VUlkan
            container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(Lifestyle.Singleton);

            // IMgGraphicsDevice
            container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(Lifestyle.Scoped);

            // IMgSwapchainCollection
            container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(Lifestyle.Scoped);

            container.Register<Magnesium.IMgPresentationSurface, MgOpenTKPresentationSurface>(Lifestyle.Singleton);

            container.Register<IShaderContentStreamer, SPIRVShaderContentStreamer>(Lifestyle.Singleton);
        }
    }
}
