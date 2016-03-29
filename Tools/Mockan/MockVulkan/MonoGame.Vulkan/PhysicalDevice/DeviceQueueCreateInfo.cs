﻿using System;

namespace MonoGame.Graphics
{
	public class DeviceQueueCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
		public UInt32 QueueCount { get; set; }
		public float[] QueuePriorities { get; set; }
	}
}

