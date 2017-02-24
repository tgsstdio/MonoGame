using OpenTK.Audio.OpenAL;
using System;

namespace MonoGame.Content.Audio.OpenAL.NVorbis
{
    internal static class ALHelper
    {
        public static readonly XRamExtension XRam = new XRamExtension();
        public static readonly EffectsExtension Efx = new EffectsExtension();

        public static void Check()
        {
            ALError error;
            if ((error = AL.GetError()) != ALError.NoError)
                throw new InvalidOperationException(AL.GetErrorString(error));
        }
    }
}
