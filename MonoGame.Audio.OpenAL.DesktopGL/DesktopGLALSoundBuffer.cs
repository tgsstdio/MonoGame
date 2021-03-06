// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using OpenTK.Audio.OpenAL;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Audio.OpenAL;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLALSoundBuffer : IOALSoundBuffer
	{
		int openALDataBuffer;
		ALFormat openALFormat;
		int dataSize;
		int sampleRate;
		private int _sourceId;
        internal int _pauseCount;

		public DesktopGLALSoundBuffer ()
		{
            try
            {
                var alError = AL.GetError();
                AL.GenBuffers(1, out openALDataBuffer);
                alError = AL.GetError();
                if (alError != ALError.NoError)
                {
                    Console.WriteLine("Failed to generate OpenAL data buffer: ", AL.GetErrorString(alError));
                }
            }
            catch (DllNotFoundException e)
            {
                throw new NoAudioHardwareException("OpenAL drivers could not be found.", e);
            }
		}

        ~DesktopGLALSoundBuffer()
        {
            Dispose(false);
        }

		public int OpenALDataBuffer {
			get {
				return openALDataBuffer;
			}
		}

		public double Duration {
			get;
			set;
		}

		public void BindDataBuffer(byte[] dataBuffer, OALAudioFormat format, int size, int sampleRate)
        {
			ALFormat internalFormat;
			switch (format)
			{
			case OALAudioFormat.Mono16Bit:
				internalFormat = ALFormat.Mono16;
				break;
			case OALAudioFormat.Stereo16Bit:
				internalFormat = ALFormat.Stereo16;
				break;
			default:
				throw new NotSupportedException ();
			}

			openALFormat = internalFormat;
            dataSize = size;
            this.sampleRate = sampleRate;
            AL.BufferData(openALDataBuffer, openALFormat, dataBuffer, dataSize, this.sampleRate);

            int bits, channels;

            AL.GetBuffer(openALDataBuffer, ALGetBufferi.Bits, out bits);
            ALError alError = AL.GetError();
            if (alError != ALError.NoError)
            {
				Console.WriteLine("Failed to get buffer bits: {0}, format={1}, size={2}, sampleRate={3}", AL.GetErrorString(alError), openALFormat, size, sampleRate);
                Duration = -1;
            }
            else
            {
                AL.GetBuffer(openALDataBuffer, ALGetBufferi.Channels, out channels);

                alError = AL.GetError();
                if (alError != ALError.NoError)
                {
					Console.WriteLine("Failed to get buffer bits: {0}, format={1}, size={2}, sampleRate={3}", AL.GetErrorString(alError), openALFormat, size, sampleRate);
                    Duration = -1;
                }
                else
                {
                    Duration = (float)(size / ((bits / 8) * channels)) / (float)sampleRate;
                }
            }
            //Console.WriteLine("Duration: " + Duration + " / size: " + size + " bits: " + bits + " channels: " + channels + " rate: " + sampleRate);

        }

        public void Pause()
        {
            if (_pauseCount == 0)
                AL.SourcePause(_sourceId);
            ++_pauseCount;
        }

        public void Resume()
        {
            --_pauseCount;
            if (_pauseCount == 0)
                AL.SourcePlay(_sourceId);
        }

		public void Dispose()
		{
            Dispose(true);
            GC.SuppressFinalize(this);
		}

		private bool _isDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Clean up managed objects
                }
                // Release unmanaged resources
                if (AL.IsBuffer(openALDataBuffer))
                {
                    AL.DeleteBuffers(1, ref openALDataBuffer);
                }

                _isDisposed = true;
            }
        }

		public int SourceId
		{
			get {
				return _sourceId;
			}

			set {
				_sourceId = value;
				if (Reserved != null)
					Reserved(this, EventArgs.Empty);

			}
		}

		public void RecycleSoundBuffer()
		{
			if (Recycled != null)
				Recycled(this, EventArgs.Empty);
		}

		#region Events
		public event EventHandler<EventArgs> Reserved;
		public event EventHandler<EventArgs> Recycled;
		#endregion
	}
}

