using System;
using System.IO;

namespace MonoGame.Graphics.Vk
{
	public class ShaderModuleCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Stream Code { get; set; }
	}
}

