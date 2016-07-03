
namespace Magnesium.OpenGL.UnitTests
{
	public class MockRenderPass : IMgRenderPass
	{
		#region IMgRenderPass implementation
		public void DestroyRenderPass (IMgDevice device, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

