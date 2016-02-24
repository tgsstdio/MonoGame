using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using MonoGame.Graphics;

namespace MonoGame.Platform.DesktopGL.Graphics
{
	public class FullGLFramebufferHelper : IGLFramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		public FullGLFramebufferHelper()
		{
			this.SupportsBlitFramebuffer = true;
			this.SupportsInvalidateFramebuffer = false;
		}

		public void GenRenderbuffer(out int renderbuffer)
		{
			GL.GenRenderbuffers(1, out renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, (RenderbufferStorage)internalFormat, width, height);
			GraphicsExtensions.CheckGLError();
		}

		public void GenFramebuffer(out int framebuffer)
		{
			GL.GenFramebuffers(1, out framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, readFramebuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public void DeleteFramebuffer(int framebuffer)
		{
			GL.DeleteFramebuffers(1, ref framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, (TextureTarget)target, texture, level);
			GraphicsExtensions.CheckGLError();
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		public void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((GenerateMipmapTarget)target);
			GraphicsExtensions.CheckGLError();
		}

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{

			GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + iColorAttachment);
			GraphicsExtensions.CheckGLError();
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0 + iColorAttachment);
			GraphicsExtensions.CheckGLError();

			GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

			GraphicsExtensions.CheckGLError();

		}

		public void CheckFramebufferStatus()
		{
			var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferErrorCode.FramebufferComplete)
			{
				string message = "Framebuffer Incomplete.";
				switch (status)
				{
				case FramebufferErrorCode.FramebufferIncompleteAttachment: message = "Not all framebuffer attachment points are framebuffer attachment complete."; break;
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: message = "No images are attached to the framebuffer."; break;
				case FramebufferErrorCode.FramebufferUnsupported: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break;
				case FramebufferErrorCode.FramebufferIncompleteMultisample: message = "Not all attached images have the same number of samples."; break;
				}
				throw new InvalidOperationException(message);
			}
		}
	}
}

