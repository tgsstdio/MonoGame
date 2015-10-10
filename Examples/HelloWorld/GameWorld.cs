using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace HelloWorld
{
	public class GameWorld : GameWindow
	{
		public GameWorld ()
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			VSync = VSyncMode.On;

			Unload += (sender, args) => 
			{

			};

			Resize += (sender, args) =>
			{
				GL.Viewport(0, 0, Width, Height);
			};

			KeyDown += (sender, args) => {
				if (args.Key == Key.Space)
				{
					this.Exit();
				}				
			};
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Ext.MatrixOrtho(MatrixMode.Modelview, -1.0, 1.0, -1.0, 1.0, 0.0, 4.0);				

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
		}
	}
}

