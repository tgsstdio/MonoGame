using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MonoGame.Core.Audio
{
    public interface ISoundEffectReader
    {
        SoundEffect Read(BinaryReader input);
    }
}