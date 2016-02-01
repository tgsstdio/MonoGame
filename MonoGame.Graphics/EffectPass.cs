namespace MonoGame.Graphics
{
	public class EffectPass
	{
		public int Pass { get; set;}
		public IEffectVariantCollection Variants {get; private set;}
		public StateGroup PassGroup { get; set; }

		public static int GeneratePass(int index)
		{
			return 1 << index;
		}

		public EffectPass (int pass, IEffectVariantCollection shaders)
		{
			Pass = pass;
			Variants = shaders;
		}
	}
}

