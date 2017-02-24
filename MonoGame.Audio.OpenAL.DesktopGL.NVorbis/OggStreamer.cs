using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MonoGame.Content.Audio.OpenAL.NVorbis
{
    internal class OggStreamer : IDisposable
    {
        const float DefaultUpdateRate = 10;
        const int DefaultBufferSize = 44100;

        static readonly object singletonMutex = new object();

        readonly object iterationMutex = new object();
        readonly object readMutex = new object();

        readonly float[] readSampleBuffer;
        readonly short[] castBuffer;

        readonly HashSet<OggStream> streams = new HashSet<OggStream>();
        readonly List<OggStream> threadLocalStreams = new List<OggStream>();

        readonly Thread underlyingThread;
        volatile bool cancelled;

        public float UpdateRate { get; private set; }
        public int BufferSize { get; private set; }

        static OggStreamer instance;
        public static OggStreamer Instance
        {
            get
            {
                lock (singletonMutex)
                {
                    if (instance == null)
                        throw new InvalidOperationException("No instance running");
                    return instance;
                }
            }
            private set { lock (singletonMutex) instance = value; }
        }

        public OggStreamer(int bufferSize = DefaultBufferSize, float updateRate = DefaultUpdateRate)
        {
            UpdateRate = updateRate;
            BufferSize = bufferSize;

            lock (singletonMutex)
            {
                if (instance != null)
                    throw new InvalidOperationException("Already running");

                Instance = this;
                underlyingThread = new Thread(EnsureBuffersFilled) { Priority = ThreadPriority.Lowest };
                underlyingThread.Start();
            }

            readSampleBuffer = new float[bufferSize];
            castBuffer = new short[bufferSize];
        }

        public void Dispose()
        {
            lock (singletonMutex)
            {
                Debug.Assert(Instance == this, "Two instances running, somehow...?");

                cancelled = true;
                lock (iterationMutex)
                    streams.Clear();

                Instance = null;
            }
        }

        internal bool AddStream(OggStream stream)
        {
            lock (iterationMutex)
                return streams.Add(stream);
        }

        internal bool RemoveStream(OggStream stream)
        {
            lock (iterationMutex)
                return streams.Remove(stream);
        }

        public bool FillBuffer(OggStream stream, int bufferId)
        {
            int readSamples;
            lock (readMutex)
            {
                readSamples = stream.Reader.ReadSamples(readSampleBuffer, 0, BufferSize);
                CastBuffer(readSampleBuffer, castBuffer, readSamples);
            }
            AL.BufferData(bufferId, stream.Reader.Channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16, castBuffer,
                readSamples * sizeof(short), stream.Reader.SampleRate);
            ALHelper.Check();

            return readSamples != BufferSize;
        }
        static void CastBuffer(float[] inBuffer, short[] outBuffer, int length)
        {
            for (int i = 0; i < length; i++)
            {
                var temp = (int)(32767f * inBuffer[i]);
                if (temp > short.MaxValue) temp = short.MaxValue;
                else if (temp < short.MinValue) temp = short.MinValue;
                outBuffer[i] = (short)temp;
            }
        }

        void EnsureBuffersFilled()
        {
            while (!cancelled)
            {
                Thread.Sleep((int)(1000 / ((UpdateRate <= 0) ? 1 : UpdateRate)));
                if (cancelled) break;

                threadLocalStreams.Clear();
                lock (iterationMutex) threadLocalStreams.AddRange(streams);

                foreach (var stream in threadLocalStreams)
                {
                    lock (stream.prepareMutex)
                    {
                        lock (iterationMutex)
                            if (!streams.Contains(stream))
                                continue;

                        bool finished = false;

                        int queued;
                        AL.GetSource(stream.alSourceId, ALGetSourcei.BuffersQueued, out queued);
                        ALHelper.Check();
                        int processed;
                        AL.GetSource(stream.alSourceId, ALGetSourcei.BuffersProcessed, out processed);
                        ALHelper.Check();

                        if (processed == 0 && queued == stream.BufferCount) continue;

                        int[] tempBuffers;
                        if (processed > 0)
                            tempBuffers = AL.SourceUnqueueBuffers(stream.alSourceId, processed);
                        else
                            tempBuffers = stream.alBufferIds.Skip(queued).ToArray();

                        for (int i = 0; i < tempBuffers.Length; i++)
                        {
                            finished |= FillBuffer(stream, tempBuffers[i]);

                            if (finished)
                            {
                                if (stream.IsLooped)
                                {
                                    stream.Close();
                                    stream.Open();
                                }
                                else
                                {
                                    streams.Remove(stream);
                                    i = tempBuffers.Length;
                                }

                                if (stream.FinishedAction != null)
                                    stream.FinishedAction.Invoke();
                            }
                        }

                        if (!finished)
                        {
                            AL.SourceQueueBuffers(stream.alSourceId, tempBuffers.Length, tempBuffers);
                            ALHelper.Check();
                        }
                        else if (!stream.IsLooped)
                            continue;
                    }

                    lock (stream.stopMutex)
                    {
                        if (stream.Preparing) continue;

                        lock (iterationMutex)
                            if (!streams.Contains(stream))
                                continue;

                        var state = AL.GetSourceState(stream.alSourceId);
                        if (state == ALSourceState.Stopped)
                        {
                            AL.SourcePlay(stream.alSourceId);
                            ALHelper.Check();
                        }
                    }
                }
            }
        }
    }
}
