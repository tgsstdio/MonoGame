using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IEffectCache
	{
		bool TryGetValue(int key, out Effect item);
		void Add (int key, Effect item);
		void Clear();
	}
}

