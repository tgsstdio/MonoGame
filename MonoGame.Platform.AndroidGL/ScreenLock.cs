using System;

namespace MonoGame.Platform.AndroidGL
{
	public class ScreenLock : IScreenLock
	{
		public ScreenLock ()
		{
			
		}

		#region IScreenLock implementation

		public bool ScreenLocked {
			get;
			set;
		}

		#endregion
	}
}

