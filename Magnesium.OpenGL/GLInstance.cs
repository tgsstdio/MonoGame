using System;

namespace Magnesium.OpenGL
{
	public class GLInstance : IMgInstance
	{
		private GLPhysicalDevice[] mPhysicalDevices = new GLPhysicalDevice[1];
		public GLInstance ()
		{
			
		}

		#region IMgInstance implementation
		public void DestroyInstance (MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();		
		}

		public Result EnumeratePhysicalDevices (out IMgPhysicalDevice[] physicalDevices)
		{
			physicalDevices = mPhysicalDevices;
			return Result.SUCCESS;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr (string pName)
		{
			throw new NotImplementedException ();
		}

		public Result CreateDisplayPlaneSurfaceKHR (MgDisplaySurfaceCreateInfoKHR createInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}
		public void DestroySurfaceKHR (MgSurfaceKHR surface, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateWin32SurfaceKHR (MgWin32SurfaceCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}
		public Result CreateDebugReportCallbackEXT (MgDebugReportCallbackCreateInfoEXT pCreateInfo, MgAllocationCallbacks allocator, out MgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException ();
		}
		public void DestroyDebugReportCallbackEXT (MgDebugReportCallbackEXT callback, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void DebugReportMessageEXT (MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}

}

