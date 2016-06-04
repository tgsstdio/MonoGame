using MonoGame.Core;
using System;

namespace MonoGame.Core
{
	public interface IClientWindowBounds
	{
		Rectangle ClientBounds { get; set; }
		void OnClientSizeChanged ();
		event EventHandler<EventArgs> ClientSizeChanged;
		void ChangeClientBounds(Rectangle bounds);
	}
}

