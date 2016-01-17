using System;

namespace MonoGame.Graphics.AZDO
{
	public interface IVertexBufferObject : IDisposable
	{
		void Bind();
		void Unbind();
	}

}

