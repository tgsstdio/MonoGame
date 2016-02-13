using MonoGame.Graphics;

namespace NewFences
{
	public interface IRenderPassGroup
	{
		void RenderAll (int index);
		IRenderPass Pass {get;set;}
		IMeshBuffer Buffer { get; set;}
	}
}
