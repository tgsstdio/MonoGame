namespace Microsoft.Xna.Framework.Content
{
	public class NullContentManager : IContentManager
	{
		public string RootDirectory {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

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

