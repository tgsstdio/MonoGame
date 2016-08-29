
namespace Magnesium.OpenGL.UnitTests
{
	public class MockRenderPass : IMgRenderPass
	{
		#region IMgRenderPass implementation
		public void DestroyRenderPass (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

