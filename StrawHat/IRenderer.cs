using System;

namespace StrawHat
{
	public interface IRenderer
	{
		void Submit(RenderPass pass, DrawItem[] items);
	}
}

