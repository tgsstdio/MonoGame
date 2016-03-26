using System;

#if WINDOWS_PHONE
extern alias MicrosoftXnaFramework;
using MsAlbumCollection = MicrosoftXnaFramework::Microsoft.Xna.Framework.Media.AlbumCollection;
#endif

namespace Microsoft.Xna.Framework.Media
{
	#if WINDOWS_PHONE	
	public class WindowsPhoneAlbumCollection : IAlbumCollection
	{

		private MsAlbumCollection albumCollection;
		public static implicit operator WindowsPhoneAlbumCollection(MsAlbumCollection albumCollection)
		{
		return new AlbumCollection(albumCollection);
		}

		private WindowsPhoneAlbumCollection(MsAlbumCollection albumCollection)
		{
		this.albumCollection = albumCollection;
		}


		#region IAlbumCollection implementation

		public Album GetAlbum (int index)
		{
			// TODO : casting here this doesn't make any sense
			return (Album)this.albumCollection[index];
		}

		public int Count {
			get
			{
				return this.albumCollection.Count;
			}
		}

		public bool IsDisposed {
			get
			{
			return this.albumCollection.IsDisposed;
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			this.albumCollection.Dispose();
		}

		#endregion
	}
	#endif
}

