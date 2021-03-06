// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Media
{
	public delegate void FinishedPlayingHandler(object sender, EventArgs args);

	public interface ISong : IDisposable
	{
		void Initialize();

		int TrackNumber {
			get;
			set;
		}

		void Stop ();

		float Volume {
			get;
			set;
		}

		string Name { get; }

		int PlayCount {
			get;
		}

		void SongCompleted(object sender, EventArgs args);
	}
}

