using System;

namespace Microsoft.Xna.Framework
{
	public class PlatformActivator : IPlatformActivator
	{
		public event EventHandler<EventArgs> Activated;
		public event EventHandler<EventArgs> Deactivated;

		#region IPlatformActivator implementation

		private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
			where TEventArgs : EventArgs
		{
			if (handler != null)
				handler(this, e);
		}

		private bool _isActive;
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					Raise(_isActive ? Activated : Deactivated, EventArgs.Empty);
				}
			}
		}

		public void AddActivatedHandler (EventHandler<EventArgs> func)
		{
			Activated += func;
		}

		public void AddDeactivatedHandler (EventHandler<EventArgs> func)
		{
			Deactivated += func;
		}

		public void RemoveActivatedHandler (EventHandler<EventArgs> func)
		{
			Activated -= func;
		}

		public void RemoveDeactivatedHandler (EventHandler<EventArgs> func)
		{
			Deactivated -= func;
		}

		#endregion


	}
}

