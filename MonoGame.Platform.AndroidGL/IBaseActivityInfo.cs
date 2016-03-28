using System;

namespace MonoGame.Platform.AndroidGL
{
	public interface IBaseActivityInfo
	{
		bool AutoPauseAndResumeMediaPlayer { set; get;}
		bool RenderOnUIThread { get; set; }
	}
}

