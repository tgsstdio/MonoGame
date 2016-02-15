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
		public int MaximumPlayingInstances {
			get {
				return int.MaxValue;
			}
		}

		public float Epsilon {
			get {
				return float.Epsilon;
			}
		}

		public void BeforeUpdate ()
		{
			mController.Update();
		}

		#endregion
	}
}

