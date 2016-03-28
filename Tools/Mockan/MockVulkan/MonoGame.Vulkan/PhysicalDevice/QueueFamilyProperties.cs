using System;

namespace MonoGame.Graphics.Vk
{
	public class QueueFamilyProperties
	{
		public QueueFlagBits QueueFlags { get; set; }
		public UInt32 QueueCount { get; set; }
		public UInt32 TimestampValidBits { get; set; }
		public Extent3D MinImageTransferGranularity { get; set; }
	}
}

