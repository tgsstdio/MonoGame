using System;

namespace Magnesium.OpenGL
{
	public class GLPhysicalDevice : IMgPhysicalDevice
	{
		#region IMgPhysicalDevice implementation
		public void GetPhysicalDeviceProperties (out MgPhysicalDeviceProperties pProperties)
		{
			throw new NotImplementedException ();
		}
		public void GetPhysicalDeviceQueueFamilyProperties (out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			throw new NotImplementedException ();
		}
		public void GetPhysicalDeviceMemoryProperties (out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			throw new NotImplementedException ();
		}
		public void GetPhysicalDeviceFeatures (out MgPhysicalDeviceFeatures pFeatures)
		{
			throw new NotImplementedException ();
		}
		public void GetPhysicalDeviceFormatProperties (MgFormat format, out MgFormatProperties pFormatProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceImageFormatProperties (MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException ();
		}
		public Result CreateDevice (MgDeviceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			pDevice = new GLDevice ();
			return Result.SUCCESS;
		}
		public Result EnumerateDeviceLayerProperties (out MgLayerProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result EnumerateDeviceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public void GetPhysicalDeviceSparseImageFormatProperties (MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceDisplayPropertiesKHR (out MgDisplayPropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceDisplayPlanePropertiesKHR (out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayPlaneSupportedDisplaysKHR (uint planeIndex, out MgDisplayKHR[] pDisplays)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayModePropertiesKHR (MgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayPlaneCapabilitiesKHR (MgDisplayModeKHR mode, uint planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceSupportKHR (uint queueFamilyIndex, MgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceCapabilitiesKHR (MgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceFormatsKHR (MgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfacePresentModesKHR (MgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			throw new NotImplementedException ();
		}
		public bool GetPhysicalDeviceWin32PresentationSupportKHR (uint queueFamilyIndex)
		{
			throw new NotImplementedException ();
		}
		#endregion
		
	}
}

