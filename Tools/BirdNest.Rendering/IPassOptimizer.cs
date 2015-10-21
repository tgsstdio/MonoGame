using System.Collections.Generic;

namespace BirdNest.Rendering
{
	public interface IPassOptimizer
	{
		void Optimize(IList<RenderPass> passes);
	}
}

