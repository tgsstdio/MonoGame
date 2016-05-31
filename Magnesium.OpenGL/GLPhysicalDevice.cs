using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public enum GLMemoryBufferType : int 
	{
		SSBO = 0,
		INDIRECT,
		VERTEX, 
		INDEX,
	}

	public static class GLMemoryBufferExtensions 
	{
		public static BufferTarget GetBufferTarget(this GLMemoryBufferType bufferType)
		{
			switch(bufferType)
			{
			case GLMemoryBufferType.SSBO:
				return BufferTarget.ShaderStorageBuffer;
			case GLMemoryBufferType.INDEX:
				return BufferTarget.ElementArrayBuffer;
			case GLMemoryBufferType.VERTEX:
				return BufferTarget.ArrayBuffer;
			case GLMemoryBufferType.INDIRECT:
				return BufferTarget.DrawIndirectBuffer;
			default:
				throw new NotSupportedException ();
			}
		}

		public static uint GetMask(this GLMemoryBufferType bufferType)
		{
			switch(bufferType)
			{
			case GLMemoryBufferType.SSBO:
				return 1 << 0;
			case GLMemoryBufferType.INDIRECT:
				return 1 << 1;
			case GLMemoryBufferType.VERTEX:
				return 1 << 2;
			case GLMemoryBufferType.INDEX:
				return 1 << 3;
			default:
				throw new NotSupportedException ();
			}
		}
	}

	public class GLPhysicalDevice : IMgPhysicalDevice
	{
		private readonly GLDevice mDevice;
		public GLPhysicalDevice (IGLQueue queue)
		{
			mDevice = new GLDevice (queue);
		}

		#region IMgPhysicalDevice implementation
		public void GetPhysicalDeviceProperties (out MgPhysicalDeviceProperties pProperties)
		{
			pProperties = new MgPhysicalDeviceProperties ();
		}
		public void GetPhysicalDeviceQueueFamilyProperties (out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			// ONE QUEUE FOR ALL
			pQueueFamilyProperties = new [] {
				new MgQueueFamilyProperties {					
					QueueFlags = MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT,
				}
			};
		}
		public void GetPhysicalDeviceMemoryProperties (out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			// TODO : overwrite here to shift memory based on which type
			// 0 : buffer based 
			// 1 : host defined (for INDIRECT)
			pMemoryProperties = new MgPhysicalDeviceMemoryProperties();
			var slots = new MgMemoryType[4];

			const MgMemoryPropertyFlagBits allOn = 
				MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT |
				MgMemoryPropertyFlagBits.HOST_CACHED_BIT | 
				MgMemoryPropertyFlagBits.HOST_COHERENT_BIT | 
				MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT | 
				MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT;

			slots [(int) GLMemoryBufferType.SSBO] = new MgMemoryType{ PropertyFlags = allOn};
			slots [(int) GLMemoryBufferType.INDIRECT] = new MgMemoryType{ PropertyFlags = allOn };
			slots [(int) GLMemoryBufferType.VERTEX] = new MgMemoryType{ PropertyFlags = allOn };
			slots [(int) GLMemoryBufferType.INDEX] = new MgMemoryType{ PropertyFlags = allOn };

			pMemoryProperties.MemoryTypes = slots;
		}
		public void GetPhysicalDeviceFeatures (out MgPhysicalDeviceFeatures pFeatures)
		{
			pFeatures = new MgPhysicalDeviceFeatures ();
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
			// USING SINGLE DEVICE & SINGLE QUEUE 
				// SHOULD BE 
			pDevice = mDevice;
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
		public Result GetPhysicalDeviceSurfaceSupportKHR (uint queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceCapabilitiesKHR (IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceFormatsKHR (IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfacePresentModesKHR (IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
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

