using System;
using Microsoft.Xna.Framework.Graphics;

namespace HelloCube
{
	public class NullSamplerStateCollectionPlatform : ISamplerStateCollectionPlatform
	{
		#region ISamplerStateCollectionPlatform implementation

		public void Dirty ()
		{
			throw new NotImplementedException ();
		}

		public void Clear ()
		{
			throw new NotImplementedException ();
		}

		public void SetSamplerState (int index)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

