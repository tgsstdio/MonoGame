using MonoGame.Core;

namespace Microsoft.Xna.Framework.Audio
{
	public class DefaultPCMSoundEffectInitializer : IPCMSoundEffectInitializer
	{
		private readonly ISoundEffectImplementationFactory mFactory;
		public DefaultPCMSoundEffectInitializer (ISoundEffectImplementationFactory factory)
		{
			mFactory = factory;
		}

		#region IPCMSoundEffectInitializer implementation

		public SoundEffect LoadEffect (byte[] audiodata, int rate, int chans, int offset, int length)
		{
			return new SoundEffect(mFactory.Create(), audiodata, rate, (AudioChannels)chans);
		}

		#endregion
	}
}

