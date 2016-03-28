using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Static class that is called before every draw to load resources that need to finish loading on the primary thread
    /// </summary>
	public class PrimaryThreadLoader : IPrimaryThreadLoader
    {
        private readonly object ListLockObject = new object();
        private readonly List<IPrimaryThreadLoaded> NeedToLoad = new List<IPrimaryThreadLoaded>(); 
        private readonly List<IPrimaryThreadLoaded> RemoveList = new List<IPrimaryThreadLoaded>();
        private DateTime _lastUpdate = DateTime.UtcNow;

        public void AddToList(IPrimaryThreadLoaded primaryThreadLoaded)
        {
            lock (ListLockObject)
            {
                NeedToLoad.Add(primaryThreadLoaded);
            }
        }

        public void RemoveFromList(IPrimaryThreadLoaded primaryThreadLoaded)
        {
            lock (ListLockObject)
            {
                NeedToLoad.Remove(primaryThreadLoaded);
            }
        }

        public void RemoveFromList(List<IPrimaryThreadLoaded> primaryThreadLoadeds)
        {
            lock (ListLockObject)
            {
                foreach (var primaryThreadLoaded in primaryThreadLoadeds)
                {
                    NeedToLoad.Remove(primaryThreadLoaded);
                }
            }
        }

        public void Clear()
        {
            lock(ListLockObject)
            {
                NeedToLoad.Clear();
            }
        }

        /// <summary>
        /// Loops through list and loads the item.  If successful, it is removed from the list.
        /// </summary>
        public void DoLoads()
        {
            if((DateTime.UtcNow - _lastUpdate).Milliseconds < 250) return;

            _lastUpdate = DateTime.UtcNow;
            lock (ListLockObject)
            {
                for (int i = 0; i < NeedToLoad.Count; i++)
                {
                    var primaryThreadLoaded = NeedToLoad[i];
                    if (primaryThreadLoaded.Load())
                    {
                        RemoveList.Add(primaryThreadLoaded);
                    }
                }

                for (int i = 0; i < RemoveList.Count; i++)
                {
                    var primaryThreadLoaded = RemoveList[i];
                    NeedToLoad.Remove(primaryThreadLoaded);
                }

                RemoveList.Clear();
            }
        }
    }
}