using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using BirdNest.MonoGame.Graphics;
using BirdNest.MonoGame.FileSystem;
using BirdNest.MonoGame.Core;

namespace ShadowMapping
{
	public class GameWorld : GameWindow
	{
		private IShaderLoader mShaderLoader;
		private IFileSystem mFileSystem;
		public GameWorld (IShaderLoader loader, IFileSystem fs)
		{
			mShaderLoader = loader;
			mFileSystem = fs;
		}

		void OnLoadDefault (EventArgs e)
		{
			base.OnLoad (e);
			VSync = VSyncMode.On;
			Unload += (sender, args) =>  {
			};
			Resize += (sender, args) =>  {
				GL.Viewport (0, 0, Width, Height);
			};
			KeyDown += (sender, args) =>  {
				if (args.Key == Key.Space)
				{
					this.Exit ();
				}
			};
		}

		protected override void OnLoad(EventArgs e)
		{
			OnLoadDefault (e);

			const float ZNEAR = 1;
			const float ZFAR = 1000;

			// setup camera
			var camera = new Vector3(3.5f, 6.5f, 4f);
			var cameraView = Matrix4.LookAt (camera, Vector3.Zero, Vector3.UnitY);
			var cameraProj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float) this.Width / (float) this.Height, ZNEAR, ZFAR);

			// setup depth buffer
			// setup frame buffer object 
			int fbo = GL.GenFramebuffer();
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, fbo);

			int depthTex = GL.GenTexture ();
			GL.BindTexture (TextureTarget.Texture2D, depthTex);
			const int WIDTH = 1024;
			const int HEIGHT = 1024;
			const int LEVEL = 0;
			GL.TexImage2D(TextureTarget.Texture2D, LEVEL, PixelInternalFormat.DepthComponent16, WIDTH, HEIGHT, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) All.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) All.ClampToEdge);

			// setup lights
			var lightPos = new Vector3(4,1,6);
			var lightView = Matrix4.LookAt (lightPos, Vector3.Zero, Vector3.UnitY);
			var lightProj = Matrix4.CreateOrthographic(WIDTH, HEIGHT, 0, 1);


			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthTex, LEVEL);

			// FOR DEPTH ONLY
			GL.DrawBuffer (DrawBufferMode.None);

			var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferErrorCode.FramebufferComplete)
			{
				throw new Exception ("Invalid Framebuffer");
			}
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, 0);

			// setup fence

			// setup shaders
			if (!mFileSystem.Register (new BlockIdentifier{ BlockId = 10001 }))
			{
				throw new Exception ("Block not found");
			}

			var depthShader = mShaderLoader.Load (new AssetIdentifier{ AssetId = 2000 });

			// setup models
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Ext.MatrixOrtho(MatrixMode.Modelview, -1.0, 1.0, -1.0, 1.0, 0.0, 4.0);				

			// render to depth buffer

			// render to screen texture

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			// sort
		}
	}
}

