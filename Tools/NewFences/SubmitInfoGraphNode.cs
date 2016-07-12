using Magnesium;

namespace NewFences
{
	public class SubmitInfoGraphNode
	{
		public bool IsVisible { get; set; }
		public MgSubmitInfo Submit { get; set; }
		public IMgFence Fence { get; set; }
	}
}

