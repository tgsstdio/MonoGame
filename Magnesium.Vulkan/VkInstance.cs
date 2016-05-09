using System;

namespace Magnesium.Vulkan
{
	public class VkInstance : IMgInstance
	{
		public int Index { get; set; }
		public IntPtr Handle { get; set; }

		#region IMgInstance implementation

		public void DestroyInstance (MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		public Result EnumeratePhysicalDevices (out IMgPhysicalDevice[] physicalDevices)
		{
			throw new System.NotImplementedException ();
		}

		public PFN_vkVoidFunction GetInstanceProcAddr (string pName)
		{
			throw new System.NotImplementedException ();
		}

		public Result CreateDisplayPlaneSurfaceKHR (MgDisplaySurfaceCreateInfoKHR createInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface)
		{
			throw new System.NotImplementedException ();
		}

		public void DestroySurfaceKHR (MgSurfaceKHR surface, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		public Result CreateWin32SurfaceKHR (MgWin32SurfaceCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface)
		{
			throw new System.NotImplementedException ();
		}

		public Result CreateDebugReportCallbackEXT (MgDebugReportCallbackCreateInfoEXT pCreateInfo, MgAllocationCallbacks allocator, out MgDebugReportCallbackEXT pCallback)
		{
			throw new System.NotImplementedException ();
		}

		public void DestroyDebugReportCallbackEXT (MgDebugReportCallbackEXT callback, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		public void DebugReportMessageEXT (MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, System.IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
		{
			throw new System.NotImplementedException ();
		}

		#endregion
	}

}

