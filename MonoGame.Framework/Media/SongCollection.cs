// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

#if WINDOWS_PHONE
extern alias MicrosoftXnaFramework;
using MsSongCollection = MicrosoftXnaFramework::Microsoft.Xna.Framework.Media.SongCollection;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
	#if WINDOWS_PHONE	
	public class WindowsPhoneSongCollection : ISongCollection
	{
		private bool isReadOnly = false;
		private List<ISong> innerlist = new List<ISong>();

        private MsSongCollection songCollection;

		internal WindowsPhoneSongCollection(MsSongCollection songCollection)
        {
            this.songCollection = songCollection;
        }

		internal WindowsPhoneSongCollection()
        {

        }

		internal WindowsPhoneSongCollection(List<ISong> songs)
        {
            this.innerlist = songs;
        }

		public void Dispose()
        {
            if (this.songCollection != null)
                this.songCollection.Dispose();
        }
		
		public IEnumerator<ISong> GetEnumerator()
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            return innerlist.GetEnumerator();
        }
		
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this.songCollection != null)
                return this.songCollection.GetEnumerator();
            return innerlist.GetEnumerator();
        }

        public int Count
        {
            get
            {
                if (this.songCollection != null)
                    return this.songCollection.Count;
				return innerlist.Count;
            }
        }
		
		public bool IsReadOnly
        {
		    get
		    {
		        if (this.songCollection != null)
		            return true;
		        return this.isReadOnly;
		    }
        }

        public ISong this[int index]
        {
            get
            {
                if (this.songCollection != null)
                    return new Song(this.songCollection[index]);
				return this.innerlist[index];
            }
        }
		
		public void Add(ISong item)
        {
            if (this.songCollection != null)
                throw new NotSupportedException();

            if (item == null)
                throw new ArgumentNullException();

            if (innerlist.Count == 0)
            {
                this.innerlist.Add(item);
                return;
            }

            for (int i = 0; i < this.innerlist.Count; i++)
            {
                if (item.TrackNumber < this.innerlist[i].TrackNumber)
                {
                    this.innerlist.Insert(i, item);
                    return;
                }
            }

            this.innerlist.Add(item);
        }
		
		public void Clear()
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            innerlist.Clear();
        }
        
//        public SongCollection Clone()
//        {
//            if (this.songCollection != null)
//                throw new NotSupportedException();
//            SongCollection sc = new SongCollection();
//			foreach (ISong song in this.innerlist)
//                sc.Add(song);
//            return sc;
//        }
        
		public bool Contains(ISong item)
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            return innerlist.Contains(item);
        }
        
		public void CopyTo(ISong[] array, int arrayIndex)
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            innerlist.CopyTo(array, arrayIndex);
        }
		
		public int IndexOf(ISong item)
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            return innerlist.IndexOf(item);
        }
        
		public bool Remove(ISong item)
        {
            if (this.songCollection != null)
                throw new NotSupportedException();
            return innerlist.Remove(item);
        }
	}
	#endif
}

