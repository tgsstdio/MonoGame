using System;

namespace MonoGame.Graphics.Vk
{
	public interface IEntrypoint
	{
		Result CreateInstance(InstanceCreateInfo createInfo, AllocationCallbacks allocator, out IInstance instance);
		Result EnumerateInstanceLayerProperties(out LayerProperties[] properties);
		Result EnumerateInstanceExtensionProperties(string layerName, out ExtensionProperties[] pProperties);
	}
}

