using Magnesium;

namespace MonoGame.Textures.FreeImageNET
{
	public class MgTexture
	{
		public MgImageLayout ImageLayout { get; set;}
		public IMgImage Image { get; set;}
		public IMgDeviceMemory DeviceMemory { get; set;}
	}
}

