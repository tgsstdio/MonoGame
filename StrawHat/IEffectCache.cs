namespace MonoGame.Graphics
{
	public interface IEffectCache
	{		
		bool TryGetValue(byte effectIndex, out Effect result);
	}

}

