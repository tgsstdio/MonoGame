namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLIndirectBuffer : IGLBuffer
	{
		#region IMgBuffer implementation
		public void DestroyBuffer (IMgDevice device, IMgAllocationCallbacks allocator)
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

		public ulong RequestedSize {
			get;
			set;
		}

		public int BufferId {
			get;
			set;
		}
		#endregion
		
	}

}

