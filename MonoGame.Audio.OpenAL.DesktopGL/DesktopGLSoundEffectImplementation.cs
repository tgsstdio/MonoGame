using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
    public class DesktopGLSoundEffectImplementation : ISoundEffectImplementation
    {
        public void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public void Initialize(byte[] buffer, int sampleRate, AudioChannels channels)
        {
            throw new NotImplementedException();
        }

        public void Initialize(byte[] buffer, int offset, int count, int sampleRate, AudioChannels channels, int loopStart, int loopLength)
        {
            throw new NotImplementedException();
        }

        public void LoadAudioStream(Stream s)
        {
            throw new NotImplementedException();
        }

        public void SetupInstance(ISoundEffectInstance inst)
        {
            throw new NotImplementedException();
        }
    }
}
