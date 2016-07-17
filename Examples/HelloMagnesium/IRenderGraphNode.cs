using Magnesium;
using System;

namespace HelloMagnesium
{
	public interface IRenderGraphNode
	{
		int DrawOrder { get; }
		bool Visible { get; }

		event EventHandler<EventArgs> DrawOrderChanged;
		event EventHandler<EventArgs> VisibleChanged;

		void Render (QueueArgument arg);
	}
}

