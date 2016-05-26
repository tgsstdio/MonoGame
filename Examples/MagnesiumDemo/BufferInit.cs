using System;
using Magnesium;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MagnesiumDemo
{
	public class BufferInit
	{
		class BufferData
		{
			public int count;
			public IMgBuffer buf;
			public IMgDeviceMemory mem;
		}

		public BufferInit ()
		{
			float dim = 1f;
			float[] vertexBuffer =
			{
				 dim,  dim, 0.0f,  1.0f, 1.0f ,
				 -dim,  dim, 0.0f, 0.0f, 1.0f ,
				 -dim, -dim, 0.0f, 0.0f, 0.0f ,
				 dim, -dim, 0.0f,  1.0f, 0.0f
			};

			var vertices = new BufferData();

			CreateBuffer<float> (
				MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
				(ulong)(vertexBuffer.Length * sizeof(float)),
				vertexBuffer,
				out vertices.buf,
				out vertices.mem);

			var indices = new BufferData();

			// Setup indices
			UInt32[] indexBuffer = { 0,1,2, 2,3,0 };
			indices.count = indexBuffer.Length;

			CreateBuffer<UInt32> (
				MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
				(ulong)(indexBuffer.Length * sizeof(UInt32)),
				indexBuffer,
				out indices.buf,
				out indices.mem);
		}

		private IMgDevice device;
		private MgPhysicalDeviceMemoryProperties mMemoryProperties;
		private bool getMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex)
		{
			// Search memtypes to find first index with those properties
			for (UInt32 i = 0; i < mMemoryProperties.MemoryTypes.Length; i++)
			{
				if ((typeBits & 1) == 1)
				{
					// Type is available, does it match user properties?
					if ((mMemoryProperties.MemoryTypes[i].PropertyFlags & memoryPropertyFlags) == memoryPropertyFlags)
					{
						typeIndex = i;
						return true;
					}
				}
				typeBits >>= 1;
			}
			// No memory types matched, return failure
			typeIndex = 0;
			return false;
		}

		bool CreateBuffer<TData>(MgBufferUsageFlagBits usage, UInt64 size, TData[] data, out IMgBuffer buffer, out IMgDeviceMemory memory) where TData : struct, ValueType
		{
			var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var result = CreateBuffer(usage, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, size, dataHandle.AddrOfPinnedObject(), out buffer, out memory);
			dataHandle.Free();
			return result;
		}

		bool CreateBuffer(MgBufferUsageFlagBits usage, UInt64 size, IntPtr data, out IMgBuffer buffer, out IMgDeviceMemory memory)
		{
			return CreateBuffer(usage, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, size, data, out buffer, out memory);
		}

		bool CreateBuffer(MgBufferUsageFlagBits usageFlags, MgMemoryPropertyFlagBits memoryPropertyFlags, UInt64 size, IntPtr data, out IMgBuffer buffer, out IMgDeviceMemory memory)
		{
			MgMemoryRequirements memReqs;
			MgMemoryAllocateInfo memAlloc = new MgMemoryAllocateInfo { AllocationSize = 0, MemoryTypeIndex = 0 };
			MgBufferCreateInfo bufferCreateInfo = new MgBufferCreateInfo{Usage = usageFlags, Size = size, Flags = 0 };

			checkResult(device.CreateBuffer(bufferCreateInfo, null, out buffer));

			device.GetBufferMemoryRequirements(buffer, out memReqs);
			memAlloc.AllocationSize = memReqs.Size;

			uint memoryTypeIndex;
			getMemoryType(memReqs.MemoryTypeBits, memoryPropertyFlags, out memoryTypeIndex);
			memAlloc.MemoryTypeIndex = memoryTypeIndex;

			checkResult(device.AllocateMemory(memAlloc, null, out memory));
			if (data != IntPtr.Zero)
			{
				IntPtr mapped;
				checkResult(memory.MapMemory(device, 0, size, 0, out mapped));
				//memcpy(mapped, data, size);
				memory.UnmapMemory(device);
			}
			checkResult(buffer.BindBufferMemory(device, memory, 0));

			return true;
		}

		Result checkResult(Result result)
		{
			if (result != Result.SUCCESS)
			{
				string errorMsg = "Fatal : VkResult returned " + result + "!";
				Console.WriteLine (errorMsg);
				Debug.Assert(result == Result.SUCCESS);
			}
			return result;
		}
	}
}

