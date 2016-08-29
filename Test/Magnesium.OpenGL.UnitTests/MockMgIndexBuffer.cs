
namespace Magnesium.OpenGL.UnitTests
{
	public class MockMgIndexBuffer : IMgBuffer
	{
		#region IMgBuffer implementation
		public void DestroyBuffer (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}
		public Result BindBufferMemory (IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

