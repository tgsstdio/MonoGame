using System;
using Magnesium;

namespace HelloMagnesium
{
	public interface IMgLogicalDevice : IDisposable
	{
		IMgPhysicalDevice GPU { get;  }
		IMgDevice Device { get;  }
		IMgQueueInfo[] Queues { get; }
	}
}

