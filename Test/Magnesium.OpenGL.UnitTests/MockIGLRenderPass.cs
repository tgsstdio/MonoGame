using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	public class MockIGLRenderPass : IGLRenderPass
	{
		public MockIGLRenderPass ()
		{
			AttachmentFormats = new GLClearAttachmentType[]{ };
		}

		#region IMgRenderPass implementation

		public void DestroyRenderPass (IMgDevice device, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region IGLRenderPass implementation

		public GLClearAttachmentType[] AttachmentFormats {
			get;
			set;
		}

		#endregion

	}

}

