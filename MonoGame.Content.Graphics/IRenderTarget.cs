using System;

namespace MonoGame.Shaders
{
	public interface IRenderTarget : IDisposable
	{
		InstanceIdentifier Id { get; }
		void Switch();
	}
}

