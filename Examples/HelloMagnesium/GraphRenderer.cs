using System;
using Magnesium;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HelloMagnesium
{
	public class GraphRenderer
	{
		public GraphRenderer ()
		{
			Renderables =
				new SortingFilteringCollection<IRenderGraphNode>(
					d => d.Visible,
					(d, handler) => d.VisibleChanged += handler,
					(d, handler) => d.VisibleChanged -= handler,
					(d1 ,d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
					(d, handler) => d.DrawOrderChanged += handler,
					(d, handler) => d.DrawOrderChanged -= handler);
		}

		public SortingFilteringCollection<IRenderGraphNode> Renderables { get; private set; }

		private static readonly Action<IRenderGraphNode, QueueArgument> RenderAction =
			(drawable, arg) => drawable.Render(arg);

		public void Render(IMgQueue queue, GameTime gameTime, uint frameIndex)
		{
			var arg = new QueueArgument{ Queue = queue, GameTime = gameTime, FrameIndex = frameIndex };
			Renderables.ForEachFilteredItem<QueueArgument> (RenderAction, arg);
		}
	}
}

