using System;
using Magnesium;
using MonoGame.Platform.DesktopGL;
using System.Diagnostics;

namespace HelloMagnesium
{
	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	public class Win32Swapchain : ISwapchain
	{
		private IOpenTKGameWindow mWindow;
		private IMgImageTools mImageTools;
		public Win32Swapchain (IOpenTKGameWindow window, IMgImageTools imageTools)
		{
			mWindow = window;
			mImageTools = imageTools;
		}

		#region ISwapchain implementation
		private IMgInstance mInstance;
		private IMgPhysicalDevice mPhysicalDevice;
		private IMgDevice mDevice;
		public void Setup (IMgInstance instance, IMgPhysicalDevice physicalDevice, IMgDevice device)
		{
			mInstance = instance;
			mPhysicalDevice = physicalDevice;
			mDevice = device;
		}

		private IMgSurfaceKHR mSurface;

		Result GenerateSurface ()
		{
			var createInfo = new MgWin32SurfaceCreateInfoKHR {
				// DOUBLE CHECK 
				Hinstance = Process.GetCurrentProcess ().Handle,
				Hwnd = mWindow.GetWindowInfo ().Handle,
			};
			var err = mInstance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
			Debug.Assert (err == Result.SUCCESS);
			return err;
		}

		public uint Initialise ()
		{
			var err = GenerateSurface ();

			// Get available queue family properties
			MgQueueFamilyProperties[] queueProps;
			mPhysicalDevice.GetPhysicalDeviceQueueFamilyProperties(out queueProps);

			bool[] supportsPresent = new bool[queueProps.Length];

			// Iterate over each queue to learn whether it supports presenting:
			for (UInt32 i = 0; i < queueProps.Length; ++i)
			{
				mPhysicalDevice.GetPhysicalDeviceSurfaceSupportKHR(i, mSurface, ref supportsPresent[i]);
			}

			// Search for a graphics and a present queue in the array of queue
			// families, try to find one that supports both
			UInt32 graphicsQueueNodeIndex = UInt32.MaxValue;
			UInt32 presentQueueNodeIndex = UInt32.MaxValue;
			for (UInt32 i = 0; i < queueProps.Length; i++)
			{
				var queue = queueProps[i];

				if ((queue.QueueFlags & MgQueueFlagBits.GRAPHICS_BIT) != 0)
				{
					if (graphicsQueueNodeIndex == UInt32.MaxValue)
					{
						graphicsQueueNodeIndex = i;
					}

					if (supportsPresent[i])
					{
						graphicsQueueNodeIndex = i;
						presentQueueNodeIndex = i;
						break;
					}
				}
			}

			if (presentQueueNodeIndex == UInt32.MaxValue)
			{
				// If didn't find a queue that supports both graphics and present, then
				// find a separate present queue.
				for (UInt32 i = 0; i < queueProps.Length; ++i)
				{
					if (supportsPresent[i])
					{
						presentQueueNodeIndex = i;
						break;
					}
				}
			}

			// Generate error if could not find both a graphics and a present queue
			if (graphicsQueueNodeIndex == UInt32.MaxValue ||
				presentQueueNodeIndex == UInt32.MaxValue)
			{
				throw new Exception("Swapchain Initialization Failure - Could not find a graphics and a present queue");
			}

			// VERBATIM from cube.c
			// TODO: Add support for separate queues, including presentation,
			//       synchronization, and appropriate tracking for QueueSubmit.
			// NOTE: While it is possible for an application to use a separate graphics
			//       and a present queues, this demo program assumes it is only using
			//       one:
			if (graphicsQueueNodeIndex != presentQueueNodeIndex)
			{
				throw new Exception("Swapchain Initialization Failure - Could not find a common graphics and a present queue");
			}

			// Get the list of VkFormat's that are supported:
			MgSurfaceFormatKHR[] surfFormats;
			err = mPhysicalDevice.GetPhysicalDeviceSurfaceFormatsKHR(mSurface, out surfFormats);
			Debug.Assert(err == Result.SUCCESS);

			// If the format list includes just one entry of VK_FORMAT_UNDEFINED,
			// the surface has no preferred format.  Otherwise, at least one
			// supported format will be returned.
			if (surfFormats.Length == 1 && surfFormats[0].Format == MgFormat.UNDEFINED)
			{
				mFormat = MgFormat.B8G8R8A8_UNORM;
			}
			else
			{
				Debug.Assert(surfFormats.Length >= 1);
				mFormat = surfFormats[0].Format;
			}
			mColorSpace = surfFormats[0].ColorSpace;

			return graphicsQueueNodeIndex;
		}
		private MgFormat mFormat;
		private MgColorSpaceKHR mColorSpace;

		private IMgSwapchainKHR mSwapChain;
		private uint mWidth;
		private uint mHeight;

		public void Create (IMgCommandBuffer cmd, uint width, uint height)
		{
			mWidth = width;
			mHeight = height;

			Result err;
			IMgSwapchainKHR oldSwapchain = mSwapChain;

			// Get physical device surface properties and formats
			MgSurfaceCapabilitiesKHR surfCaps;
			err = mPhysicalDevice.GetPhysicalDeviceSurfaceCapabilitiesKHR(mSurface, out surfCaps);
			Debug.Assert(err == Result.SUCCESS);

			// Get available present modes
			MgPresentModeKHR[] presentModes;
			err = mPhysicalDevice.GetPhysicalDeviceSurfacePresentModesKHR(mSurface, out presentModes);
			Debug.Assert(err == Result.SUCCESS);

			var swapchainExtent = new MgExtent2D {};
			// width and height are either both -1, or both not -1.
			if (surfCaps.CurrentExtent.Width == -1)
			{
				// If the surface size is undefined, the size is set to
				// the size of the images requested.
				swapchainExtent.Width = mWidth;
				swapchainExtent.Height = mHeight;
			}
			else
			{
				// If the surface size is defined, the swap chain size must match
				swapchainExtent = surfCaps.CurrentExtent;
				mWidth = surfCaps.CurrentExtent.Width;
				mHeight = surfCaps.CurrentExtent.Height;
			}

			// Prefer mailbox mode if present, it's the lowest latency non-tearing present  mode
			MgPresentModeKHR swapchainPresentMode = MgPresentModeKHR.FIFO_KHR;
			for (uint i = 0; i < presentModes.Length; i++) 
			{
				if (presentModes[i] == MgPresentModeKHR.MAILBOX_KHR) 
				{
					swapchainPresentMode = MgPresentModeKHR.MAILBOX_KHR;
					break;
				}
				if ((swapchainPresentMode != MgPresentModeKHR.MAILBOX_KHR) && (presentModes[i] == MgPresentModeKHR.IMMEDIATE_KHR)) 
				{
					swapchainPresentMode = MgPresentModeKHR.IMMEDIATE_KHR;
				}
			}

			// Determine the number of images
			uint desiredNumberOfSwapchainImages = surfCaps.MinImageCount + 1;
			if ((surfCaps.MaxImageCount > 0) && (desiredNumberOfSwapchainImages > surfCaps.MaxImageCount))
			{
				desiredNumberOfSwapchainImages = surfCaps.MaxImageCount;
			}

			MgSurfaceTransformFlagBitsKHR preTransform;
			if ((surfCaps.SupportedTransforms & MgSurfaceTransformFlagBitsKHR.IDENTITY_BIT_KHR) != 0)
			{
				preTransform = MgSurfaceTransformFlagBitsKHR.IDENTITY_BIT_KHR;
			}
			else
			{
				preTransform = surfCaps.CurrentTransform;
			}

			MgSwapchainCreateInfoKHR swapchainCI = new MgSwapchainCreateInfoKHR {
				Surface = mSurface,
				MinImageCount = desiredNumberOfSwapchainImages,
				ImageFormat = mFormat,
				ImageColorSpace = mColorSpace,
				ImageExtent = swapchainExtent,
				ImageUsage = MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT,
				PreTransform = (MgSurfaceTransformFlagBitsKHR)preTransform,
				ImageArrayLayers = 1,
				ImageSharingMode = MgSharingMode.EXCLUSIVE,
				QueueFamilyIndices = null,
				PresentMode = swapchainPresentMode,
				OldSwapchain = oldSwapchain,
				Clipped = true,
				CompositeAlpha = MgCompositeAlphaFlagBitsKHR.OPAQUE_BIT_KHR,
			};

			err = mDevice.CreateSwapchainKHR(swapchainCI, null, out mSwapChain);
			Debug.Assert(err == Result.SUCCESS);

			// If an existing swap chain is re-created, destroy the old swap chain
			// This also cleans up all the presentable images
			if (oldSwapchain != null) 
			{ 
				for (uint i = 0; i < mImageCount; i++)
				{
					mBuffers[i].view.DestroyImageView(mDevice, null);
				}
				oldSwapchain.DestroySwapchainKHR(mDevice, null);
			}

			// Get the swap chain images
			err = mDevice.GetSwapchainImagesKHR(mSwapChain, out mImages);
			Debug.Assert(err == Result.SUCCESS);

			// Get the swap chain buffers containing the image and imageview
			mImageCount = (uint)mImages.Length;
			mBuffers = new SwapChainBuffer[mImageCount];
			for (uint i = 0; i < mImageCount; i++)
			{
				var buffer = new SwapChainBuffer ();
				var colorAttachmentView = new MgImageViewCreateInfo{
					Format = mFormat,
					Components = new MgComponentMapping{
						R = MgComponentSwizzle.R,
						G = MgComponentSwizzle.G,
						B = MgComponentSwizzle.B,
						A = MgComponentSwizzle.A,
					},
					SubresourceRange = new MgImageSubresourceRange
					{
						AspectMask = MgImageAspectFlagBits.COLOR_BIT,
						BaseMipLevel = 0,
						LevelCount = 1,
						BaseArrayLayer = 0,
						LayerCount = 1,
					
					},
					ViewType = MgImageViewType.TYPE_2D,
					Flags = 0,
				};

				buffer.image = mImages[i];

				// Transform images from initial (undefined) to present layout
				mImageTools.SetImageLayout(
					cmd, 
					buffer.image, 
					MgImageAspectFlagBits.COLOR_BIT, 
					MgImageLayout.UNDEFINED, 
					MgImageLayout.PRESENT_SRC_KHR);

				colorAttachmentView.Image = buffer.image;

				err = mDevice.CreateImageView(colorAttachmentView, null, out buffer.view);
				Debug.Assert(err == Result.SUCCESS);

				mBuffers [i] = buffer;
			}
		}

		~Win32Swapchain()
		{
			Dispose (false);
		}

		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		// Free all Vulkan resources used by the swap chain
		private bool mIsDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			for (uint i = 0; i < mImageCount; i++)
			{
				mBuffers[i].view.DestroyImageView(mDevice, null);
			}
			mSwapChain.DestroySwapchainKHR(mDevice, null);
			mSurface.DestroySurfaceKHR(mInstance, null);

			mIsDisposed = true;
		}


		private class SwapChainBuffer
		{
			public IMgImage image;
			public IMgImageView view;
		}

		private IMgImage[] mImages;
		private uint mImageCount;
		private SwapChainBuffer[] mBuffers;

		#endregion
	}
}

