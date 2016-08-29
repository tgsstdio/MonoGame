using System;

namespace Magnesium.OpenGL.UnitTests
{
	public class MockMgPipelineLayout : IMgPipelineLayout
	{
		#region IMgPipelineLayout implementation
		public void DestroyPipelineLayout (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}

}

