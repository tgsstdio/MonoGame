namespace MonoGame.Graphics
{
	public class EffectPass
	{
		public int Pass { get; set;}
		public IEffectPipelineCollection Variants {get; private set;}
		public StateGroup PassGroup { get; set; }

		public static int GeneratePass(int index)
		{
			return 1 << index;
		}

		public EffectPass (int pass, IEffectPipelineCollection shaders)
		{
			Pass = pass;
			Variants = shaders;
		}
	}
}

