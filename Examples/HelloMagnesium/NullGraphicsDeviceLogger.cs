using MonoGame.Platform.DesktopGL;
using System;

namespace HelloMagnesium
{
	public class MockGraphicsDeviceLogger : IGraphicsDeviceLogger
	{
		#region IGraphicsDeviceLogger implementation
		public void Log (string message)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}

}
