using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	public class MockIGLRenderPass : IGLRenderPass
	{
		public MockIGLRenderPass ()
		{
			AttachmentFormats = new GLClearAttachmentInfo[]{ };
		}

		#region IMgRenderPass implementation

		public void DestroyRenderPass (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region IGLRenderPass implementation

		public GLClearAttachmentInfo[] AttachmentFormats {
			get;
			set;
		}

		#endregion

	}

}

