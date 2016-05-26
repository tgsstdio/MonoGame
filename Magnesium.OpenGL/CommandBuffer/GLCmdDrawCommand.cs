using System;

namespace Magnesium.OpenGL
{
	public struct GLCmdDrawCommand
	{
		public enum DrawType : byte
		{
			Draw = 0,
			DrawIndexed,
			DrawIndirect,
			DrawIndexedIndirect,
		}

		public DrawType CommandType;

		public uint vertexCount;
		public uint instanceCount;
		public uint firstVertex;
		public uint firstInstance;
		public int vertexOffset;
		public uint indexCount;
		public uint firstIndex;

		public ulong offset;
		public uint drawCount;
		public uint stride;

		public int IndexBuffer;
		public int VertexBuffer;
		public int Pipeline;
		public int Scissors;
		public int Viewports;
		public int DescriptorSets;
	}
}

