// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System;

namespace Microsoft.Xna.Framework.Audio
{
	public class SoundEffectInstancePool : ISoundEffectInstancePool
    {

#if WINDOWS || (WINRT && !WINDOWS_PHONE) || DESKTOPGL || WEB || ANGLE

        // These platforms are only limited by memory.
        private const int MAX_PLAYING_INSTANCES = int.MaxValue;

#elif MONOMAC

        // Reference: http://stackoverflow.com/questions/3894044/maximum-number-of-openal-sound-buffers-on-iphone
        private const int MAX_PLAYING_INSTANCES = 256;

#elif WINDOWS_PHONE

        // Reference: http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.audio.instanceplaylimitexception.aspx
        private const int MAX_PLAYING_INSTANCES = 64;

#elif IOS

        // Reference: http://stackoverflow.com/questions/3894044/maximum-number-of-openal-sound-buffers-on-iphone
        private const int MAX_PLAYING_INSTANCES = 32;

#elif ANDROID

        // Set to the same as OpenAL on iOS
        internal const int MAX_PLAYING_INSTANCES = 32;

#endif
		private readonly List<ISoundEffectInstance> _playingInstances;
		private readonly List<ISoundEffectInstance> _pooledInstances;

		private ISoundEffectInstancePoolPlatform mPoolPlatform;
		private ISoundEffectInstanceFactory mFactory;
		public SoundEffectInstancePool(ISoundEffectInstancePoolPlatform platform, ISoundEffectInstanceFactory factory)
        {
			//mPlatform.MAX_PLAYING_INSTANCES = 0;
            // Reduce garbage generation by allocating enough capacity for
            // the maximum playing instances or at least some reasonable value.
			mPoolPlatform = platform;
			mFactory = factory;
			var maxInstances = mPoolPlatform.MAX_PLAYING_INSTANCES < 1024 ? mPoolPlatform.MAX_PLAYING_INSTANCES : 1024;
			_playingInstances = new List<ISoundEffectInstance>(maxInstances);
			_pooledInstances = new List<ISoundEffectInstance>(maxInstances);
        }

        /// <summary>
        /// Gets a value indicating whether the platform has capacity for more sounds to be played at this time.
        /// </summary>
        /// <value><c>true</c> if more sounds can be played; otherwise, <c>false</c>.</value>
        public bool SoundsAvailable
        {
            get
            {
				return _playingInstances.Count < mPoolPlatform.MAX_PLAYING_INSTANCES;
            }
        }

        /// <summary>
        /// Add the specified instance to the pool if it is a pooled instance and removes it from the
        /// list of playing instances.
        /// </summary>
        /// <param name="inst">The SoundEffectInstance</param>
		internal void Add(ISoundEffectInstance inst)
        {
			if (inst.IsPooled)
            {
                _pooledInstances.Add(inst);
                inst.Effect = null;
            }

            _playingInstances.Remove(inst);
        }

        /// <summary>
        /// Adds the SoundEffectInstance to the list of playing instances.
        /// </summary>
        /// <param name="inst">The SoundEffectInstance to add to the playing list.</param>
		public void Remove(ISoundEffectInstance inst)
        {
            _playingInstances.Add(inst);
        }

        /// <summary>
        /// Returns a pooled SoundEffectInstance if one is available, or allocates a new
        /// SoundEffectInstance if the pool is empty.
        /// </summary>
        /// <returns>The SoundEffectInstance.</returns>
		public ISoundEffectInstance GetInstance(bool forXAct)
        {
            ISoundEffectInstance inst = null;
            var count = _pooledInstances.Count;
            if (count > 0)
            {
                // Grab the item at the end of the list so the remove doesn't copy all
                // the list items down one slot.
                inst = _pooledInstances[count - 1];
                _pooledInstances.RemoveAt(count - 1);

                // Reset used instance to the "default" state.
                inst.IsPooled = true;
                inst.IsXAct = forXAct;
                inst.Volume = 1.0f;
                inst.Pan = 0.0f;
                inst.Pitch = 0.0f;
                inst.IsLooped = false;
            }
            else
            {
				// TODO : remove instance stuff
				inst = mFactory.CreateNewInstance(this);
                inst.IsPooled = true;
                inst.IsXAct = forXAct;
            }

            return inst;
        }

        /// <summary>
        /// Iterates the list of playing instances, returning them to the pool if they
        /// have stopped playing.
        /// </summary>
        public void Update()
        {
			mPoolPlatform.BeforeUpdate ();

            ISoundEffectInstance inst = null;
            // Cleanup instances which have finished playing.                    
            for (var x = 0; x < _playingInstances.Count;)
            {
                inst = _playingInstances[x];

                if (inst.State == SoundState.Stopped || inst.IsDisposed || inst.Effect == null)
                {
                    Add(inst);
                    continue;
                }
                else if (inst.Effect.IsDisposed)
                {
                    Add(inst);
                    // Instances created through SoundEffect.CreateInstance need to be disposed when
                    // their owner SoundEffect is disposed.
                    if (!inst.IsPooled)
                        inst.Dispose();
                    continue;
                }

                x++;
            }
        }

        public void UpdateMasterVolume()
        {
            foreach (var inst in _playingInstances)
            {
                // XAct sounds are not controlled by the SoundEffect
                // master volume, so we can skip them completely.
                if (inst.IsXAct)
                    continue;

                // Re-applying the volume to itself will update
                // the sound with the current master volume.
                inst.Volume = inst.Volume;
            }
        }
    }
}
