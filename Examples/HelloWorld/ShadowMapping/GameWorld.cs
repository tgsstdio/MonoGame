using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using BirdNest.MonoGame.Graphics;
using BirdNest.MonoGame.FileSystem;
using BirdNest.MonoGame.Core;
using BirdNest.MonoGame;

namespace ShadowMapping
{
	public class GameWorld : GameWindow
	{
		private IShaderLoader mShaderLoader;
		private IFileSystem mFileSystem;
		private IRenderTargetRange mTargetRange;
		private Func<InstanceIdentifier> mIdGenerator;
		public GameWorld (IShaderLoader loader, IFileSystem fs, Func<InstanceIdentifier> idGenerator, IRenderTargetRange range)
		{
			mShaderLoader = loader;
			mFileSystem = fs;
			mTargetRange = range;
			mIdGenerator = idGenerator;
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

			var renderer = new SortedRenderer ();

			// setup shaders
			if (!mFileSystem.Register (new BlockIdentifier{ BlockId = 10001 }))
			{
				throw new Exception ("Block not found");
			}

			var depthShader = mShaderLoader.Load (new AssetIdentifier{ AssetId = 2000 });

			var litShader = mShaderLoader.Load (new AssetIdentifier{ AssetId = 2001 });

			const float ZNEAR = 1;
			const float ZFAR = 1000;

			// setup camera
			var camera = new CameraInfo();
			camera.Eye = new Vector3(3.5f, 6.5f, 4f);
			camera.Up = Vector3.UnitY;
			camera.Target = Vector3.Zero;
			camera.FieldOfView = MathHelper.DegreesToRadians (45.0f);
			camera.X = (float)this.Width;
			camera.Y = (float)this.Height;
			camera.ZNear = ZNEAR;
			camera.ZFar = ZFAR;

			camera.Update ();

			// setup frame buffer object 
			var fbo = new FrameBufferObject(mIdGenerator);
			mTargetRange.Add (fbo);

			const int WIDTH = 1024;
			const int HEIGHT = 1024;
			const int LEVEL = 0;

			// setup lights
			var lamp = new LightInfo ();
			lamp.Position = new Vector3(4,1,6);
			lamp.Target = Vector3.Zero;
			lamp.Up = Vector3.UnitY;
			lamp.PixelWidth = WIDTH; 
			lamp.PixelHeight = HEIGHT;
			lamp.Update ();

			var frame_0 = new Pass (mIdGenerator, fbo, lamp, depthShader);
			var shadowMap = fbo.GenerateDepthMap ("depth", 0, WIDTH, HEIGHT, LEVEL);

			renderer.Add (frame_0).Outputs (shadowMap);
			fbo.Validate ();

			var frame_1 = new Pass (mIdGenerator, mTargetRange.Default, camera, litShader);
			renderer.Add (frame_1).Requires (shadowMap);

			renderer.Sort ();


			// setup fence


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

