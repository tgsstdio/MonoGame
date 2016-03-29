using System;

namespace MonoGame.Graphics
{
	public interface IMgEntrypoint
	{
		Result CreateInstance(MgInstanceCreateInfo createInfo, MgAllocationCallbacks allocator, out IInstance instance);
		Result EnumerateInstanceLayerProperties(out LayerProperties[] properties);
		Result EnumerateInstanceExtensionProperties(string layerName, out ExtensionProperties[] pProperties);
	}
}

