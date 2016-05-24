namespace MonoGame.Graphics
{
	public interface IEffectPipelineCollection
	{
		bool TryGetValue(ushort value, out EffectPipeline result);
	}
}

