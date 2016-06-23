namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLIndirectBuffer : IGLIndirectBuffer
	{
		#region IMgBuffer implementation
		public void DestroyBuffer (IMgDevice device, MgAllocationCallbacks allocator)
		{
			
		}
		public Result BindBufferMemory (IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			return Result.SUCCESS;
		}
		#endregion
		#region IGLIndirectBuffer implementation
		public GLMemoryBufferType BufferType {
			get {
				return GLMemoryBufferType.INDIRECT;
			}
		}
		public System.IntPtr Source {
			get;
			set;
		}
		#endregion
		
	}

}

