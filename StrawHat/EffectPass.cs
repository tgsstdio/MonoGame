namespace MonoGame.Graphics
{
	public class EffectPass
	{
		public byte Pass { get; set;}
		public IEffectVariantCollection Variants {get; private set;}

		public EffectPass (byte pass, IEffectVariantCollection shaders)
		{
			Pass = pass;
			Variants = shaders;
		}
	}
}

