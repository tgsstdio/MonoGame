using Microsoft.Xna.Framework;
using Magnesium;
using MonoGame.Core;
using MonoGame.Content;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

namespace HelloMagnesium
{
	public class HelloMagnesiumGame : Game
	{
		private IMgThreadPartition mPartition;
		private readonly ITexture2DLoader mTex2D;
		public HelloMagnesiumGame(
			IGraphicsDeviceManager manager,
			IMgThreadPartition partition,
			ITexture2DLoader tex2DLoader
		)
		{
			mTex2D = tex2DLoader;
			mPartition = partition;

			// Create device 

			// initialise viewport 

			// initialise scissor

			// Create effect / pass / sub pass / pipeline tree
		}

		static void CopyIntArray (IntPtr dest, int[] data, int elementCount, int srcStartIndex)
		{
			int length = sizeof(int) * elementCount;
			Marshal.Copy (data, srcStartIndex, dest, length);
		}

		class VkBuffer
		{
			public IMgBuffer Buffer { get; set; }
			public IMgDeviceMemory DeviceMemory { get; set; }
		}

		static VkBuffer CreateBuffer<TData>(
			IMgThreadPartition partition,
			MgBufferUsageFlagBits usage,
			uint bufferSize,
			TData[] data,
			int elementCount
		)
		{
			MgMemoryPropertyFlagBits memoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT;

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

			if (data != null)
			{
				IntPtr dest;
				result = deviceMemory.MapMemory(device, 0, bufferSize, 0, out dest);
				Debug.Assert (result == Result.SUCCESS);

				var structType = typeof(TData);
				int srcStartIndex = 0;

//				if (structType == typeof(int))
//				{
//					CopyIntArray (dest, data, elementCount, srcStartIndex);					
//				}
//				else if (structType == typeof(long))
//				{
//					int length = sizeof (long) * elementCount;						
//					Marshal.Copy ((long[]) data, srcStartIndex, dest, length);
//				}						
//				else if (structType == typeof(byte))
//				{
//					int length = sizeof (byte) * elementCount;
//					Marshal.Copy ((byte[]) data, srcStartIndex, dest, length);
//				}	
//				else if (structType == typeof(short))
//				{
//					int length = sizeof (short) * elementCount;					
//					Marshal.Copy ((short[]) data, srcStartIndex, dest, length);
//				}
//				else if (structType == typeof(float))
//				{
//					int length = sizeof (float) * elementCount;						
//					Marshal.Copy ((float[]) data, srcStartIndex, dest, length);
//				}
//				else if (structType == typeof(double))
//				{
//					int length = sizeof (double) * elementCount;						
//					Marshal.Copy ((double[]) data, srcStartIndex, dest, length);
//				}
//				else 
//				{
//					// TData stuff
//
//					for (int i = srcStartIndex; i < elementCount; ++i)
//					{
//						
//					}
//
//					throw new NotSupportedException ();
//				}			
					
				deviceMemory.UnmapMemory(device);
			}
			buffer.BindBufferMemory(device, deviceMemory, 0);

			return new VkBuffer {
				Buffer = buffer,
				DeviceMemory = deviceMemory,
			};
		}

		private ITexture2D mBackground;
		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent()
		{
			mBackground = mTex2D.Load (new AssetIdentifier {AssetId = 0x80000001});

			// create vertex buffer set of quad
				// vertex buffer of vertices, tex2d
				// instance buffer 

			const uint NO_OF_VERTICES = 4;
			IMgBuffer vert_0 = null;
			MgBufferCreateInfo bufCreateInfo = new MgBufferCreateInfo {
				Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
				Size = (5 * sizeof(float)) * NO_OF_VERTICES
			};
			var result = mPartition.Device.CreateBuffer (bufCreateInfo, null, out vert_0);
			Debug.Assert (result == Result.SUCCESS);




			// create index buffer of quad

			// create command buffer for quad		

			// create descriptor set for 
				// background image
				// constant buffer SSBO

		}

		public override void Draw(GameTime gameTime)
		{


			// submit command buffer to queue
			base.Draw (gameTime);	
		}
	}
}
