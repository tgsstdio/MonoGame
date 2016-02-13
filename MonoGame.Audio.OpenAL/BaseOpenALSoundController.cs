using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame.Audio.OpenAL
{
	public abstract class BaseOpenALSoundController : IOpenALSoundController
    {
		protected IOALSourceArray AvailableSources;
        protected List<IOALSoundBuffer> InUse;
		protected List<IOALSoundBuffer> NowPlaying;
		protected List<IOALSoundBuffer> PurgeMe;
        protected bool HardwareAvailable = false;

		private readonly IOpenALSoundContext mContext;

        /// <summary>
        /// Sets up the hardware resources used by the controller.
        /// </summary>
		protected BaseOpenALSoundController(IOpenALSoundContext context, IOALSourceArray sources)
        {
			mContext = context;
			// DO we have hardware here and it is ready ?
			HardwareAvailable = mContext.InitialisationError == null;

			AvailableSources = sources;
			InUse = new List<IOALSoundBuffer>();
			NowPlaying = new List<IOALSoundBuffer>();
			PurgeMe = new List<IOALSoundBuffer>();
		}

		~BaseOpenALSoundController()
        {
            Dispose(false);
        }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Destroys the AL context and closes the device, when they exist.
		/// </summary>
		protected abstract void CleanUpOpenAL ();

		private bool _isDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (HardwareAvailable)
                    {
                        CleanUpOpenAL();
                    }
                }
                _isDisposed = true;
            }
		}

		public bool ReserveSource (IOALSoundBuffer soundBuffer)
		{
            if (!CheckInitState())
            {
                return(false);
            }
            int sourceNumber;
			if (AvailableSources.IsEmpty()) {

				soundBuffer.SourceId = 0;
				return false;
			}			

			sourceNumber = AvailableSources.First ();
			soundBuffer.SourceId = sourceNumber;
			InUse.Add (soundBuffer);

			AvailableSources.Remove (sourceNumber);

			//sourceId = sourceNumber;
			return true;
		}

		public void RecycleSource (IOALSoundBuffer soundBuffer)
		{
            if (!CheckInitState())
            {
                return;
            }
            InUse.Remove(soundBuffer);
			AvailableSources.Add (soundBuffer.SourceId);
			soundBuffer.RecycleSoundBuffer();
		}

		public abstract void PlaySound (IOALSoundBuffer soundBuffer);

		public abstract void StopSound (IOALSoundBuffer soundBuffer);

		public void PauseSound (IOALSoundBuffer soundBuffer)
		{
            if (!CheckInitState())
            {
                return;
            }
            soundBuffer.Pause();
		}

		public void ResumeSound(IOALSoundBuffer soundBuffer)
        {
            if (!CheckInitState())
            {
                return;
            }
            soundBuffer.Resume();
        }

		public abstract bool IsState (IOALSoundBuffer soundBuffer, int state); 

        /// <summary>
        /// Checks if the AL controller was initialized properly. If there was an
        /// exception thrown during the OpenAL init, then that exception is thrown
        /// inside of NoAudioHardwareException.
        /// </summary>
        /// <returns>True if the controller was initialized, false if not.</returns>
        protected bool CheckInitState()
        {
            if (!HardwareAvailable)
            {
				if (mContext.InitialisationError != null)
                {
					Exception e = mContext.InitialisationError;
					//TODO : do we need to clear error 
					//mContext.InitialisationError = null;
                    throw new NoAudioHardwareException("No audio hardware available.", e);
                }
                return (false);
            }
            return (true);
        }

		public abstract double SourceCurrentPosition (int sourceId);
		public abstract void Update ();

	}
}

