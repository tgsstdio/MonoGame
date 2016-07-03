
namespace Magnesium.OpenGL.UnitTests
{
	public class MockMgFrameBuffer : IMgFramebuffer
	{
		#region IMgFramebuffer implementation
		public void DestroyFramebuffer (IMgDevice device, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

