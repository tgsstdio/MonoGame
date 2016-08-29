using System;
using Magnesium;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HelloMagnesium
{
	public class VkBuffer
	{
		public VkBuffer(IMgThreadPartition partition, MgBufferUsageFlagBits usage, uint bufferSize)
		{
			var memoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT;

			var bufferCreateInfo = new MgBufferCreateInfo {
				Usage = usage,
				Size = bufferSize,
			};

			IMgBuffer buffer;

			var device = partition.Device;

			var result = device.CreateBuffer(bufferCreateInfo, null, out buffer);
			Debug.Assert (result == Result.SUCCESS);

			MgMemoryRequirements memReqs;
			device.GetBufferMemoryRequirements(buffer, out memReqs);

			uint memoryTypeIndex;
			partition.GetMemoryType (memReqs.MemoryTypeBits, memoryPropertyFlags, out memoryTypeIndex);

			var memAlloc = new MgMemoryAllocateInfo
			{
				MemoryTypeIndex = memoryTypeIndex,
				AllocationSize = memReqs.Size,
			};

			IMgDeviceMemory deviceMemory;
			result = device.AllocateMemory(memAlloc, null, out deviceMemory);
			Debug.Assert (result == Result.SUCCESS);

			buffer.BindBufferMemory(device, deviceMemory, 0);

			BufferSize = bufferSize;
			Buffer = buffer;
			DeviceMemory = deviceMemory;
		}

		public void Destroy(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			Buffer.DestroyBuffer (device, allocator);
			DeviceMemory.FreeMemory (device, allocator);
		}

		public uint BufferSize { get; private set;	}
		public IMgBuffer Buffer { get; private set; }
		public IMgDeviceMemory DeviceMemory { get; private set; }

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="srcStartIndex">Source start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, float[] data, int srcStartIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			const int stride = sizeof(float);
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, srcStartIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, int[] data, int startIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			const int stride = sizeof(int);
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, startIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, double[] data, int startIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			const int stride = sizeof(double);
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, startIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, IntPtr[] data, int startIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			int stride = Marshal.SizeOf(typeof(IntPtr));
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, startIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, byte[] data, int startIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			const int stride = sizeof(byte);
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, startIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		public Result SetData(IMgDevice device, uint sizeInBytes, short[] data, int startIndex, int elementCount)
		{
			if (data == null)
				return Result.SUCCESS;

			const int stride = sizeof(short);
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);

			Marshal.Copy (data, startIndex, dest, elementCount);

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}

		/// <summary>
		/// Map memory, then copies data then unmaps device memory
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="device">Device.</param>
		/// <param name="sizeInBytes">Size in bytes.</param>
		/// <param name="data">Data.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="elementCount">Element count.</param>
		/// <typeparam name="TData">The 1st type parameter.</typeparam>
		public Result SetData<TData>(IMgDevice device, uint sizeInBytes, TData[] data, int startIndex, int elementCount)
			where TData : struct
		{
			if (data == null)
				return Result.SUCCESS;

			int stride = Marshal.SizeOf(typeof(TData));
			if (sizeInBytes < (stride * elementCount))
			{
				throw new ArgumentOutOfRangeException ("sizeInBytes"); 
			}

			IntPtr dest;
			var result = DeviceMemory.MapMemory (device, 0, sizeInBytes, 0, out dest);
			Debug.Assert (result == Result.SUCCESS);


			// Copy the struct to unmanaged memory.	
			int offset = 0;
			for(int i = 0; i < elementCount; ++i)
			{
				IntPtr localDest = IntPtr.Add(dest, offset);
				Marshal.StructureToPtr(data[i + startIndex], localDest, false);
				offset += stride;
			}

			DeviceMemory.UnmapMemory (device);

			return Result.SUCCESS;
		}
	}

}

