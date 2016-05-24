namespace MonoGame.Graphics
{
	public class PremutationSelector : IEffectPipelineCollection
	{
		private readonly EffectPipeline[] mVariants;		
		public PremutationSelector (EffectPipeline[] variants)
		{
			mVariants = variants;
		}

		#region IEffectVariantCollection implementation

		public bool TryGetValue (ushort options, out EffectPipeline result)
		{
			for (int i = 0; i < mVariants.Length; ++i)
			{
				var permutation = mVariants [i];

				if ((options & permutation.Options) == permutation.Options)
				{
					result = permutation;
					return true;
				}
			}
			result = null;
			return false;
		}

		#endregion
	}
}

