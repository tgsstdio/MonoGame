using System;

namespace MonoGame.Graphics.Vk
{
	public interface IPhysicalDevice
	{
		void GetPhysicalDeviceProperties(out PhysicalDeviceProperties pProperties);
		void GetPhysicalDeviceQueueFamilyProperties(out QueueFamilyProperties[] pQueueFamilyProperties);
		void GetPhysicalDeviceMemoryProperties(out PhysicalDeviceMemoryProperties pMemoryProperties);
		void GetPhysicalDeviceFeatures(out PhysicalDeviceFeatures pFeatures);
		void GetPhysicalDeviceFormatProperties(Format format, out FormatProperties pFormatProperties);
		Result GetPhysicalDeviceImageFormatProperties(Format format, ImageType type, ImageTiling tiling, ImageUsageFlagBits usage, ImageCreateFlagBits flags, out ImageFormatProperties pImageFormatProperties);
		Result CreateDevice(DeviceCreateInfo pCreateInfo, AllocationCallbacks allocator, out IDevice pDevice);
		Result EnumerateDeviceLayerProperties(out LayerProperties[] pProperties);
		Result EnumerateDeviceExtensionProperties(string pLayerName, out ExtensionProperties[] pProperties);
		void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlagBits samples, ImageUsageFlagBits usage, ImageTiling tiling, out SparseImageFormatProperties[] pProperties);
		Result GetPhysicalDeviceDisplayPropertiesKHR(out DisplayPropertiesKHR[] pProperties);
		Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out DisplayPlanePropertiesKHR[] pProperties);
		Result GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out DisplayKHR[] pDisplays);
		Result GetDisplayModePropertiesKHR(DisplayKHR display, out DisplayModePropertiesKHR[] pProperties);
		//Result CreateDisplayModeKHR(DisplayKHR display, DisplayModeCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out DisplayModeKHR pMode);
		Result GetDisplayPlaneCapabilitiesKHR(DisplayModeKHR mode, UInt32 planeIndex, out DisplayPlaneCapabilitiesKHR pCapabilities);
		Result GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, SurfaceKHR surface, ref bool pSupported);
		Result GetPhysicalDeviceSurfaceCapabilitiesKHR(SurfaceKHR surface, out SurfaceCapabilitiesKHR pSurfaceCapabilities);
		Result GetPhysicalDeviceSurfaceFormatsKHR(SurfaceKHR surface, out SurfaceFormatKHR[] pSurfaceFormats);
		Result GetPhysicalDeviceSurfacePresentModesKHR(SurfaceKHR surface, out PresentModeKHR[] pPresentModes);
		bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex);
	}
}

