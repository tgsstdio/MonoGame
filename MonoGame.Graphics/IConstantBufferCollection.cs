using System;

namespace MonoGame.Graphics
{
	public interface IConstantBufferCollection
	{
		IConstantBuffer[] Filter (Mesh mesh, EffectPass pass, int options);
		void Add(IConstantBuffer b);
	}
}

