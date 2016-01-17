using System;

namespace MonoGame.Graphics
{
	public interface IRenderer
	{
		void Submit(RenderPass pass, DrawItem[] items);
	}
}

