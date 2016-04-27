using System;
using MonoGame.Core;

namespace Microsoft.Xna.Framework.Audio
{
	public class DirectXADPCMSoundEffectInitializer : IADPCMSoundEffectInitializer
	{
		private readonly ISoundEffectImplementationFactory mFactory;		
		public DirectXADPCMSoundEffectInitializer (ISoundEffectImplementationFactory factory)
		{
			mFactory = factory;
		}

		#region IADPCMSoundEffectInitializer implementation

		public SoundEffect LoadEffect (byte[] audiodata, int rate, int chans, int align)
		{
			#if DIRECTX
			return = new SoundEffect(mFactory.Create(), audiodata, rate, (AudioChannels)chans)
			{
			_format = new SharpDX.Multimedia.WaveFormatAdpcm(rate, chans, align)
			};
			#else
			throw new NotSupportedException();
			#endif
		}

		#endregion
	}
}

