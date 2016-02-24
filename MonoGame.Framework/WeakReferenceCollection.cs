using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
	public class WeakReferenceCollection : IWeakReferenceCollection
	{
		// Resources may be added to and removed from the list from many threads.
		private readonly object _resourcesLock = new object();

		// Use WeakReference for the global resources list as we do not know when a resource
		// may be disposed and collected. We do not want to prevent a resource from being
		// collected by holding a strong reference to it in this list.
		private readonly List<WeakReference> _resources = new List<WeakReference>();

		public void AddResourceReference(WeakReference resourceReference)
		{
			lock (_resourcesLock)
			{
				_resources.Add(resourceReference);
			}
		}

		public void RemoveResourceReference(WeakReference resourceReference)
		{
			lock (_resourcesLock)
			{
				_resources.Remove(resourceReference);
			}
		}

		~WeakReferenceCollection()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources ()
		{
			lock (_resourcesLock)
			{
				foreach (var resource in _resources)
				{
					var target = resource.Target as IDisposable;
					if (target != null)
						target.Dispose ();
				}
				_resources.Clear ();
			}
		}

		private bool mDisposed = false;
		private void Dispose (bool disposing)
		{
			if (mDisposed)
			{
				return;
			}

			if (disposing)
			{
				// managed stuff here
			}
			
			ReleaseUnmanagedResources ();

			mDisposed = true;
		}

	}
}

