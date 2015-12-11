using System;
using BirdNest.MonoGame.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	public class FrameBufferObject : IRenderTarget
	{
		public int FBO { get; private set; }
		public FrameBufferObject (Func<InstanceIdentifier> identityGenerator)
		{
			Id = identityGenerator ();

			FBO = GL.GenFramebuffer();
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, FBO);
		}

		public void Bind ()
		{
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, FBO);
		}

		public void Unbind ()
		{
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, 0);
		}

		public void Validate ()
		{
			Bind ();
			var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferErrorCode.FramebufferComplete)
			{
				throw new Exception ("Invalid Framebuffer");
			}
			Unbind ();
		}

		public TextureOutput GenerateDepthMap (string name, int binding, int width, int height, int level)
		{
			int depthTex = GL.GenTexture ();
			GL.BindTexture (TextureTarget.Texture2D, depthTex);

			GL.TexImage2D(TextureTarget.Texture2D, level, PixelInternalFormat.DepthComponent16, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) All.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) All.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) All.ClampToEdge);

			// FOR DEPTH ONLY
			GL.Ext.FramebufferDrawBuffer(FBO, DrawBufferMode.None);
			GL.Ext.NamedFramebufferTexture(FBO, FramebufferAttachment.DepthAttachment, depthTex, level);

			return new TextureOutput{ Name = name, Binding= binding, TextureId = depthTex, Width = width, Height = height, Level = level };
		}

		#region IRenderTarget implementation

		public void Switch ()
		{
			throw new NotImplementedException ();
		}

		public InstanceIdentifier Id {
			private set;
			get;
		}

		#endregion

		#region IDisposable implementation

		~FrameBufferObject()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseManagedResources()
		{

		}

		void ReleaseUnmanagedResources()
		{
			GL.DeleteFramebuffer (FBO);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		#endregion
	}
}

