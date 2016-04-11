// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Media;


namespace MonoGame.Platform.AndroidGL
{
	public class NullMediaPlayer :IMediaPlayer
	{
		#region IMediaPlayer implementation

		public void Play (ISong song)
		{
			throw new System.NotImplementedException ();
		}

		public bool IsRepeating {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		public void Pause ()
		{
			throw new System.NotImplementedException ();
		}
		public void Resume ()
		{
			throw new System.NotImplementedException ();
		}
		public MediaState State {
			get {
				throw new System.NotImplementedException ();
			}
		}
		public bool IsMuted {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}
}
