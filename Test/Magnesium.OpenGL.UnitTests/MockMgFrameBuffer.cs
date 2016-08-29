
namespace Magnesium.OpenGL.UnitTests
{
	public class MockMgFrameBuffer : IMgFramebuffer
	{
		#region IMgFramebuffer implementation
		public void DestroyFramebuffer (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

