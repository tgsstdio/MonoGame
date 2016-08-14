using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdDrawEntrypoint : IGLCmdDrawEntrypoint
	{
		public void DrawArrays(MgPrimitiveTopology topology, uint first, uint count, uint instanceCount, uint firstInstance)
		{
			throw new NotImplementedException();
		}

		public void DrawArraysIndirect(MgPrimitiveTopology topology, IntPtr indirect, uint count, uint stride)
		{
			throw new NotImplementedException();
		}

		public void DrawIndexed(MgPrimitiveTopology topology, MgIndexType indexType, uint first, uint count, uint instanceCount, int vertexOffset)
		{
			throw new NotImplementedException();
		}

		public void DrawIndexedIndirect(MgPrimitiveTopology topology, MgIndexType indexType, IntPtr indirect, uint count, uint stride)
		{
			throw new NotImplementedException();
		}
	}
}