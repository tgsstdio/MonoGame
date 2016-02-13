using System;
using Microsoft.Xna.Framework.Graphics;

namespace HelloCube
{
	public class NullTextureCollectionPlatform : ITextureCollectionPlatform
	{
		#region ITextureCollectionPlatform implementation

		public void SetTextures (Microsoft.Xna.Framework.IGraphicsDevice device)
		{
			throw new NotImplementedException ();
		}

		public void Clear ()
		{
			throw new NotImplementedException ();
		}

		public void Init ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

