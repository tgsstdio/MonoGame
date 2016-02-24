﻿using System;
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
					container.RegisterMapping<Microsoft.Xna.Framework.GameWindow, BaseOpenTKGameWindow>();

					container.Register<IGamePlatform, OpenTKGamePlatform>(Reuse.Singleton);
					container.Register<IGraphicsDeviceManager, DesktopGLGraphicsDeviceManager>(Reuse.Singleton);
					container.Register<IPlatformActivator, PlatformActivator>(Reuse.Singleton);

					container.Register<IGraphicsAdapterCollection, DesktopGLGraphicsAdapterCollection>(Reuse.Singleton);
					container.Register<IGraphicsCapabilities, GraphicsCapabilities>(Reuse.Singleton);
					//container.Register<IGLExtensionLookup, FullGLExtensionLookup>(Reuse.Singleton);
					container.Register<IGLExtensionLookup, NullGLExtensionLookup>(Reuse.Singleton);
					container.Register<IGraphicsCapabilitiesLookup, FullGLSpecificExtensionLookup>(Reuse.Singleton);

					container.Register<IGraphicsDevicePreferences, MockGraphicsDevicePreferences>(Reuse.Singleton);


					// WINDOW EXIT
					container.Register<IDrawSuppressor, DrawSupressor>(Reuse.Singleton);
					container.Register<IWindowExitStrategy, DesktopGLExitStrategy>(Reuse.Singleton);

					container.Register<IOpenTKWindowResetter, DesktopGLWindowResetter>(Reuse.Singleton);
					container.Register<IMouseListener, DesktopGLMouseListener>(Reuse.Singleton);
					container.Register<IGraphicsDeviceQuery, DesktopGLGraphicsDeviceQuery>(Reuse.Singleton);

						// AUDIO
					container.Register<IOpenALSoundController, DesktopGLOALSoundController>(Reuse.Singleton);
					container.Register<IOpenALSoundContext, DesktopGLOpenALSoundContext>(Reuse.Singleton);
					container.Register<IOALSourceArray, DesktopGLOALSourcesArray>(Reuse.Singleton);
					container.Register<ISoundEffectInstancePoolPlatform, DesktopGLSoundEffectInstancePoolPlatform>(Reuse.Singleton);
					container.Register<ISoundEffectInstancePool, DesktopGLOALSoundEffectInstancePool>(Reuse.Singleton);
					container.Register<ISoundEnvironment, SoundEnvironment>(Reuse.Singleton);

					// MOCK 
					container.Register<IGraphicsDevice, DesktopGLGraphicsDevice> (Reuse.Singleton);
					container.Register<IContentManager, NullContentManager> (Reuse.Singleton);
					container.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
					container.Register<ISamplerStateCollectionPlatform, NullSamplerStateCollectionPlatform>(Reuse.Singleton);
					container.Register<ITextureCollectionPlatform, NullTextureCollectionPlatform>(Reuse.Singleton);
					container.Register<IGraphicsDevicePlatform, FullDesktopGLGraphicsDevicePlatform>(Reuse.Singleton);

					container.Register<IBackBufferPreferences, DesktopGLBackBufferPreferences>();
					container.Register<IPresentationParameters, PresentationParameters>(Reuse.Singleton);

					// RUNTIME
					container.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);
					using (var scope = container.OpenScope ())
					{
						using (var window = new NativeWindow())
						{							
							container.RegisterInstance<INativeWindow>(window);

							using (var audioContext = container.Resolve<IOpenALSoundContext>())
							{
								audioContext.Initialise();
								using (var view = container.Resolve<BaseOpenTKGameWindow>())								
								{
									using (var backbone = container.Resolve<IGameBackbone> ())
									{
										var exitStrategy = container.Resolve<IWindowExitStrategy>();
										exitStrategy.Initialise();

//										var extensions = container.Resolve<IGLExtensionLookup>();
//										extensions.Initialise();
//	
//										var capabilities = container.Resolve<IGraphicsCapabilities>();
//										if (capabilities.SupportsFramebufferObjectARB)
//										{
//											container.Register<IGLFramebufferHelper, FullGLFramebufferHelper>(Reuse.Singleton);
//										}
//										//#if !(GLES || MONOMAC)
//										else if (capabilities.SupportsFramebufferObjectEXT)
//										{
//											container.Register<IGLFramebufferHelper, FullGLFramebufferHelperEXT>(Reuse.Singleton);
//										}
//										//#endif
//										else
//										{
//											throw new PlatformNotSupportedException(
//												"MonoGame requires either ARB_framebuffer_object or EXT_framebuffer_object." +
//												"Try updating your graphics drivers.");
//										}

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
			}
		}
	}
}
