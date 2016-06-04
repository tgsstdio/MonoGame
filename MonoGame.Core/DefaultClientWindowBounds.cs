using System;

namespace MonoGame.Core
{
	public class DefaultClientWindowBounds : IClientWindowBounds
	{
		#region IClientWindowBounds implementation

		public event EventHandler<EventArgs> ClientSizeChanged;

		public void OnClientSizeChanged ()
		{
			if (ClientSizeChanged != null)
				ClientSizeChanged (this, EventArgs.Empty);
		}

		public Rectangle ClientBounds {
			get;
			set;
		}

		public void ChangeClientBounds(Rectangle bounds)
		{
			if (bounds != ClientBounds)
			{
				ClientBounds = bounds;
				OnClientSizeChanged();
			}
		}

		#endregion
	}
}

