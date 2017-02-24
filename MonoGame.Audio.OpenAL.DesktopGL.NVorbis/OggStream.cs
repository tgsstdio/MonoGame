// This code originated from:
//
//    http://theinstructionlimit.com/ogg-streaming-using-opentk-and-nvorbis
//    https://github.com/renaudbedard/nvorbis/
//
// It was released to the public domain by the author (Renaud Bedard).
// No other license is intended or required. 

using System;
using System.IO;
using NVorbis;
using OpenTK.Audio.OpenAL;


namespace MonoGame.Content.Audio.OpenAL.NVorbis
{
    internal class OggStream : IDisposable
    {
        const int DefaultBufferCount = 3;

        internal readonly object stopMutex = new object();
        internal readonly object prepareMutex = new object();

        internal readonly int alSourceId;
        internal readonly int[] alBufferIds;

        readonly int alFilterId;
        readonly Stream underlyingStream;

        internal VorbisReader Reader { get; private set; }
        internal bool Ready { get; private set; }
        internal bool Preparing { get; private set; }

        public Action FinishedAction { get; private set; }
        public int BufferCount { get; private set; }

        public OggStream(string filename, Action finishedAction = null, int bufferCount = DefaultBufferCount) : this(File.OpenRead(filename), finishedAction, bufferCount) { }
        public OggStream(Stream stream, Action finishedAction = null, int bufferCount = DefaultBufferCount)
        {
            FinishedAction = finishedAction;
            BufferCount = bufferCount;

            alBufferIds = AL.GenBuffers(bufferCount);
            alSourceId = AL.GenSource();

            if (ALHelper.XRam.IsInitialized)
            {
                ALHelper.XRam.SetBufferMode(BufferCount, ref alBufferIds[0], XRamExtension.XRamStorage.Hardware);
                ALHelper.Check();
            }

            Volume = 1;

            if (ALHelper.Efx.IsInitialized)
            {
                alFilterId = ALHelper.Efx.GenFilter();
                ALHelper.Efx.Filter(alFilterId, EfxFilteri.FilterType, (int)EfxFilterType.Lowpass);
                ALHelper.Efx.Filter(alFilterId, EfxFilterf.LowpassGain, 1);
                LowPassHFGain = 1;
            }

            underlyingStream = stream;
        }

        public void Prepare()
        {
            if (Preparing) return;

            var state = AL.GetSourceState(alSourceId);

            lock (stopMutex)
            {
                switch (state)
                {
                    case ALSourceState.Playing:
                    case ALSourceState.Paused:
                        return;

                    case ALSourceState.Stopped:
                        lock (prepareMutex)
                        {
                            Close();
                            Empty();
                        }
                        break;
                }

                if (!Ready)
                {
                    lock (prepareMutex)
                    {
                        Preparing = true;
                        Open(precache: true);
                    }
                }
            }
        }

        public void Play()
        {
            var state = AL.GetSourceState(alSourceId);

            switch (state)
            {
                case ALSourceState.Playing: return;
                case ALSourceState.Paused:
                    Resume();
                    return;
            }

            Prepare();

            AL.SourcePlay(alSourceId);
            ALHelper.Check();

            Preparing = false;

            OggStreamer.Instance.AddStream(this);
        }

        public void Pause()
        {
            if (AL.GetSourceState(alSourceId) != ALSourceState.Playing)
                return;

            OggStreamer.Instance.RemoveStream(this);
            AL.SourcePause(alSourceId);
            ALHelper.Check();
        }

        public void Resume()
        {
            if (AL.GetSourceState(alSourceId) != ALSourceState.Paused)
                return;

            OggStreamer.Instance.AddStream(this);
            AL.SourcePlay(alSourceId);
            ALHelper.Check();
        }

        public void Stop()
        {
            var state = AL.GetSourceState(alSourceId);
            if (state == ALSourceState.Playing || state == ALSourceState.Paused)
                StopPlayback();

            lock (stopMutex)
            {
                OggStreamer.Instance.RemoveStream(this);
            }
        }

        public ALSourceState GetState()
        {
            return AL.GetSourceState(alSourceId);
        }

        public TimeSpan GetPosition()
        {
            if (Reader == null)
                return TimeSpan.Zero;

            return Reader.DecodedTime;
        }

        public TimeSpan GetLength()
        {
            return Reader.TotalTime;
        }

        float lowPassHfGain;
        public float LowPassHFGain
        {
            get { return lowPassHfGain; }
            set
            {
                if (ALHelper.Efx.IsInitialized)
                {
                    ALHelper.Efx.Filter(alFilterId, EfxFilterf.LowpassGainHF, lowPassHfGain = value);
                    ALHelper.Efx.BindFilterToSource(alSourceId, alFilterId);
                    ALHelper.Check();
                }
            }
        }

        float volume;
        public float Volume
        {
            get { return volume; }
            set
            {
                AL.Source(alSourceId, ALSourcef.Gain, volume = value);
                ALHelper.Check();
            }
        }

        public bool IsLooped { get; set; }

        public void Dispose()
        {
            var state = AL.GetSourceState(alSourceId);
            if (state == ALSourceState.Playing || state == ALSourceState.Paused)
                StopPlayback();

            lock (prepareMutex)
            {
                OggStreamer.Instance.RemoveStream(this);

                if (state != ALSourceState.Initial)
                    Empty();

                Close();

                underlyingStream.Dispose();
            }

            AL.DeleteSource(alSourceId);
            AL.DeleteBuffers(alBufferIds);

            if (ALHelper.Efx.IsInitialized)
                ALHelper.Efx.DeleteFilter(alFilterId);

            ALHelper.Check();
        }

        void StopPlayback()
        {
            AL.SourceStop(alSourceId);
            ALHelper.Check();
        }

        void Empty()
        {
            int queued;
            AL.GetSource(alSourceId, ALGetSourcei.BuffersQueued, out queued);
            if (queued > 0)
            {
                try
                {
                    AL.SourceUnqueueBuffers(alSourceId, queued);
                    ALHelper.Check();
                }
                catch (InvalidOperationException)
                {
                    // This is a bug in the OpenAL implementation
                    // Salvage what we can
                    int processed;
                    AL.GetSource(alSourceId, ALGetSourcei.BuffersProcessed, out processed);
                    var salvaged = new int[processed];
                    if (processed > 0)
                    {
                        AL.SourceUnqueueBuffers(alSourceId, processed, salvaged);
                        ALHelper.Check();
                    }

                    // Try turning it off again?
                    AL.SourceStop(alSourceId);
                    ALHelper.Check();

                    Empty();
                }
            }
        }

        internal void Open(bool precache = false)
        {
            underlyingStream.Seek(0, SeekOrigin.Begin);
            Reader = new VorbisReader(underlyingStream, false);

            if (precache)
            {
                // Fill first buffer synchronously
                OggStreamer.Instance.FillBuffer(this, alBufferIds[0]);
                AL.SourceQueueBuffer(alSourceId, alBufferIds[0]);
                ALHelper.Check();

                // Schedule the others asynchronously
                OggStreamer.Instance.AddStream(this);
            }

            Ready = true;
        }

        internal void Close()
        {
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
            Ready = false;
        }
    }
}
