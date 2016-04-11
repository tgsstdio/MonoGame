using System;
using Android.Graphics;

namespace MonoGame.Platform.AndroidGL
{
	public interface IAndroidAlbumArtContentResolver
	{
		Bitmap Resolve(Android.Net.Uri thumbnail);
	}
}

