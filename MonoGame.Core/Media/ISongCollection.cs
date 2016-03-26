// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System.Collections.Generic;
using System.Collections;
using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface ISongCollection : ICollection<ISong>, IEnumerable<ISong>, IEnumerable, IDisposable
	{
		int Count { get; }
		bool IsReadOnly { get; }
		void Add (ISong item);
		void Clear();
		bool Contains (ISong item);
		void CopyTo (ISong[] array, int arrayIndex);
		int IndexOf (ISong item);
		bool Remove(ISong item);
	}

}

