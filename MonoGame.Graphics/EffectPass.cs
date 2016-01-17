namespace MonoGame.Graphics
{
	public class EffectPass
	{
		public int Pass { get; set;}
		public IEffectVariantCollection Variants {get; private set;}

		public EffectPass (int pass, IEffectVariantCollection shaders)
		{
			Pass = pass;
			Variants = shaders;
		}
	}
}

