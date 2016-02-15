using Microsoft.Xna.Framework.Audio;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLOALSoundEffectInstancePool : BaseSoundEffectInstancePool
	{
		private readonly IOpenALSoundController mController;
		public DesktopGLOALSoundEffectInstancePool (ISoundEffectInstancePoolPlatform platform, IOpenALSoundController controller)
			: base(platform)
		{
			mController = controller;
		}

		#region implemented abstract members of BaseSoundEffectInstancePool

		protected override ISoundEffectInstance CreateNewInstance ()
		{
			return new DesktopGLSoundEffectInstance (this, mController, new DesktopGLALSoundBuffer ());
		}

		#endregion
	}
}

