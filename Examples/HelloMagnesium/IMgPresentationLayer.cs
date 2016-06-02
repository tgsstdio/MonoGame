using Magnesium;
using System;

namespace HelloMagnesium
{
	public interface IMgPresentationLayer : IDisposable
	{
		void Initialize();
		IMgSurfaceKHR Surface { get; }
	}
}

