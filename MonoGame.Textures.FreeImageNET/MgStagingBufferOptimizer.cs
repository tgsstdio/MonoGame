using System;
using Magnesium;
using System.Diagnostics;

namespace MonoGame.Textures.FreeImageNET
{
	public class MgStagingBufferOptimizer : IMgTextureGenerator
	{
		private IMgThreadPartition mPartition;
		public MgStagingBufferOptimizer (IMgThreadPartition partition)
		{
			mPartition = partition;
		}

		#region IMgImageOptimizer implementation

		// FROM texture.cpp (2016) Sascha Williams
		public MgTexture Load (System.IO.Stream fs, MgImageSource source)
		{
			var device = mPartition.Device;
			var queue = mPartition.Queue;
			var cmdPool = mPartition.CommandPool;
			uint mipLevels = (uint)source.Mipmaps.Length;

			// Create a host-visible staging buffer that contains the raw image data
			IMgBuffer stagingBuffer;
			IMgDeviceMemory stagingMemory;

			var bufferCreateInfo = new MgBufferCreateInfo {
				Size = source.Size,
				Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
				SharingMode = MgSharingMode.EXCLUSIVE,
			};

			// This buffer is used as a transfer source for the buffer copy
			var result = device.CreateBuffer(bufferCreateInfo, null, out stagingBuffer);
			Debug.Assert (result == Result.SUCCESS);

			MgMemoryRequirements memReqs;
			// Get memory requirements for the staging buffer (alignment, memory type bits)
			mPartition.Device.GetBufferMemoryRequirements(stagingBuffer, out memReqs);

			var memAllocInfo = new MgMemoryAllocateInfo {
				AllocationSize = memReqs.Size,
			};

			// Get memory type index for a host visible buffer
			uint memoryTypeIndex;
			bool isTypeValid = getMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out memoryTypeIndex);
			Debug.Assert (isTypeValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;


			result = device.AllocateMemory(memAllocInfo, null, out stagingMemory);
			Debug.Assert (result == Result.SUCCESS);

			result = stagingBuffer.BindBufferMemory (device, stagingMemory, 0);
			Debug.Assert (result == Result.SUCCESS);

			// Copy texture data into staging buffer
			IntPtr data;
			result = stagingMemory.MapMemory (device, 0, memReqs.Size, 0, out data);
			Debug.Assert (result == Result.SUCCESS);

			// TODO : Copy here

			stagingMemory.UnmapMemory (device);

			// Setup buffer copy regions for each mip level
			var bufferCopyRegions = new MgBufferImageCopy[source.Mipmaps.Length];

			for (uint i = 0; i < bufferCopyRegions.Length; ++i)
			{
				bufferCopyRegions [i] = new MgBufferImageCopy {
					ImageSubresource = new MgImageSubresourceLayers{
						AspectMask = MgImageAspectFlagBits.COLOR_BIT,
						MipLevel = i,
						BaseArrayLayer = 0,
						LayerCount = 1,
					},
					ImageExtent = new MgExtent3D {
						Width = source.Mipmaps[i].Width,
						Height = source.Mipmaps[i].Height,
						Depth = 1,
					},
					BufferOffset = source.Mipmaps[i].Offset,
				};
			}

			// Create optimal tiled target image
			var imageCreateInfo = new MgImageCreateInfo
			{
				ImageType =  MgImageType.TYPE_2D,
				Format = source.Format,
				MipLevels = mipLevels,
				ArrayLayers = 1,
				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
				Tiling = MgImageTiling.OPTIMAL,
				SharingMode = MgSharingMode.EXCLUSIVE,
				InitialLayout =  MgImageLayout.PREINITIALIZED,
				Extent = new MgExtent3D
					{
						Width = source.Width,
						Height = source.Height,
						Depth = 1
					},
				Usage = MgImageUsageFlagBits.TRANSFER_DST_BIT | MgImageUsageFlagBits.SAMPLED_BIT,
			};

			var texture = new MgTexture ();

			IMgImage image;
			result = device.CreateImage(imageCreateInfo, null, out image);
			Debug.Assert (result == Result.SUCCESS);
			texture.Image = image;

			device.GetImageMemoryRequirements(texture.Image, out memReqs);
			memAllocInfo.AllocationSize = memReqs.Size;

			isTypeValid = getMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out memoryTypeIndex);
			Debug.Assert (isTypeValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;

			IMgDeviceMemory deviceMem;
			result = device.AllocateMemory(memAllocInfo, null, out deviceMem);
			Debug.Assert (result == Result.SUCCESS);
			texture.DeviceMemory = deviceMem; 

			result = texture.Image.BindImageMemory(device, texture.DeviceMemory, 0);
			Debug.Assert (result == Result.SUCCESS);

			var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
			{
				CommandPool = cmdPool,
				Level = MgCommandBufferLevel.PRIMARY,
				CommandBufferCount = 1,
			};

			var commands = new IMgCommandBuffer[1];
			result =  device.AllocateCommandBuffers(cmdBufAllocateInfo, commands);
			Debug.Assert (result == Result.SUCCESS);

			var cmdBufInfo = new MgCommandBufferBeginInfo
			{
				Flags = 0,
			};

			var copyCmd = commands [0];
			result = commands[0].BeginCommandBuffer(cmdBufInfo);
			Debug.Assert (result == Result.SUCCESS);

			// Image barrier for optimal image (target)
			// Optimal image will be used as destination for the copy
			setImageLayout(
				copyCmd,
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT,
				MgImageLayout.PREINITIALIZED,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				0,
				mipLevels);

			// Copy mip levels from staging buffer
			copyCmd.CmdCopyBufferToImage(
				stagingBuffer,
				texture.Image,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				bufferCopyRegions
			);

			// Change texture image layout to shader read after all mip levels have been copied
			texture.ImageLayout = MgImageLayout.SHADER_READ_ONLY_OPTIMAL;
			setImageLayout(
				copyCmd,
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				texture.ImageLayout,
				0,
				mipLevels);

			result = copyCmd.EndCommandBuffer();
			Debug.Assert (result == Result.SUCCESS);

			var submitInfo = new MgSubmitInfo {				
				CommandBuffers = commands,
			};

			queue.QueueSubmit(new[] {submitInfo}, null);
			
			result = queue.QueueWaitIdle();
			Debug.Assert (result == Result.SUCCESS);
	
			device.FreeCommandBuffers(cmdPool, commands);

			// Clean up staging resources
			stagingMemory.FreeMemory(device, null);
			stagingBuffer.DestroyBuffer(device, null);

			return texture;
		}

		private bool getMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex)
		{
			typeIndex = 0;
			return true;
		}

		void setImageLayout(
			IMgCommandBuffer cmdbuffer, 
			IMgImage image, 
			MgImageAspectFlagBits aspectMask, 
			MgImageLayout oldImageLayout, 
			MgImageLayout newImageLayout,
			uint mipLevel,
			uint mipLevelCount		
		)
		{

		}

		#endregion
	}
}

