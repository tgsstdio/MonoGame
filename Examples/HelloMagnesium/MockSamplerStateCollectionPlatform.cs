using System;
using Microsoft.Xna.Framework.Graphics;

namespace HelloMagnesium
{
	public class MockSamplerStateCollectionPlatform : ISamplerStateCollectionPlatform
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

