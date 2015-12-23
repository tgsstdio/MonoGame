using System;

namespace Microsoft.Xna.Framework
{
	public interface IWeakReferenceCollection
	{
		void AddResourceReference(WeakReference resourceReference);
		void RemoveResourceReference(WeakReference resourceReference);
	}
}

