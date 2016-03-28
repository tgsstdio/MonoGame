using System;

namespace MonoGame.Graphics.Vk
{
	public interface IInstance
	{
		void DestroyInstance(AllocationCallbacks allocator);
		Result EnumeratePhysicalDevices(out IPhysicalDevice[] physicalDevices);
		PFN_vkVoidFunction GetInstanceProcAddr(string pName);
		Result CreateDisplayPlaneSurfaceKHR(DisplaySurfaceCreateInfoKHR createInfo, AllocationCallbacks allocator, out SurfaceKHR pSurface);
		void DestroySurfaceKHR(SurfaceKHR surface, AllocationCallbacks allocator);
		Result CreateWin32SurfaceKHR(Win32SurfaceCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out SurfaceKHR pSurface);
		Result CreateDebugReportCallbackEXT(DebugReportCallbackCreateInfoEXT pCreateInfo, AllocationCallbacks allocator, out DebugReportCallbackEXT pCallback);
		void DestroyDebugReportCallbackEXT(DebugReportCallbackEXT callback, AllocationCallbacks allocator);
		void DebugReportMessageEXT(DebugReportFlagBitsEXT flags, DebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
	}
}

