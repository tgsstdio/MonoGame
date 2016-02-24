using System;

namespace Microsoft.Xna.Framework
{
	public class WindowingBackBufferPreferences : IBackBufferPreferences
	{
		public WindowingBackBufferPreferences (GameWindow window)
		{
			DefaultBackBufferWidth = Math.Max(window.ClientBounds.Height, window.ClientBounds.Width);
			DefaultBackBufferHeight = Math.Min(window.ClientBounds.Height, window.ClientBounds.Width);
		}

		#region IBackBufferPreferences implementation

		public int DefaultBackBufferHeight {
			get;
			private set;
		}

		public int DefaultBackBufferWidth {
			get;
			private set;
		}

		#endregion
	}
}

