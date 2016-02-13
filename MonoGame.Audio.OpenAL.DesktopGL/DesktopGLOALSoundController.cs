using MonoGame.Audio.OpenAL; 
using OpenTK.Audio.OpenAL;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLOALSoundController : BaseOpenALSoundController
    {
        /// <summary>
        /// Sets up the hardware resources used by the controller.
        /// </summary>
		public DesktopGLOALSoundController (IOpenALSoundContext context, IOALSourceArray sources)
			: base (context, sources)
        {

		}

		public override void PlaySound (IOALSoundBuffer soundBuffer)
        {
            if (!CheckInitState())
            {
                return;
            }
			lock (NowPlaying)
            {
				NowPlaying.Add (soundBuffer);
            }
			AL.SourcePlay (soundBuffer.SourceId);
		}

		public override void StopSound (IOALSoundBuffer soundBuffer)
        {
            if (!CheckInitState())
            {
                return;
            }
            AL.SourceStop(soundBuffer.SourceId);

            AL.Source (soundBuffer.SourceId, ALSourcei.Buffer, 0);
			lock (NowPlaying) {
				NowPlaying.Remove (soundBuffer);
            }
            RecycleSource (soundBuffer);
		}

		public override bool IsState (IOALSoundBuffer soundBuffer, int state)
		{
            if (!CheckInitState())
            {
                return (false);
            }
            int sourceState;

			AL.GetSource (soundBuffer.SourceId, ALGetSourcei.SourceState, out sourceState);

			if (state == sourceState) {
				return true;
			}

			return false;
		}

        public override double SourceCurrentPosition (int sourceId)
		{
            if (!CheckInitState())
            {
                return(0.0);
            }
            int pos;
			AL.GetSource (sourceId, ALGetSourcei.SampleOffset, out pos);
			return pos;
		}

		public override void Update()
        {
			if (!HardwareAvailable)
            {
                //OK to ignore this here because the game can run without sound.
                 return;
            }

            ALSourceState state;

			lock (NowPlaying)
            {
				for (int i = NowPlaying.Count - 1; i >= 0; --i)
                {
					var soundBuffer = NowPlaying[i];
                    state = AL.GetSourceState(soundBuffer.SourceId);
                    if (state == ALSourceState.Stopped)
                    {
						PurgeMe.Add(soundBuffer);
						NowPlaying.RemoveAt(i);
                    }
                }
            }
            lock (PurgeMe)
            {
				foreach (var soundBuffer in PurgeMe)
                {
                    AL.Source(soundBuffer.SourceId, ALSourcei.Buffer, 0);
                    RecycleSource(soundBuffer);
                }
				PurgeMe.Clear();
            }
        }

		protected override void CleanUpOpenAL ()
		{

		}
	}
}

