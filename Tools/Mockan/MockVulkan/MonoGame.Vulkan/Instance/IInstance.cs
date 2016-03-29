using System;

namespace MonoGame.Graphics
{
	public interface IInstance
	{
		void DestroyInstance(MgAllocationCallbacks allocator);
		Result EnumeratePhysicalDevices(out IPhysicalDevice[] physicalDevices);
		PFN_vkVoidFunction GetInstanceProcAddr(string pName);
		Result CreateDisplayPlaneSurfaceKHR(DisplaySurfaceCreateInfoKHR createInfo, MgAllocationCallbacks allocator, out SurfaceKHR pSurface);
		void DestroySurfaceKHR(SurfaceKHR surface, MgAllocationCallbacks allocator);
		Result CreateWin32SurfaceKHR(Win32SurfaceCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out SurfaceKHR pSurface);
		Result CreateDebugReportCallbackEXT(DebugReportCallbackCreateInfoEXT pCreateInfo, MgAllocationCallbacks allocator, out DebugReportCallbackEXT pCallback);
		void DestroyDebugReportCallbackEXT(DebugReportCallbackEXT callback, MgAllocationCallbacks allocator);
		void DebugReportMessageEXT(DebugReportFlagBitsEXT flags, DebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
	}
}

