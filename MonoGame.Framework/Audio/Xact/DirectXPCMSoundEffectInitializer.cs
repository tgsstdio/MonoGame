using System;
using MonoGame.Core;

namespace Microsoft.Xna.Framework.Audio
{
	public class DirectXPCMSoundEffectInitializer : IPCMSoundEffectInitializer
	{
		private readonly ISoundEffectImplementationFactory mFactory;		
		public DirectXPCMSoundEffectInitializer (ISoundEffectImplementationFactory factory)
		{
			mFactory = factory;	
		}

		#region IPCMSoundEffectInitializer implementation

		public SoundEffect LoadEffect (byte[] audiodata, int rate, int chans, int offset, int length)
		{
#if DIRECTX
			// FIXME: needs to be tested with updated SoundEffect implementation
			// TODO: Wouldn't storing a SoundEffectInstance like this
			// result in the "parent" SoundEffect being garbage collected?

			SharpDX.Multimedia.WaveFormat waveFormat = new SharpDX.Multimedia.WaveFormat(rate, chans);
			var sfx = new SoundEffect(mFactory.Create(), audiodata, 0, audiodata.Length, rate, (AudioChannels)chans, wavebankentry.LoopRegion.Offset, wavebankentry.LoopRegion.Length)
			{
			_format = waveFormat
			};

			return sfx;
#else 
			throw new NotSupportedException();
#endif
		}

		#endregion
	}
}

