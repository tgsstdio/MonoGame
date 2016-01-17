using System;

namespace MonoGame.Graphics.AZDO
{
	public interface IShaderProgram : IDisposable
	{
		void Use();
		void Unuse();
		void SetUniformIndex(byte index);
		byte GetUniformIndex();
		void Bind (IConstantBufferCollection buffers);
		ushort GetBufferMask();
	}
}

