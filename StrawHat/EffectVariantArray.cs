namespace MonoGame.Graphics
{ 
	public class EffectVariantArray : IEffectVariantCollection
	{
		private readonly EffectShaderVariant[] mVariants;
		public EffectVariantArray (EffectShaderVariant[] variants)
		{
			mVariants = variants;
		}

		#region IEffectVariantCollection implementation

		public bool TryGetValue (ushort options, out EffectShaderVariant result)
		{
			if (options >= mVariants.Length)
			{
				result = null;
				return false;
			}
			else
			{
				result = mVariants [options];
				return true;
			}
		}

		#endregion
	}
}

