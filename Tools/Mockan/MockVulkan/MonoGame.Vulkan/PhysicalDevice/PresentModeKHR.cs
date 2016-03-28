using System;

namespace MonoGame.Graphics.Vk
{
	public enum PresentModeKHR : byte
	{
		IMMEDIATE_KHR = 0,
		MAILBOX_KHR = 1,
		FIFO_KHR = 2,
		FIFO_RELAXED_KHR = 3,
	}
}

