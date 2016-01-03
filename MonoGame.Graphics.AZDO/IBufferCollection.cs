using System;

namespace MonoGame.Graphics.AZDO
{
	public interface IConstantBufferCollection
	{
		ConstantBuffer[] Filter (Mesh mesh, EffectPass pass, int options);
		void Add(ConstantBuffer b);
	}
}

