using System;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

#if MONOMAC
using MonoMac;
using MonoMac.OpenGL;
#endif

#if (WINDOWS || DESKTOPGL) && !GLES
using OpenTK.Graphics.OpenGL;

#endif

#if GLES
using OpenTK.Graphics.ES20;
using System.Security;
#endif

namespace MonoGame.Platform.DesktopGL.Graphics
{
	internal class FramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		#if MONOMAC
		[DllImport(Constants.OpenGLLibrary, EntryPoint = "glRenderbufferStorageMultisampleEXT")]
		internal extern static void GLRenderbufferStorageMultisampleExt(All target, int samples, All internalformat, int width, int height);

		[DllImport(Constants.OpenGLLibrary, EntryPoint = "glBlitFramebufferEXT")]
		internal extern static void GLBlitFramebufferExt(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);

		[DllImport(Constants.OpenGLLibrary, EntryPoint = "glGenerateMipmapEXT")]
		internal extern static void GLGenerateMipmapExt(GenerateMipmapTarget target);
		#endif

		internal FramebufferHelper(GraphicsDevice graphicsDevice)
		{
			this.SupportsBlitFramebuffer = true;
			this.SupportsInvalidateFramebuffer = false;
		}

		internal virtual void GenRenderbuffer(out int renderbuffer)
		{
			GL.GenRenderbuffers(1, out renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			#if !MONOMAC
			GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, (RenderbufferStorage)internalFormat, width, height);
			#else
			GLRenderbufferStorageMultisampleExt(All.Renderbuffer, samples, (All)internalFormat, width, height);
			#endif
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void GenFramebuffer(out int framebuffer)
		{
			GL.GenFramebuffers(1, out framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, readFramebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		internal virtual void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		internal virtual void DeleteFramebuffer(int framebuffer)
		{
			GL.DeleteFramebuffers(1, ref framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, (TextureTarget)target, texture, level);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void GenerateMipmap(int target)
		{
			#if !MONOMAC
			GL.GenerateMipmap((GenerateMipmapTarget)target);
			#else
			GLGenerateMipmapExt((GenerateMipmapTarget)target);
			#endif
			GraphicsExtensions.CheckGLError();

		}

		internal virtual void BlitFramebuffer(int iColorAttachment, int width, int height)
		{

			GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + iColorAttachment);
			GraphicsExtensions.CheckGLError();
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0 + iColorAttachment);
			GraphicsExtensions.CheckGLError();
			#if !MONOMAC
			GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
			#else
			GLBlitFramebufferExt(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
			#endif
			GraphicsExtensions.CheckGLError();

		}

		internal virtual void CheckFramebufferStatus()
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

