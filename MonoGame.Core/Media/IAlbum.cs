// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface IAlbum : IDisposable
	{
		IArtist Artist { get; }
		TimeSpan Duration { get; }
		IGenre Genre { get; }
		bool HasArt { get; }
		bool IsDisposed { get; }
		string Name { get; }
		ISongCollection Songs { get; }
	}

}
