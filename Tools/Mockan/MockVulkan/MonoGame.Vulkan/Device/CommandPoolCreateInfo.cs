using System;

namespace MonoGame.Graphics
{
	public class CommandPoolCreateInfo
	{
		public CommandPoolCreateFlagBits Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
	}

}

