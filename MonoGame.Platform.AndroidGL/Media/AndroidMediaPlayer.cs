using System;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL.Media
{
	public class AndroidMediaPlayer : IMediaPlayer, IAndroidMediaPlayer
	{
		public AndroidMediaPlayer ()
		{
		}

		#region IMediaPlayer implementation

		public void Play (ISong song)
		{
			var temp = song as AndroidSong;
			PlaySong (temp);
		}

		public void PlaySong (AndroidSong song)
		{
			throw new NotImplementedException ();
		}

		public void Pause ()
		{
			throw new NotImplementedException ();
		}

		public void Resume ()
		{
			throw new NotImplementedException ();
		}

		public MediaState State {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsMuted {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}


		public bool IsRepeating {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		#endregion
	}
}

