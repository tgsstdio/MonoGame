using Magnesium;
using System;

namespace NewFences
{
	public interface IQueueRenderable
	{
		int DrawOrder { get; }
		bool Visible { get; }

		event EventHandler<EventArgs> DrawOrderChanged;
		event EventHandler<EventArgs> VisibleChanged;

		void Render (IMgQueue queue, uint frameIndex);
	}
}

