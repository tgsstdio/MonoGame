using System;

namespace MonoGame.Graphics.Vk
{
	public class Win32SurfaceCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public IntPtr Hinstance { get; set; }
		public IntPtr Hwnd { get; set; }
	}
}

