﻿using System;

namespace MonoGame.Graphics.Vk
{
	public class MemoryRequirements
	{
		public UInt64 Size { get; set; }
		public UInt64 Alignment { get; set; }
		public UInt32 MemoryTypeBits { get; set; }
	}
}
