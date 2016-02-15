using Microsoft.Xna.Framework.Audio;

namespace HelloCube
{
	public class NullSoundEffectInstanceFactory : ISoundEffectInstanceFactory
	{
		#region ISoundEffectInstanceFactory implementation
		public ISoundEffectInstance CreateNewInstance (ISoundEffectInstancePool parent)
		{
			throw new System.NotImplementedException ();
		}
		public ISoundEffectInstance CreateNewInstance ()
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}
