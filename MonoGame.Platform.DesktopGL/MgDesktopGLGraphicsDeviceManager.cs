using MonoGame.Graphics;
using Magnesium;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

namespace MonoGame.Platform.DesktopGL
{
	public class MgDesktopGLGraphicsDeviceManager : MgGraphicsDeviceManager
	{
		readonly IOpenTKWindowResetter mWindowReset;
		public MgDesktopGLGraphicsDeviceManager(
			IMgGraphicsDevice device,
			IMgThreadPartition partition,
			IPresentationParameters presentationParameters,
			IMgSwapchainCollection swapchainCollection,
			IGraphicsAdapterCollection adapters,
			IGraphicsProfiler profiler,
			IOpenTKWindowResetter windowReset
		)
		: base (device, partition, presentationParameters, swapchainCollection, adapters, profiler)
		{
			mWindowReset = windowReset;
		}

		#region implemented abstract members of MgGraphicsDeviceManager

		public override void ResetClientBounds ()
		{

		}

		protected override void ApplyLocalChanges ()
		{
			mWindowReset.ResetWindowBounds ();
		}

		protected override void ForceSetFullScreen ()
		{
			
		}

		protected override void AfterDeviceCreation ()
		{
		}

		protected override void ApplySupportedOrientation (Microsoft.Xna.Framework.DisplayOrientation value)
		{

		}

		protected override void ReleaseManagedResources ()
		{

		}

		protected override void ReleaseUnmanagedResources ()
		{

		}

		#endregion
	}
}

