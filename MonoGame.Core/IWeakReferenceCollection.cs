using System;

namespace Microsoft.Xna.Framework
{
	[CLSCompliant(true)]
	public interface IWeakReferenceCollection : IDisposable
	{
		void AddResourceReference(WeakReference resourceReference);
		void RemoveResourceReference(WeakReference resourceReference);
	}
}

