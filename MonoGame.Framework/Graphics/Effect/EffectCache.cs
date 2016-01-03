using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
	public class EffectCache : IEffectCache
	{
		/// <summary>
		/// The cache of effects from unique byte streams.
		/// </summary>
		internal Dictionary<int, Effect> mEffectCache;

		public EffectCache ()
		{
			mEffectCache = new Dictionary<int, Effect> ();
		}

		#region IEffectCache implementation

		public bool TryGetValue (int key, out Effect item)
		{
			throw new NotImplementedException ();
		}

		public void Add (int key, Effect item)
		{
			mEffectCache.Add (key, item);
		}

		public void Clear ()
		{
			mEffectCache.Clear ();
		}

		#endregion
	}
}

