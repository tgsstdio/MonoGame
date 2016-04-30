// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Media;


namespace MonoGame.Platform.AndroidGL
{
	public class NullMediaLibrary : IMediaLibrary
	{
		#region IMediaLibrary implementation
		public void Load (System.Action<int> progressCallback)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
		#region IDisposable implementation
		public void Dispose ()
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}
}
