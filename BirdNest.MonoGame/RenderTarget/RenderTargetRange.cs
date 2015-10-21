using System;
using System.Collections.Generic;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class RenderTargetRange : IRenderTargetRange
	{
		private readonly List<IRenderTarget> mTargets;
		public RenderTargetRange (Func<InstanceIdentifier> nextId)
		{
			Default = new NullRenderTarget(nextId);
			mTargets = new List<IRenderTarget> ();
		}

		#region IRenderTargetRange implementation

		public IRenderTarget Default {
			get;
			private set;
		}

		public IList<IRenderTarget> Targets {
			get {
				return mTargets;
			}
		}

		#endregion

		public void Add (IRenderTarget target)
		{
			mTargets.Add (target);
		}

		public void Clear ()
		{
			ReleaseManagedResources ();
		}

		~RenderTargetRange()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseManagedResources()
		{
			foreach (var target in mTargets)
			{
				target.Dispose ();
			}

			mTargets.Clear ();
		}


		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}
	}
}

