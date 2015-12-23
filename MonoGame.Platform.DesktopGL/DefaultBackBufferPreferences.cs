using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL
{
	public class DefaultBackBufferPreferences : IBackBufferPreferences
	{
		#region IBackBufferPreferences implementation

		public int DefaultBackBufferHeight
		{
			get {
				return 480;
			}
		}

		public int DefaultBackBufferWidth {
			get {
				return 800;
			}
		}

		#endregion

	}
}

