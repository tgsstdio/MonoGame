using System;
using OpenTK.Graphics.ES20;
using System.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL;

namespace MonoGame.Platform.IOS.Graphics
{
	public class IOSGLFramebufferHelper : IGLFramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		internal const string OpenGLLibrary = ObjCRuntime.Constants.OpenGLESLibrary;
		#region GL_EXT_discard_framebuffer

		internal const All AllColorExt = (All)0x1800;
		internal const All AllDepthExt = (All)0x1801;
		internal const All AllStencilExt = (All)0x1802;

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glDiscardFramebufferEXT", ExactSpelling = true)]
		internal extern static void GLDiscardFramebufferExt(All target, int numAttachments, [MarshalAs(UnmanagedType.LPArray)] All[] attachments);

		#endregion

		#region GL_APPLE_framebuffer_multisample

		internal const All AllFramebufferIncompleteMultisampleApple = (All)0x8D56;
		internal const All AllMaxSamplesApple = (All)0x8D57;
		internal const All AllReadFramebufferApple = (All)0x8CA8;
		internal const All AllDrawFramebufferApple = (All)0x8CA9;
		internal const All AllRenderBufferSamplesApple = (All)0x8CAB;

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glRenderbufferStorageMultisampleAPPLE", ExactSpelling = true)]
		internal extern static void GLRenderbufferStorageMultisampleApple(All target, int samples, All internalformat, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glResolveMultisampleFramebufferAPPLE", ExactSpelling = true)]
		internal extern static void GLResolveMultisampleFramebufferApple();

		internal void GLBlitFramebufferApple(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, TextureMagFilter filter)
		{
			GLResolveMultisampleFramebufferApple();
		}

		#endregion

		#region GL_NV_framebuffer_multisample

		internal const All AllFramebufferIncompleteMultisampleNV = (All)0x8D56;
		internal const All AllMaxSamplesNV = (All)0x8D57;
		internal const All AllReadFramebufferNV = (All)0x8CA8;
		internal const All AllDrawFramebufferNV = (All)0x8CA9;
		internal const All AllRenderBufferSamplesNV = (All)0x8CAB;

		#endregion

		#region GL_IMG_multisampled_render_to_texture

		internal const All AllFramebufferIncompleteMultisampleImg = (All)0x9134;
		internal const All AllMaxSamplesImg = (All)0x9135;

		#endregion

		#region GL_EXT_multisampled_render_to_texture

		internal const All AllFramebufferIncompleteMultisampleExt = (All)0x8D56;
		internal const All AllMaxSamplesExt = (All)0x8D57;

		#endregion

		internal delegate void GLInvalidateFramebufferDelegate(All target, int numAttachments, All[] attachments);
		internal delegate void GLRenderbufferStorageMultisampleDelegate(All target, int samples, All internalFormat, int width, int height);
		internal delegate void GLBlitFramebufferDelegate(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, TextureMagFilter filter);
		internal delegate void GLFramebufferTexture2DMultisampleDelegate(All target, All attachment, All textarget, int texture, int level, int samples);

		internal GLInvalidateFramebufferDelegate GLInvalidateFramebuffer;
		internal GLRenderbufferStorageMultisampleDelegate GLRenderbufferStorageMultisample;
		internal GLFramebufferTexture2DMultisampleDelegate GLFramebufferTexture2DMultisample;
		internal GLBlitFramebufferDelegate GLBlitFramebuffer;

		internal All AllReadFramebuffer = All.Framebuffer;
		internal All AllDrawFramebuffer = All.Framebuffer;

		public IOSGLFramebufferHelper(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice._extensions.Contains("GL_EXT_discard_framebuffer"))
			{
				this.GLInvalidateFramebuffer = new GLInvalidateFramebufferDelegate(GLDiscardFramebufferExt);
				this.SupportsInvalidateFramebuffer = true;
			}

			if (graphicsDevice._extensions.Contains("GL_APPLE_framebuffer_multisample"))
			{
				this.GLRenderbufferStorageMultisample = new GLRenderbufferStorageMultisampleDelegate(GLRenderbufferStorageMultisampleApple);
				this.GLBlitFramebuffer = new GLBlitFramebufferDelegate(GLBlitFramebufferApple);
				this.AllReadFramebuffer = AllReadFramebufferApple;
				this.AllDrawFramebuffer = AllDrawFramebufferApple;
			}

			this.SupportsBlitFramebuffer = this.GLBlitFramebuffer != null;
		}

		internal virtual void GenRenderbuffer(out int renderbuffer)
		{
			renderbuffer = 0;

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
			if (samples > 0 && this.GLRenderbufferStorageMultisample != null)
				GLRenderbufferStorageMultisample(All.Renderbuffer, samples, (All)internalFormat, width, height);
			else
				GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, (RenderbufferInternalFormat)internalFormat, width, height);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void GenFramebuffer(out int framebuffer)
		{
			framebuffer = 0;

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
			GL.BindFramebuffer((FramebufferTarget)AllReadFramebuffer, readFramebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal readonly All[] GLDiscardAttachementsDefault = { AllColorExt, AllDepthExt, AllStencilExt, };
		internal readonly All[] GLDiscardAttachements = { All.ColorAttachment0, All.DepthAttachment, All.StencilAttachment, };

		internal virtual void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
			this.GLInvalidateFramebuffer(AllDrawFramebuffer, 3, GLDiscardAttachements);
		}

		internal virtual void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
			this.GLInvalidateFramebuffer(AllReadFramebuffer, 3, GLDiscardAttachements);
		}

		internal virtual void DeleteFramebuffer(int framebuffer)
		{
			GL.DeleteFramebuffers(1, ref framebuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			if (samples > 0 && this.GLFramebufferTexture2DMultisample != null)
				this.GLFramebufferTexture2DMultisample(All.Framebuffer, (All)attachement, (All)target, texture, level, samples);
			else
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferSlot)attachement, (TextureTarget)target, texture, level);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (FramebufferSlot)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((TextureTarget)target);
			GraphicsExtensions.CheckGLError();
		}

		internal virtual void BlitFramebuffer(int iColorAttachment, int width, int height)
		{
			this.GLBlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, TextureMagFilter.Nearest);
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
				case FramebufferErrorCode.FramebufferIncompleteDimensions: message = "Not all attached images have the same width and height."; break;
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: message = "No images are attached to the framebuffer."; break;
				case FramebufferErrorCode.FramebufferUnsupported: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break; 
				}
				throw new InvalidOperationException(message);
			}
		}
	}

}

