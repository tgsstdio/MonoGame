// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
﻿
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
    /// <summary>Represents a loaded sound resource.</summary>
    /// <remarks>
    /// <para>A SoundEffect represents the buffer used to hold audio data and metadata. SoundEffectInstances are used to play from SoundEffects. Multiple SoundEffectinstance objects can be created and played from the same SoundEffect object.</para>
    /// <para>The only limit on the number of loaded SoundEffects is restricted by available memory. When a SoundEffect is disposed, all SoundEffectInstances created from it will become invalid.</para>
    /// <para>SoundEffect.Play() can be used for 'fire and forget' sounds. If advanced playback controls like volume or pitch is required, use SoundEffect.CreateInstance().</para>
    /// </remarks>
    public sealed partial class SoundEffect : IDisposable
    {
        #region Internal Audio Data

//		public static void PlatformShutdown ()
//		{
//			// TODO : No idea what to do
//		}

        private string _name;
        
        private bool _isDisposed = false;
        private TimeSpan _duration = TimeSpan.Zero;

        #endregion

        #region Internal Constructors

        internal SoundEffect() { }

        #endregion

        #region Public Constructors

		public ISoundEffectImplementation Platform { get; private set; }
		/// <param name = "platform">Platform for device</param>
        /// <param name="buffer">Buffer containing PCM wave data.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz)</param>
        /// <param name="channels">Number of channels (mono or stereo).</param>
		public SoundEffect(ISoundEffectImplementation platform, byte[] buffer, int sampleRate, AudioChannels channels)
        {
			Platform = platform;
            _duration = GetSampleDuration(buffer.Length, sampleRate, channels);

			Platform.Initialize(buffer, sampleRate, channels);
        }

		/// <param name = "platform"></param>
        /// <param name="buffer">Buffer containing PCM wave data.</param>
        /// <param name="offset">Offset, in bytes, to the starting position of the audio data.</param>
        /// <param name="count">Amount, in bytes, of audio data.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz)</param>
        /// <param name="channels">Number of channels (mono or stereo).</param>
        /// <param name="loopStart">The position, in samples, where the audio should begin looping.</param>
        /// <param name="loopLength">The duration, in samples, that audio should loop over.</param>
        /// <remarks>Use SoundEffect.GetSampleDuration() to convert time to samples.</remarks>
		public SoundEffect(ISoundEffectImplementation platform, byte[] buffer, int offset, int count, int sampleRate, AudioChannels channels, int loopStart, int loopLength)
        {
			Platform = platform;
            _duration = GetSampleDuration(count, sampleRate, channels);

			Platform.Initialize(buffer, offset, count, sampleRate, channels, loopStart, loopLength);
        }

        #endregion

        #region Finalizer

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/> is reclaimed by garbage collection.
        /// </summary>
        ~SoundEffect()
        {
            Dispose(false);
        }

        #endregion

        #region Additional SoundEffect/SoundEffectInstance Creation Methods

        /// <summary>
        /// Creates a new SoundEffectInstance for this SoundEffect.
        /// </summary>
        /// <returns>A new SoundEffectInstance for this SoundEffect.</returns>
        /// <remarks>Creating a SoundEffectInstance before calling SoundEffectInstance.Play() allows you to access advanced playback features, such as volume, pitch, and 3D positioning.</remarks>
//        public SoundEffectInstance CreateInstance()
//        {
//            var inst = new SoundEffectInstance();
//            Platform.SetupInstance(inst);
//
//            inst._isPooled = false;
//            inst._effect = this;
//
//            return inst;
//        }

        /// <summary>
        /// Creates a SoundEffect object based on the specified data stream.
        /// </summary>
		/// <param name = "platform"></param>
        /// <param name="s">Stream object containing PCM wave data.</param>
        /// <returns>A new SoundEffect object.</returns>
        /// <remarks>The Stream object must point to the head of a valid PCM wave file. Also, this wave file must be in the RIFF bitstream format.</remarks>
		public SoundEffect(ISoundEffectImplementation platform, Stream s)
        {
            if (s == null)
                throw new ArgumentNullException();

            // Notes from the docs:

            /*The Stream object must point to the head of a valid PCM wave file. Also, this wave file must be in the RIFF bitstream format.
              The audio format has the following restrictions:
              Must be a PCM wave file
              Can only be mono or stereo
              Must be 8 or 16 bit
              Sample rate must be between 8,000 Hz and 48,000 Hz*/
			Platform = platform;
			Platform.LoadAudioStream(s);
        }

        /// <summary>
        /// Gets the TimeSpan representation of a single sample.
        /// </summary>
        /// <param name="sizeInBytes">Size, in bytes, of audio data.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz). Must be between 8000 Hz and 48000 Hz</param>
        /// <param name="channels">Number of channels in the audio data.</param>
        /// <returns>TimeSpan object that represents the calculated sample duration.</returns>
        public static TimeSpan GetSampleDuration(int sizeInBytes, int sampleRate, AudioChannels channels)
        {
            if (sampleRate < 8000 || sampleRate > 48000)
                throw new ArgumentOutOfRangeException();

            // Reference: http://social.msdn.microsoft.com/Forums/windows/en-US/5a92be69-3b4e-4d92-b1d2-141ef0a50c91/how-to-calculate-duration-of-wave-file-from-its-size?forum=winforms
            var numChannels = (int)channels;

            var dur = sizeInBytes / (sampleRate * numChannels * 16f / 8f);

            var duration = TimeSpan.FromSeconds(dur);

            return duration;
        }

        /// <summary>
        /// Gets the size of a sample from a TimeSpan.
        /// </summary>
        /// <param name="duration">TimeSpan object that contains the sample duration.</param>
        /// <param name="sampleRate">Sample rate, in Hertz (Hz), of audio data. Must be between 8,000 and 48,000 Hz.</param>
        /// <param name="channels">Number of channels in the audio data.</param>
        /// <returns>Size of a single sample of audio data.</returns>
        public static int GetSampleSizeInBytes(TimeSpan duration, int sampleRate, AudioChannels channels)
        {
            if (sampleRate < 8000 || sampleRate > 48000)
                throw new ArgumentOutOfRangeException();

            // Reference: http://social.msdn.microsoft.com/Forums/windows/en-US/5a92be69-3b4e-4d92-b1d2-141ef0a50c91/how-to-calculate-duration-of-wave-file-from-its-size?forum=winforms

            var numChannels = (int)channels;

            var sizeInBytes = duration.TotalSeconds * (sampleRate * numChannels * 16f / 8f);

            return (int)sizeInBytes;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the duration of the SoundEffect.</summary>
        public TimeSpan Duration { get { return _duration; } }

        /// <summary>Gets or sets the asset name of the SoundEffect.</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>Indicates whether the object is disposed.</summary>
        public bool IsDisposed { get { return _isDisposed; } }

        /// <summary>Releases the resources held by this <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/>.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources held by this <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/>.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c>, Dispose was called explicitly.</param>
        /// <remarks>If the disposing parameter is true, the Dispose method was called explicitly. This
        /// means that managed objects referenced by this instance should be disposed or released as
        /// required.  If the disposing parameter is false, Dispose was called by the finalizer and
        /// no managed objects should be touched because we do not know if they are still valid or
        /// not at that time.  Unmanaged resources should always be released.</remarks>
        void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                Platform.Dispose(disposing);
                _isDisposed = true;
            }
        }

        #endregion

    }
}
