using System;

namespace MonoGame.Graphics.Vk
{
	public class PhysicalDeviceMemoryProperties
	{
		public MemoryType[] MemoryTypes { get; set; }
		public MemoryHeap[] MemoryHeaps { get; set; }
	}
}

