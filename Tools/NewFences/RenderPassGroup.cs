using System;
using MonoGame.Graphics;

namespace NewFences
{
	public class RenderPassGroup : IRenderPassGroup
	{
		public void RenderAll (int index)
		{
			throw new NotImplementedException ();
		}

		public IMeshBuffer Buffer { get; set;}

		public IRenderPass Pass {
			get;
			set;
		}
	}
}
