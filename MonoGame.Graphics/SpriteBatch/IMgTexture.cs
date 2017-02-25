using Magnesium;

namespace MonoGame.Graphics
{
	public interface IMgTexture
	{
		IMgImage Image { get; }
		IMgImageView View { get; }
		IMgDeviceMemory DeviceMemory { get; }
		IMgSampler Sampler {get; }

		void DestroyTexture(IMgDevice device, IMgAllocationCallbacks allocator);
        uint Width { get; }
        uint Height { get; }
        uint Depth { get; }
        uint MipmapLevels { get; }
        uint ArrayLayers { get; }
	}

}

