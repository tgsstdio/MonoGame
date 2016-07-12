using System;
using Magnesium;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NewFences
{
	public class GraphRenderer
	{
		public GraphRenderer ()
		{
			Drawables =
				new SortingFilteringCollection<IQueueRenderable>(
					d => d.Visible,
					(d, handler) => d.VisibleChanged += handler,
					(d, handler) => d.VisibleChanged -= handler,
					(d1 ,d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
					(d, handler) => d.DrawOrderChanged += handler,
					(d, handler) => d.DrawOrderChanged -= handler);
		}

		public SortingFilteringCollection<IQueueRenderable> Drawables { get; private set; }

		private class QueueItem
		{
			public IMgQueue Queue {get; set; }
			public uint FrameIndex { get; set; }
		}

		private static readonly Action<IQueueRenderable, QueueItem> RenderAction =
			(drawable, arg) => drawable.Render(arg.Queue, arg.FrameIndex);

		public void Render(IMgQueue queue, uint frameIndex)
		{
			var arg = new QueueItem{ Queue = queue, FrameIndex = frameIndex };
			Drawables.ForEachFilteredItem<QueueItem> (RenderAction, arg);
		}
	}
}

