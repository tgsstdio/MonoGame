using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	public interface INodeFlattener
	{
		void Parse (SceneNode s1);
		IList<RenderPass> Passes { get; }
	}
}

