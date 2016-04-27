using System.IO;
using MonoGame.Core;

namespace Microsoft.Xna.Framework.Audio
{
	public class DefaultADPCMSoundEffectInitializer : IADPCMSoundEffectInitializer
	{
		private readonly ISoundEffectImplementationFactory mFactory;
		public DefaultADPCMSoundEffectInitializer (ISoundEffectImplementationFactory factory)
		{
			mFactory = factory;
		}

		#region IADPCMSoundEffectInitializer implementation

		public SoundEffect LoadEffect (byte[] audiodata, int rate, int chans, int align)
		{
            using (var dataStream = new MemoryStream(audiodata)) {
                using (var source = new BinaryReader(dataStream)) {
                    return new SoundEffect(
						mFactory.Create(),
                        MSADPCMToPCM.MSADPCM_TO_PCM(source, (short) chans, (short) align),
                        rate,
                        (AudioChannels)chans
                    );
                }
            }
		}

		#endregion
	}
}

