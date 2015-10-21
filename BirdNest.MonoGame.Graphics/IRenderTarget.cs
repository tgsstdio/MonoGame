using System;

namespace BirdNest.MonoGame.Graphics
{
	public interface IRenderTarget : IDisposable
	{
		InstanceIdentifier Id { get; }
		void Switch();
	}
}

