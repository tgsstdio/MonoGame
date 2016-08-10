using Magnesium;
using System;

namespace MonoGame.Graphics
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

