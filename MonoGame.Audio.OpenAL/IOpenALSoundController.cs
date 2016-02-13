using System;

namespace MonoGame.Audio.OpenAL
{
	public interface IOpenALSoundController : IDisposable
	{
		void RecycleSource (IOALSoundBuffer soundBuffer);
		void StopSound (IOALSoundBuffer soundBuffer);
		void PauseSound (IOALSoundBuffer soundBuffer);

		/// <summary>
		/// Reserves the given sound buffer. If there are no available sources then false is
		/// returned, otherwise true will be returned and the sound buffer can be played. If
		/// the controller was not able to setup the hardware, then false will be returned.
		/// </summary>
		/// <param name="soundBuffer">The sound buffer you want to play</param>
		/// <returns>True if the buffer can be played, and false if not.</returns>
		bool ReserveSource (IOALSoundBuffer soundBuffer);

		void ResumeSound(IOALSoundBuffer soundBuffer);

        /// <summary>
        /// Called repeatedly, this method cleans up the state of the management lists. This method
        /// will also lock on the playingSourcesCollection. Sources that are stopped will be recycled
        /// using the RecycleSource method.
        /// </summary>
		void Update();
	}
}

