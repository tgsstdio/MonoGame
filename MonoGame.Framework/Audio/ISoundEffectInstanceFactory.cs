namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectInstanceFactory
	{
		ISoundEffectInstance CreateNewInstance(ISoundEffectInstancePool parent);
		ISoundEffectInstance CreateNewInstance();
	}
}

