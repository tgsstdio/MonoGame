
namespace HelloMagnesium
{
	public class PrecompiledGraphNode : IRenderGraphNode
	{
		public PrecompiledGraphNode (SubmitInfoGraphNode[] frameInstances)
		{
			FrameInstances = frameInstances;
		}

		public SubmitInfoGraphNode[] FrameInstances { get; private set;}

		#region IDrawable implementation

		public event System.EventHandler<System.EventArgs> DrawOrderChanged;

		public event System.EventHandler<System.EventArgs> VisibleChanged;

		public void Render(QueueArgument arg)
		{
			var submission = FrameInstances [arg.FrameIndex];

			arg.Queue.QueueSubmit(new []{submission.Submit}, submission.Fence);
		}

		private int mDrawOrder = 0;
		public int DrawOrder {
			get {
				return mDrawOrder;
			}
			set {
				if (mDrawOrder != value)
				{
					mDrawOrder = value;
					if (DrawOrderChanged != null)
					{
						DrawOrderChanged.Invoke (this, new System.EventArgs ());
					}
				}
			}
		}

		private bool mIsVisible = true;
		public bool Visible {
			get {
				return mIsVisible;
			}
			set {
				if (mIsVisible != value)
				{
					mIsVisible = value;
					if (VisibleChanged != null)
					{
						VisibleChanged.Invoke (this, new System.EventArgs ());
					}
				}
			}
		}

		#endregion


	}
}

