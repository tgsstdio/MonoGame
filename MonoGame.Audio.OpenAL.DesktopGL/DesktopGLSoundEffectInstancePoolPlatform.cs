using Microsoft.Xna.Framework.Audio;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLSoundEffectInstancePoolPlatform : ISoundEffectInstancePoolPlatform
	{
		private readonly IOpenALSoundController mController;
		public DesktopGLSoundEffectInstancePoolPlatform (IOpenALSoundController controller)
		{
			mController = controller;
		}

		#region ISoundEffectInstancePoolPlatform implementation
		public int MAX_PLAYING_INSTANCES {
			get {
				return int.MaxValue;
			}
		}
		#endregion

		public void BeforeUpdate ()
		{
			mController.Update();
		}
	}
}

