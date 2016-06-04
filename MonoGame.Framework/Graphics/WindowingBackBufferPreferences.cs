using System;
using MonoGame.Core;

namespace Microsoft.Xna.Framework
{
	public class WindowingBackBufferPreferences : IBackBufferPreferences
	{
		public WindowingBackBufferPreferences (IClientWindowBounds client)
		{
			DefaultBackBufferWidth = Math.Max(client.ClientBounds.Height, client.ClientBounds.Width);
			DefaultBackBufferHeight = Math.Min(client.ClientBounds.Height, client.ClientBounds.Width);
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

