using Microsoft.Xna.Framework.Content;

namespace HelloCube
{
	public class NullContentManager : IContentManager
	{
		public void Unload ()
		{
			throw new System.NotImplementedException ();
		}

		#region IDisposable implementation

		public void Dispose ()
		{

		}

		#endregion
	}
}

