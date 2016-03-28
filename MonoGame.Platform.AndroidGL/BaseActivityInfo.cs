using System;

namespace MonoGame.Platform.AndroidGL
{
	public class BaseActivityInfo : IBaseActivityInfo
	{
		#region IActivityInfo implementation
		public BaseActivityInfo ()
		{
			AutoPauseAndResumeMediaPlayer = true;
			RenderOnUIThread = true; 
		}

		public bool AutoPauseAndResumeMediaPlayer {
			get;
			set;
		}

		public bool RenderOnUIThread {
			get;
			set;
		}

		#endregion
	}
}

