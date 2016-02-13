using System;

namespace Microsoft.Xna.Framework
{
	public interface IPlatformActivator
	{
		bool IsActive { get; set; }

		void AddActivatedHandler (EventHandler<EventArgs> func);
		void AddDeactivatedHandler (EventHandler<EventArgs> func);
		void RemoveActivatedHandler (EventHandler<EventArgs> func);
		void RemoveDeactivatedHandler (EventHandler<EventArgs> func);
	}
}

