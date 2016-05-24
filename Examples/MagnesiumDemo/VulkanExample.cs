using System;
using Magnesium;
using Magnesium.Vulkan;
using System.Collections.Generic;

namespace MagnesiumDemo
{
	public class VulkanExample
	{
		readonly IMgEntrypoint mEntryPtr;

		public VulkanExample ()
		{
			mEntryPtr = new VkEntrypoint ();
			InitVulkan ();
		}

		public void GetLayerProperties()
		{
			MgLayerProperties[] output;
			var result = mEntryPtr.EnumerateInstanceLayerProperties (out output);
		}

		public void InitVulkan()
		{
			var err = CreateInstance ();
		}

		IMgInstance instance;
		Result CreateInstance()
		{
			const UInt32 MAJOR = 2;
			const UInt32 MINOR = 0;
			const UInt32 PATCH = 0;

			var appInfo = new MgApplicationInfo
			{
				ApplicationName = "VulkanExample",
				EngineName = "Magnesium",
				ApiVersion = (((MAJOR) << 22) | ((MINOR) << 12) | (PATCH))
			};

			const string VK_KHR_SURFACE_EXTENSION_NAME = "VK_KHR_surface";

			var enabledExtensions = new List<string>{ VK_KHR_SURFACE_EXTENSION_NAME };

			//#if WIN32
			const string VK_KHR_WIN32_SURFACE_EXTENSION_NAME = "VK_KHR_win32_surface";
			enabledExtensions.Add(VK_KHR_WIN32_SURFACE_EXTENSION_NAME);
//			#elif ANDROID
//			const string VK_KHR_ANDROID_SURFACE_EXTENSION_NAME = "VK_KHR_android_surface";
//			enabledExtensions.Add(VK_KHR_ANDROID_SURFACE_EXTENSION_NAME);
//			#elif LINUX
//			const string VK_KHR_XCB_SURFACE_EXTENSION_NAME = "VK_KHR_xcb_surface";
//			enabledExtensions.Add(VK_KHR_XCB_SURFACE_EXTENSION_NAME);
//			#endif

			var instanceCreateInfo = new MgInstanceCreateInfo
			{ 
				ApplicationInfo = appInfo,
			};

			const string VK_EXT_DEBUG_REPORT_EXTENSION_NAME = "VK_EXT_debug_report";

			bool enableValidation = true;

			if (enabledExtensions.Count > 0)
			{
				if (enableValidation)
				{
					enabledExtensions.Add(VK_EXT_DEBUG_REPORT_EXTENSION_NAME);
				}
				instanceCreateInfo.EnabledExtensionNames = enabledExtensions.ToArray ();
			}

			string[] VALIDATION_LAYER_NAMES = 
			{
				// This is a meta layer that enables all of the standard
				// validation layers in the correct order :
				// threading, parameter_validation, device_limits, object_tracker, image, core_validation, swapchain, and unique_objects
				"VK_LAYER_LUNARG_standard_validation"
			};

			if (enableValidation)
			{
				instanceCreateInfo.EnabledLayerNames = VALIDATION_LAYER_NAMES;
			}

			return mEntryPtr.CreateInstance (instanceCreateInfo, null, out instance);
		}
	}
}

