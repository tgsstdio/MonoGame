// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface IGenre : IDisposable
	{
		IAlbumCollection Albums { get; }
		bool IsDisposed { get; }
		string Name { get; }
		ISongCollection Songs { get; }
	}

}
