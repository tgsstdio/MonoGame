using System;

namespace Microsoft.Xna.Framework.Audio
{
	public class SoundEnvironment : ISoundEnvironment
	{
		#region Static Members

		private float _distanceScale = 1.0f;
		/// <summary>
		/// Gets or sets the scale of distance calculations.
		/// </summary>
		/// <remarks> 
		/// <para>DistanceScale defaults to 1.0 and must be greater than 0.0.</para>
		/// <para>Higher values reduce the rate of falloff between the sound and listener.</para>
		/// </remarks>
		public float DistanceScale
		{
			get { return _distanceScale; }
			set
			{
				if (value <= 0f)
					throw new ArgumentOutOfRangeException ("value of DistanceScale");

				_distanceScale = value;
			}
		}

		private float _dopplerScale = 1f;
		/// <summary>
		/// Gets or sets the scale of Doppler calculations applied to sounds.
		/// </summary>
		/// <remarks>
		/// <para>DopplerScale defaults to 1.0 and must be greater or equal to 0.0</para>
		/// <para>Affects the relative velocity of emitters and listeners.</para>
		/// <para>Higher values more dramatically shift the pitch for the given relative velocity of the emitter and listener.</para>
		/// </remarks>
		public float DopplerScale
		{
			get { return _dopplerScale; }
			set
			{
				// As per documenation it does not look like the value can be less than 0
				//   although the documentation does not say it throws an error we will anyway
				//   just so it is like the DistanceScale
				if (value < 0.0f)
					throw new ArgumentOutOfRangeException ("value of DopplerScale");

				_dopplerScale = value;
			}
		}

		private float speedOfSound = 343.5f;
		/// <summary>Returns the speed of sound used when calculating the Doppler effect..</summary>
		/// <remarks>
		/// <para>Defaults to 343.5. Value is measured in meters per second.</para>
		/// <para>Has no effect on distance attenuation.</para>
		/// </remarks>
		public float SpeedOfSound
		{
			get { return speedOfSound; }
			set
			{
				if (value <= 0.0f)
					throw new ArgumentOutOfRangeException();

				speedOfSound = value;
			}
		}

		#endregion
	}
}

