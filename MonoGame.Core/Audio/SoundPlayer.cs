using System;

namespace Microsoft.Xna.Framework.Audio
{
	public class SoundPlayer : ISoundPlayer
	{
		private readonly ISoundEffectInstancePool mPool;
		public SoundPlayer(ISoundEffectInstancePool pool)
        {
			mPool = pool;
		}

		#region Play

		/// <summary>Gets an internal SoundEffectInstance and plays it.</summary>
		/// <returns>True if a SoundEffectInstance was successfully played, false if not.</returns>
		/// <remarks>
		/// <para>Play returns false if more SoundEffectInstances are currently playing then the platform allows.</para>
		/// <para>To loop a sound or apply 3D effects, call SoundEffect.CreateInstance() and SoundEffectInstance.Play() instead.</para>
		/// <para>SoundEffectInstances used by SoundEffect.Play() are pooled internally.</para>
		/// </remarks>
		public bool Play(SoundEffect effect)
		{
			var inst = GetPooledInstance(false, effect);
			if (inst == null)
				return false;

			inst.Play();

			return true;
		}

		/// <summary>Gets an internal SoundEffectInstance and plays it with the specified volume, pitch, and panning.</summary>
		/// <returns>True if a SoundEffectInstance was successfully created and played, false if not.</returns>
		/// <param name = "effect"></param>
		/// <param name="volume">Volume, ranging from 0.0 (silence) to 1.0 (full volume). Volume during playback is scaled by SoundEffect.MasterVolume.</param>
		/// <param name="pitch">Pitch adjustment, ranging from -1.0 (down an octave) to 0.0 (no change) to 1.0 (up an octave).</param>
		/// <param name="pan">Panning, ranging from -1.0 (left speaker) to 0.0 (centered), 1.0 (right speaker).</param>
		/// <remarks>
		/// <para>Play returns false if more SoundEffectInstances are currently playing then the platform allows.</para>
		/// <para>To apply looping or simulate 3D audio, call SoundEffect.CreateInstance() and SoundEffectInstance.Play() instead.</para>
		/// <para>SoundEffectInstances used by SoundEffect.Play() are pooled internally.</para>
		/// </remarks>
		public bool Play(SoundEffect effect, float volume, float pitch, float pan)
		{
			var inst = GetPooledInstance(false, effect);
			if (inst == null)
				return false;

			inst.Volume = volume;
			inst.Pitch = pitch;
			inst.Pan = pan;

			inst.Play();

			return true;
		}

		/// <summary>
		/// Returns a sound effect instance from the pool or null if none are available.
		/// </summary>
		public ISoundEffectInstance GetPooledInstance(bool forXAct, SoundEffect effect)
		{
			if (!mPool.SoundsAvailable)
				return null;

			var inst = mPool.GetInstance(forXAct);
			inst.Effect = effect;
			effect.Platform.SetupInstance(inst);

			return inst;
		}

		#endregion
	}
}

