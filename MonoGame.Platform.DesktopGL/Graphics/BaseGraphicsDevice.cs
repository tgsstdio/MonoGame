using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public abstract class BaseGraphicsDevice : IGraphicsDevice
	{
		// FIXME: PLACEHOLDER
		private readonly IGraphicsDevicePlatform mPlatform;
		protected BaseGraphicsDevice (IGraphicsDevicePlatform platform, IPresentationParameters presentation, IGraphicsAdapter adapter, IWeakReferenceCollection resources)
		{
			mPlatform = platform;
			PresentationParameters = presentation;
			PresentationParameters.DepthStencilFormat = DepthFormat.Depth24;
			Adapter = adapter;
			WeakReferences = resources;
		}

		public virtual void Initialize()
		{
			SetupDevice ();
			ResetDevice ();
		}

		public IGraphicsAdapter Adapter
		{
			get;
			private set;
		}

		/// <summary>
		/// Setup this instance.
		/// allocate (i.e. new ) any specific members
		/// </summary>
		private void SetupDevice()
		{
			mPlatform.Setup ();

			// TODO : Desktop GL / AZDO implementation
			// all new calls
		}

		/// <summary>
		/// Initialize this instance 
		/// MORE of an reset
		/// </summary>
		private void ResetDevice()
		{
			mPlatform.Initialize();
		}

		#region IGraphicsDevice implementation

		~BaseGraphicsDevice()
		{
			Dispose(false);
		}

		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected abstract void ReleaseUnmanagedResources ();
		protected abstract void ReleaseManagedResources ();

		private bool _isDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			ReleaseUnmanagedResources ();
			if (disposing)
			{
				// Dispose of all remaining graphics resources before disposing of the graphics device				
				WeakReferences.Dispose ();
				ReleaseManagedResources ();
			}

			_isDisposed = true;
		}

		public void CreateDevice (IGraphicsAdapter adapter, GraphicsProfile graphicsProfile)
		{
			Adapter = adapter;
			SetupDevice();
			GraphicsProfile = graphicsProfile;
			ResetDevice();
		}

		public IWeakReferenceCollection WeakReferences {
			get;
			private set;
		}

		public GraphicsProfile GraphicsProfile {
			get;
			private set;
		}

		internal GraphicsMetrics _graphicsMetrics;
		public void Present ()
		{
			_graphicsMetrics = new GraphicsMetrics();
			mPlatform.Present();
		}

		public IPresentationParameters PresentationParameters {
			get;
			set;
		}

		#endregion


	}
}

