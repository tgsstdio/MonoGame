namespace MonoGame.Graphics
{
	public interface IEffectVariantCollection
	{
		bool TryGetValue(ushort value, out EffectShaderVariant result);
	}
}

