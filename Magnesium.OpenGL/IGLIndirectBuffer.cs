using System;

namespace Magnesium.OpenGL
{
	public interface IGLIndirectBuffer : IMgBuffer
	{
		GLMemoryBufferType BufferType { get; }
		IntPtr Source { get; }
	}

}

