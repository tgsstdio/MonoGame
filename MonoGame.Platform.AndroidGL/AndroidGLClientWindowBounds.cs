using MonoGame.Core;
using Android.Content;
using System;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidGLClientWindowBounds : IClientWindowBounds
	{
		public AndroidGLClientWindowBounds (Context context)
		{
			ClientBounds = new Rectangle(0, 0, context.Resources.DisplayMetrics.WidthPixels, context.Resources.DisplayMetrics.HeightPixels);
		}

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

