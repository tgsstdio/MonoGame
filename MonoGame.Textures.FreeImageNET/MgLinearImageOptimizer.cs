using System;
using Magnesium;
using System.Diagnostics;

namespace MonoGame.Textures.FreeImageNET
{
	public class MgLinearImageOptimizer : IMgTextureGenerator
	{
		IMgThreadPartition mPartition;
		IMgImageTools mImageTools;
		public MgLinearImageOptimizer (IMgThreadPartition partition, IMgImageTools imageTools)
		{
			mPartition = partition;
			mImageTools = imageTools;
		}

		#region IMgImageOptimizer implementation

		public MgTexture Load (System.IO.Stream fs, MgImageSource source)
		{
			// Prefer using optimal tiling, as linear tiling 
			// may support only a small set of features 
			// depending on implementation (e.g. no mip maps, only one layer, etc.)

			IMgImage mappableImage;
			IMgDeviceMemory mappableMemory;

			uint mipLevels = (uint)source.Mipmaps.Length;
			// Load mip map level 0 to linear tiling image
			var  imageCreateInfo = new MgImageCreateInfo
			{
				ImageType = MgImageType.TYPE_2D,
				Format = source.Format,
				MipLevels = mipLevels,
				ArrayLayers = 1,
				Samples =  MgSampleCountFlagBits.COUNT_1_BIT,
				Tiling = MgImageTiling.LINEAR,
				Usage =  MgImageUsageFlagBits.SAMPLED_BIT,
				SharingMode = MgSharingMode.EXCLUSIVE,
				InitialLayout = MgImageLayout.PREINITIALIZED,
				Extent = new MgExtent3D{
					Width = source.Width,
					Height = source.Height,
					Depth = 1 },
			};

			var device = mPartition.Device;

			var err = device.CreateImage(imageCreateInfo, null, out mappableImage);
			Debug.Assert(err == Result.SUCCESS);

			// Get memory requirements for this image 
			// like size and alignment
			MgMemoryRequirements memReqs;
			device.GetImageMemoryRequirements(mappableImage, out memReqs);
			// Set memory allocation size to required memory size
			var memAllocInfo = new MgMemoryAllocateInfo
			{
				AllocationSize = memReqs.Size,
			};

			// Get memory type that can be mapped to host memory
			uint memoryTypeIndex;
			bool isValid = mPartition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out memoryTypeIndex);
			Debug.Assert(isValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;

			// Allocate host memory
			err = device.AllocateMemory(memAllocInfo, null, out mappableMemory);
			Debug.Assert(err == Result.SUCCESS);

			// Bind allocated image for use
			err = mappableImage.BindImageMemory(device, mappableMemory, 0);
			Debug.Assert(err == Result.SUCCESS);

			// Get sub resource layout
			// Mip map count, array layer, etc.
			var subRes = new MgImageSubresource
			{
				AspectMask = MgImageAspectFlagBits.COLOR_BIT,
			};

			MgSubresourceLayout subResLayout;
			IntPtr data;

			// Get sub resources layout 
			// Includes row pitch, size offsets, etc.
			device.GetImageSubresourceLayout(mappableImage, subRes, out subResLayout);

			// Map image memory
			err = mappableMemory.MapMemory(device, 0, memReqs.Size, 0, out data);
			Debug.Assert(err == Result.SUCCESS);

			// Copy image data into memory
			//memcpy(data, tex2D[subRes.mipLevel].data(), tex2D[subRes.mipLevel].size());

			mappableMemory.UnmapMemory(device);

			// Linear tiled images don't need to be staged
			// and can be directly used as textures
			var texture = new MgTexture {
				Image = mappableImage,
				DeviceMemory = mappableMemory,
				ImageLayout =  MgImageLayout.SHADER_READ_ONLY_OPTIMAL,
			};

			var cmdPool = mPartition.CommandPool;

			var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
			{
				CommandPool = cmdPool,
				Level = MgCommandBufferLevel.PRIMARY,
				CommandBufferCount = 1,
			};

			var commands = new IMgCommandBuffer[1];
			err =  device.AllocateCommandBuffers(cmdBufAllocateInfo, commands);
			Debug.Assert (err == Result.SUCCESS);

			var cmdBufInfo = new MgCommandBufferBeginInfo
			{
				Flags = 0,
			};

			var copyCmd = commands [0];
			err = commands[0].BeginCommandBuffer(cmdBufInfo);
			Debug.Assert (err == Result.SUCCESS);

			// Setup image memory barrier transfer image to shader read layout
			setImageLayout(
				copyCmd, 
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT, 
				MgImageLayout.PREINITIALIZED,
				texture.ImageLayout,
				0,
				mipLevels);

			err = copyCmd.EndCommandBuffer();
			Debug.Assert (err == Result.SUCCESS);

			var submitInfo = new MgSubmitInfo {				
				CommandBuffers = commands,
			};

			var queue = mPartition.Queue;
			queue.QueueSubmit(new[] {submitInfo}, null);

			err = queue.QueueWaitIdle();
			Debug.Assert (err == Result.SUCCESS);

			device.FreeCommandBuffers(cmdPool, commands);

			return texture;
		}

		// Create an image memory barrier for changing the layout of
		// an image and put it into an active command buffer
		void setImageLayout(
			IMgCommandBuffer cmdBuffer,
			IMgImage image, 
			MgImageAspectFlagBits aspectMask,
			MgImageLayout oldImageLayout,
			MgImageLayout newImageLayout,
			uint mipLevel,
			uint mipLevelCount)
		{
			// Create an image barrier object
			var imageMemoryBarrier = new MgImageMemoryBarrier () {
				OldLayout = oldImageLayout,
				NewLayout = newImageLayout,
				Image = image,
				SubresourceRange = new MgImageSubresourceRange {
					AspectMask = aspectMask,
					BaseMipLevel = mipLevel,
					LevelCount = mipLevelCount,
					LayerCount = 1,
				},
			};

			// Only sets masks for layouts used in this example
			// For a more complete version that can be used with
			// other layouts see vkTools::setImageLayout

			// Source layouts (new)

			if (oldImageLayout == MgImageLayout.PREINITIALIZED)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// Target layouts (new)

			// New layout is transfer destination (copy, blit)
			// Make sure any reads from and writes to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_DST_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT | MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// New layout is shader read (sampler, input attachment)
			// Make sure any writes to the image have been finished
			if (newImageLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.SHADER_READ_BIT;
			}

			// New layout is transfer source (copy, blit)
			// Make sure any reads from and writes to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_SRC_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT | MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// Put barrier on top
			MgPipelineStageFlagBits srcStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;
			MgPipelineStageFlagBits destStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;

			const int VK_FLAGS_NONE  = 0;

			// Put barrier inside setup command buffer
			cmdBuffer.CmdPipelineBarrier(
				srcStageFlags, 
				destStageFlags, 
				VK_FLAGS_NONE, 
				null,
				null,
				new []{imageMemoryBarrier});
		}

		#endregion
	}
}

