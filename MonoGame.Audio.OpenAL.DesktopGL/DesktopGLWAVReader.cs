using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Core.Audio;
using OpenTK.Audio.OpenAL;

// Code from AudioLoader.cs

namespace MonoGame.Audio.OpenAL.DesktopGL
{
    public class DesktopGLWAVReader : ISoundEffectReader
    {
        private ISoundEffectImplementation mPlatform;
        public DesktopGLWAVReader(ISoundEffectImplementation platform)
        {
            mPlatform = platform;
        }

        private AudioChannels GetAudioChannel(int noOfChannels)
        {
            switch (noOfChannels)
            {
                case 1:
                    return AudioChannels.Mono;
                case 2:
                    return AudioChannels.Stereo;
                default:
                    throw new NotSupportedException();

            }
        }

        public SoundEffect Read(BinaryReader input)
        {
            var noOfChannels = 0;
            var sampleRate = 0;
            ALFormat format;

            var audioData = LoadWave(input, out format, out noOfChannels, out sampleRate);
            return new SoundEffect(mPlatform, audioData, sampleRate, GetAudioChannel(noOfChannels));
        }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Mono8 : OpenTK.Audio.OpenAL.ALFormat.Mono16;
                case 2: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Stereo8 : OpenTK.Audio.OpenAL.ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        private static byte[] LoadWave(BinaryReader reader, out ALFormat format, out int noOfChanges, out int sampleRate)
        {
            // code based on opentk exemple

            byte[] audioData;

            //header
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            reader.ReadInt32(); // riff_chunck_size

            string wformat = new string(reader.ReadChars(4));
            if (wformat != "WAVE")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            // WAVE header
            string format_signature = new string(reader.ReadChars(4));
            while (format_signature != "fmt ")
            {
                reader.ReadBytes(reader.ReadInt32());
                format_signature = new string(reader.ReadChars(4));
            }

            int format_chunk_size = reader.ReadInt32();

            // total bytes read: tbp
            int audio_format = reader.ReadInt16(); // 2
            int num_channels = reader.ReadInt16(); // 4

            int sample_rate = reader.ReadInt32();  // 8
            reader.ReadInt32();    // 12, byte_rate
            reader.ReadInt16();  // 14, block_align
            int bits_per_sample = reader.ReadInt16(); // 16

            if (audio_format != 1)
            {
                throw new NotSupportedException("Wave compression is not supported.");
            }

            // reads residual bytes
            if (format_chunk_size > 16)
                reader.ReadBytes(format_chunk_size - 16);

            string data_signature = new string(reader.ReadChars(4));

            while (data_signature.ToLowerInvariant() != "data")
            {
                reader.ReadBytes(reader.ReadInt32());
                data_signature = new string(reader.ReadChars(4));
            }

            if (data_signature != "data")
            {
                throw new NotSupportedException("Specified wave file is not supported.");
            }

            int data_chunk_size = reader.ReadInt32();

            format = GetSoundFormat(num_channels, bits_per_sample);
            audioData = reader.ReadBytes((int)reader.BaseStream.Length);
            noOfChanges = num_channels;
            sampleRate = sample_rate;

            return audioData;
        }
    }
}
