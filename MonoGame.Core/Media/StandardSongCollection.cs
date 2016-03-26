// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
	public class StandardSongCollection : ISongCollection
	{
		private bool isReadOnly = false;
		private List<ISong> innerlist = new List<ISong>();

		public StandardSongCollection()
        {

        }

		public StandardSongCollection(List<ISong> songs)
        {
            this.innerlist = songs;
        }

		public void Dispose()
        {

		}
		
		public IEnumerator<ISong> GetEnumerator()
        {
            return innerlist.GetEnumerator();
        }
		
        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerlist.GetEnumerator();
        }

        public int Count
        {
            get
            {
				return innerlist.Count;
            }
        }
		
		public bool IsReadOnly
        {
		    get
		    {
		        return this.isReadOnly;
		    }
        }

        public ISong this[int index]
        {
            get
            {
				return this.innerlist[index];
            }
        }
		
		public void Add(ISong item)
        {
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
            innerlist.Clear();
        }
        
//        public StandardSongCollection Clone()
//        {
//            StandardSongCollection sc = new StandardSongCollection();
//			foreach (ISong song in this.innerlist)
//                sc.Add(song);
//            return sc;
//        }
        
		public bool Contains(ISong item)
        {
            return innerlist.Contains(item);
        }
        
		public void CopyTo(ISong[] array, int arrayIndex)
        {
            innerlist.CopyTo(array, arrayIndex);
        }
		
		public int IndexOf(ISong item)
        {
            return innerlist.IndexOf(item);
        }
        
		public bool Remove(ISong item)
        {
            return innerlist.Remove(item);
        }
	}
}

