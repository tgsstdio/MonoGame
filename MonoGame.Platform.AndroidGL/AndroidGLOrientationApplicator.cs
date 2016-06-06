using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidGLOrientationApplicator
	{
		public AndroidGLOrientationSetter WindowingState { get; private set; }
		private readonly IGraphicsDeviceManager mManager;
		public AndroidGLOrientationApplicator (AndroidGLOrientationSetter windowing, IGraphicsDeviceManager manager)
		{
			WindowingState = windowing;
			mManager = manager;
		}

		/// <summary>
		/// Updates the screen orientation. Filters out requests for unsupported orientations.
		/// </summary>
		public void ApplyOrientation(DisplayOrientation newOrientation)
		{

			if (WindowingState.SetOrientation(newOrientation))
				mManager.ApplyChanges();
		}
	}
}

