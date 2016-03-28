using System;

namespace MonoGame.Graphics.Vk
{
	public class ShaderModuleCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UIntPtr CodeSize { get; set; }
		public IntPtr Code { get; set; }
	}
}

