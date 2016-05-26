using System;

namespace Magnesium.OpenGL
{
	public interface IShaderProgram : IDisposable
	{
		void Use();
		void Unuse();
		void SetUniformIndex(byte index);
		byte GetUniformIndex();
		void BindMask (IConstantBufferCollection buffers);
		ushort GetBufferMask();
		uint GetBindingSet ();
		void BindSet (uint setIndex);
	}
}

