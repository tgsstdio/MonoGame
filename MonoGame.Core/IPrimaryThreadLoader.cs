using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
	public interface IPrimaryThreadLoader
	{
		void AddToList (IPrimaryThreadLoaded primaryThreadLoaded);
		void RemoveFromList (IPrimaryThreadLoaded primaryThreadLoaded);
		void RemoveFromList (List<IPrimaryThreadLoaded> primaryThreadLoadeds);
		void Clear();
		void DoLoads();
	}
}