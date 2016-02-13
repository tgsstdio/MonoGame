// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MonoGame.Audio.OpenAL
{
	public interface IOALSoundBuffer : IDisposable
	{
		int OpenALDataBuffer { get; }
		double Duration { get; set; }
		void Pause ();
		void Resume ();
		int SourceId { get; set; }
		void RecycleSoundBuffer();

		#region Events
		event EventHandler<EventArgs> Reserved;
		event EventHandler<EventArgs> Recycled;
		#endregion
	}
}

