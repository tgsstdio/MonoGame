using OpenTK.Graphics.ES20;
using FramebufferAttachment = OpenTK.Graphics.ES20.All;
using RenderbufferStorage = OpenTK.Graphics.ES20.All;
using GLPrimitiveType = OpenTK.Graphics.ES20.BeginMode;

namespace MonoGame.Platform.AndroidGL.Graphics
{
	public class Es20GLDevicePlatform : IAndroidGLDevicePlatform
	{
		public void Initialize ()
		{
			int maxTextures = 16;
			GL.GetInteger(All.MaxTextureImageUnits, ref maxTextures);
			GraphicsExtensions.CheckGLError();
			MaxTextureSlots = maxTextures;

			int maxVertexAttribs = 0;
			GL.GetInteger(All.MaxVertexAttribs, ref maxVertexAttribs);
			GraphicsExtensions.CheckGLError();
			MaxVertexAttributes = maxVertexAttribs;

			int texSize = 0;
			GL.GetInteger(All.MaxTextureSize, ref texSize);
			GraphicsExtensions.CheckGLError();		
			MaxTextureSize = texSize;

		}

		public void AfterApplyRenderTargets (int renderCount)
		{
			//GL.DrawBuffers(renderCount, this._drawBuffers);
		}

		#region IGLDevicePlatform implementation

		public int MaxVertexAttributes {
			get;
			private set;
		}

		public int MaxTextureSize {
			get;
			private set;
		}

		public int MaxTextureSlots {
			get;
			private set;
		}

		#endregion
	}
}

