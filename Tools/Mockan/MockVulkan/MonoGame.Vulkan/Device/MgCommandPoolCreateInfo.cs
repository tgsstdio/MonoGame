using System;

namespace MonoGame.Graphics
{
	public class MgCommandPoolCreateInfo
	{
		public MgCommandPoolCreateFlagBits Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
	}

}

