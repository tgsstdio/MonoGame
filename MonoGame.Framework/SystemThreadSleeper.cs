using System;
using MonoGame.Core;

namespace Microsoft.Xna.Framework
{
	public class SystemThreadSleeper : IThreadSleeper
	{
		#region IThreadSleeper implementation

		public void Sleep (int time)
		{
			System.Threading.Thread.Sleep(time);
		}

		#endregion
	}
}

