using MonoGame.Core;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLThreadSleeper : IThreadSleeper
	{
		#region IThreadSleeper implementation

		public void Sleep (int time)
		{
			System.Threading.Thread.Sleep(time);
		}

		#endregion
	}
}

