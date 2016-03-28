namespace Microsoft.Xna.Framework.Content
{
	public class NullContentManager : IContentManager
	{
		public void Unload ()
		{
			throw new System.NotImplementedException ();
		}

		public void ReloadGraphicsContent ()
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

