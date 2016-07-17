using System;
using Magnesium;
using Microsoft.Xna.Framework;

namespace HelloMagnesium
{
	public abstract class ImmediateModeGraphNode : IRenderGraphNode
	{
		private IMgFramebuffer[] mDestinations;
		protected ImmediateModeGraphNode (IMgFramebuffer[] destinations)
		{
			mDestinations = destinations;
		}

		#region IRenderGraphNode implementation

		public event EventHandler<EventArgs> DrawOrderChanged;

		public event EventHandler<EventArgs> VisibleChanged;

		public abstract SubmitInfoGraphNode Draw (IMgFramebuffer dest, GameTime gameTime);

		public void Render (QueueArgument arg)
		{
			var fb = mDestinations [arg.FrameIndex];

			SubmitInfoGraphNode node = Draw (fb, arg.GameTime);

			// compile here
			arg.Queue.QueueSubmit(new []{node.Submit}, node.Fence);
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

