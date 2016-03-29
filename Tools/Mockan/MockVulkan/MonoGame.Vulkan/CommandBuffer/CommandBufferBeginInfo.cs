using System;

namespace MonoGame.Graphics.Vk
{
	public class CommandBufferBeginInfo
	{
		public CommandBufferUsageFlagBits Flags { get; set; }
		public CommandBufferInheritanceInfo InheritanceInfo { get; set; }
	}
}

