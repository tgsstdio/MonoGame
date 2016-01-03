using System.Collections.Generic;

namespace MonoGame.Graphics
{
	public class EffectVariantDictionary : IEffectVariantCollection
	{
		private readonly Dictionary<ushort, EffectShaderVariant> mEntries;
		public EffectVariantDictionary ()
		{
			mEntries = new Dictionary<ushort, EffectShaderVariant> ();
		}

		public void Add(ushort key, EffectShaderVariant item)
		{
			mEntries.Add (key, item);
		}

		#region IEffectVariantCollection implementation

		public bool TryGetValue (ushort options, out EffectShaderVariant result)
		{
			return mEntries.TryGetValue (options, out result);
		}

		#endregion
	}
}

