using Microsoft.Xna.Framework;
using Magnesium;
using MonoGame.Core;
using MonoGame.Content;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Graphics;

namespace HelloMagnesium
{
	public class HelloMagnesiumGame : Game
	{
		private IMgThreadPartition mPartition;
		private readonly IMgBaseTextureLoader mTex2D;

		private IMgDepthStencilBuffer mDepthStencil;
		private IPresentationParameters mPresentation;
		private IContentStreamer mContent;

		private class GraphicsBank
		{
			public IMgSemaphore RenderComplete {
				get;
				set;
			}

			public IMgSemaphore PresentComplete {
				get;
				set;
			}

			public IMgCommandBuffer[] CommandBuffers {
				get;
				set;
			}

			public IMgPipeline[] GraphicsPipelines {
				get;
				set;
			}

			public IMgPipelineLayout PipelineLayout {
				get;
				set;
			}

			public IMgDescriptorSetLayout SetLayout {
				get;
				set;
			}

			public uint Height {
				get;
				set;
			}

			public uint Width {
				get;
				set;
			}

			public MgRect2D CurrentScissor {
				get;
				set;
			}

			public MgViewport CurrentViewport {
				get;
				set;
			}

			public IMgFramebuffer[] FrameBuffers {
				get;
				set;
			}

			public IMgRenderPass RenderPass {
				get;
				set;
			}
		}

		private GraphicsBank Bank = new GraphicsBank ();

		IMgPresentationSurface mPresentationLayer;

		public HelloMagnesiumGame(
			IGraphicsDeviceManager manager,
			IMgThreadPartition partition,
			IMgBaseTextureLoader tex2DLoader,
			IMgDepthStencilBuffer depthStencil,
			IPresentationParameters presentation,
			IMgSwapchainCollection swapChain,
			IMgPresentationSurface presentationLayer,
			IContentStreamer content
		)
		{
			mTex2D = tex2DLoader;
			mPartition = partition;
			mDepthStencil = depthStencil;
			mPresentation = presentation;
			mContent = content;
			mPresentationLayer = presentationLayer;

			// Create device
			//manager.CreateDevice();

			Bank.Width = (uint)mPresentation.BackBufferWidth;
			Bank.Height = (uint)mPresentation.BackBufferHeight;

			// Create depthstencil
			MgFormat colorFormat = MgFormat.R8G8B8A8_UINT;
			MgFormat depthFormat = MgFormat.D24_UNORM_S8_UINT;

			var dsCreateInfo = new MgDepthStencilBufferCreateInfo
			{
				Format = depthFormat,
				Width = Bank.Width,
				Height = Bank.Height,
				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
			};
			mDepthStencil.Create(dsCreateInfo);

			// Create renderpass 
			Bank.RenderPass = SetupRenderPass(mPartition.Device, colorFormat, depthFormat);
			// Create framebuffer
			Bank.FrameBuffers = SetupFrameBuffers(mPartition.Device, Bank.RenderPass, depthStencil, swapChain, Bank.Width, Bank.Height);
			// initialise viewport
			Bank.CurrentViewport = new MgViewport {
				Width = Bank.Width,
				Height = Bank.Height,
				X = 0,
				Y = 0,
				MinDepth = 0f,
				MaxDepth = 1f,
			};

			// initialise scissor
			Bank.CurrentScissor = new MgRect2D {
				Extent = new MgExtent2D{Width = Bank.Width, Height = Bank.Height},
				Offset = new MgOffset2D{X = 0, Y = 0},
			};

			// Create synchronization objects
			MgSemaphoreCreateInfo semaphoreCreateInfo = new MgSemaphoreCreateInfo {

			};

			// Create a semaphore used to synchronize image presentation
			// Ensures that the image is displayed before we start submitting new commands to the queu
			IMgSemaphore presentComplete;
			var err = mPartition.Device.CreateSemaphore(semaphoreCreateInfo, null, out presentComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.PresentComplete = presentComplete;

			// Create a semaphore used to synchronize command submission
			// Ensures that the image is not presented until all commands have been sumbitted and executed
			IMgSemaphore renderComplete;
			err = mPartition.Device.CreateSemaphore(semaphoreCreateInfo, null, out renderComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.RenderComplete = renderComplete;

			GenerateEffectPipeline ();
		}

		void GenerateEffectPipeline ()
		{
			// Create effect / pass / sub pass / pipeline tree
			using (var vs = mContent.LoadContent (new AssetIdentifier {AssetId = 0x80000002}, new[] {".vs"}))
			using (var fs = mContent.LoadContent (new AssetIdentifier {AssetId = 0x80000003}, new[] {".fs"}))
			{
				IMgShaderModule vertSM;
				var vertCreateInfo = new MgShaderModuleCreateInfo {
					Code = vs,
					CodeSize = new UIntPtr ((ulong)vs.Length),
				};
				mPartition.Device.CreateShaderModule (vertCreateInfo, null, out vertSM);
				IMgShaderModule fragSM;
				var fragCreateInfo = new MgShaderModuleCreateInfo {
					Code = fs,
					CodeSize = new UIntPtr ((ulong)fs.Length),
				};
				mPartition.Device.CreateShaderModule (fragCreateInfo, null, out fragSM);
				MgDescriptorSetLayoutCreateInfo dslCreateInfo = new MgDescriptorSetLayoutCreateInfo {
					Bindings = new[] {
						//						new MgDescriptorSetLayoutBinding
						//						{
						//							Binding = 0,
						//							DescriptorType = MgDescriptorType.STORAGE_BUFFER,
						//							StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
						//							DescriptorCount = 1,
						//						},
						new MgDescriptorSetLayoutBinding {
							Binding = 0,
							DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
							StageFlags = MgShaderStageFlagBits.FRAGMENT_BIT,
							DescriptorCount = 1,
						},
					},
				};
				IMgDescriptorSetLayout setLayout;
				mPartition.Device.CreateDescriptorSetLayout (dslCreateInfo, null, out setLayout);
				Bank.SetLayout = setLayout;
				var pLayoutCreateInfo = new MgPipelineLayoutCreateInfo {
					SetLayouts = new[] {
						setLayout
					},
				};
				IMgPipelineLayout layout;
				mPartition.Device.CreatePipelineLayout (pLayoutCreateInfo, null, out layout);
				Bank.PipelineLayout = layout;
				var pipelineParameters = new[] {
					new MgGraphicsPipelineCreateInfo {
						RenderPass = Bank.RenderPass,
						DepthStencilState = new MgPipelineDepthStencilStateCreateInfo {
							Front = new MgStencilOpState {
								WriteMask = ~0U,
								Reference = ~0U,
								CompareMask = int.MaxValue,
								CompareOp = MgCompareOp.ALWAYS,
								PassOp = MgStencilOp.KEEP,
								FailOp = MgStencilOp.KEEP,
								DepthFailOp = MgStencilOp.KEEP,
							},
							Back = new MgStencilOpState {
								WriteMask = ~0U,
								Reference = ~0U,
								CompareMask = int.MaxValue,
								CompareOp = MgCompareOp.ALWAYS,
								PassOp = MgStencilOp.KEEP,
								FailOp = MgStencilOp.KEEP,
								DepthFailOp = MgStencilOp.KEEP,
							},
							StencilTestEnable = false,
							DepthTestEnable = true,
							DepthWriteEnable = true,
							DepthCompareOp = MgCompareOp.LESS,
						},
						Stages = new[] {
							new MgPipelineShaderStageCreateInfo {
								Module = vertSM,
								Name = "main",
								Stage = MgShaderStageFlagBits.VERTEX_BIT,
							},
							new MgPipelineShaderStageCreateInfo {
								Module = fragSM,
								Name = "main",
								Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
							},
						},
						InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo {
							Topology = MgPrimitiveTopology.TRIANGLE_LIST,
						},
						Layout = layout,
						MultisampleState = new MgPipelineMultisampleStateCreateInfo {
							RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
						},
						RasterizationState = new MgPipelineRasterizationStateCreateInfo {
							PolygonMode = MgPolygonMode.FILL,
							CullMode = MgCullModeFlagBits.NONE,
							FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
							Flags = 0,
							DepthClampEnable = true,
							LineWidth = 1f,
						},
						VertexInputState = new MgPipelineVertexInputStateCreateInfo {
							VertexBindingDescriptions = new[] {
								new MgVertexInputBindingDescription {
									Binding = 0,
									Stride = sizeof(float) * 5,
									InputRate = MgVertexInputRate.VERTEX
								},
							},
							VertexAttributeDescriptions = new[] {
								new MgVertexInputAttributeDescription {
									Binding = 0,
									Location = 0,
									Format = MgFormat.R32G32B32_SFLOAT,
									Offset = 0,
								},
								new MgVertexInputAttributeDescription {
									Binding = 0,
									Location = 1,
									Format = MgFormat.R32G32_SFLOAT,
									Offset = sizeof(float) * 3,
								},
							},
						},
						ViewportState = new MgPipelineViewportStateCreateInfo {
							Scissors = new[] {
								Bank.CurrentScissor
							},
							Viewports = new[] {
								Bank.CurrentViewport
							},
						},
					},
				};
				IMgPipeline[] graphicsPipelines;
				mPartition.Device.CreateGraphicsPipelines (null, pipelineParameters, null, out graphicsPipelines);
				Bank.GraphicsPipelines = graphicsPipelines;
				vertSM.DestroyShaderModule (mPartition.Device, null);
				fragSM.DestroyShaderModule (mPartition.Device, null);
			}
		}

		static IMgRenderPass SetupRenderPass(IMgDevice device, MgFormat colorformat, MgFormat depthFormat)
		{
			var attachments = new []
			{
				// Color attachment[0] 
				new MgAttachmentDescription{
					Format = colorformat,
					// TODO : multisampling
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					LoadOp =  MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
					InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				},
				// Depth attachment[1]
				new MgAttachmentDescription{
					Format = depthFormat,
					// TODO : multisampling
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,

					// TODO : activate stencil if needed
					StencilLoadOp =  MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp =  MgAttachmentStoreOp.DONT_CARE,

					InitialLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
				}
			};

			var colorReference = new MgAttachmentReference
			{
				Attachment = 0,
				Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
			};

			var depthReference = new MgAttachmentReference{
				Attachment = 1,
				Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
			};

			var subpass = new MgSubpassDescription
			{
				PipelineBindPoint = MgPipelineBindPoint.GRAPHICS,
				Flags = 0,
				InputAttachments = null,
				ColorAttachments = new []{colorReference},
				ResolveAttachments = null,
				DepthStencilAttachment = depthReference,
				PreserveAttachments = null,
			};

			var renderPassInfo = new MgRenderPassCreateInfo{
				Attachments = attachments,
				Subpasses = new []{subpass},
				Dependencies = null,
			};

			Result err;

			IMgRenderPass renderPass;
			err = device.CreateRenderPass(renderPassInfo, null, out renderPass);
			Debug.Assert(err == Result.SUCCESS);
			return renderPass;
		}

		static IMgFramebuffer[] SetupFrameBuffers(IMgDevice device, IMgRenderPass renderPass, IMgDepthStencilBuffer depthStencil, IMgSwapchainCollection swapChain, uint width, uint height)
		{
			// Create frame buffers for every swap chain image
			var frameBuffers = new IMgFramebuffer[swapChain.Buffers.Length];
			for (uint i = 0; i < frameBuffers.Length; i++)
			{
				var frameBufferCreateInfo = new MgFramebufferCreateInfo
				{
					RenderPass = renderPass,
					Attachments = new []
					{
						swapChain.Buffers[i].View,
						// Depth/Stencil attachment is the same for all frame buffers
						depthStencil.View,
					},
					Width = width,
					Height = height,
					Layers = 1,
				};

				var err = device.CreateFramebuffer(frameBufferCreateInfo, null, out frameBuffers[i]);
				Debug.Assert(err == Result.SUCCESS);
			}

			return frameBuffers;
		}

		static void CopyIntArray (IntPtr dest, int[] data, int elementCount, int srcStartIndex)
		{
			int length = sizeof(int) * elementCount;
			Marshal.Copy (data, srcStartIndex, dest, length);
		}

		class VkBuffer
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

			public void Destroy(IMgDevice device, MgAllocationCallbacks allocator)
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

		private MgBaseTexture mBackground;
		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent()
		{
			mBackground = mTex2D.Load (new AssetIdentifier { AssetId = 0x80000001 });

			// create vertex buffer set of quad
			// vertex buffer of vertices, tex2d
			// instance buffer 

			const uint NO_OF_VERTICES = 4;
			var vertexBuf = new VkBuffer (mPartition, MgBufferUsageFlagBits.VERTEX_BUFFER_BIT, (5 * sizeof(float)) * NO_OF_VERTICES);
			// TODO : COPY DATA HERE

			// create index buffer of quad
			const int NO_OF_INDICES = 6;
			var indexBuf = new VkBuffer (mPartition, MgBufferUsageFlagBits.INDEX_BUFFER_BIT, sizeof(Int32) * NO_OF_INDICES);
			// TODO : COPY DATA HERE

			// create command buffer for quad

			uint NO_OF_FRAMEBUFFERS = (uint)Bank.FrameBuffers.Length;

			var pCommandBuffers = new IMgCommandBuffer[NO_OF_FRAMEBUFFERS];
			var allocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_FRAMEBUFFERS,
				CommandPool = mPartition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY
			};
			var error = mPartition.Device.AllocateCommandBuffers (allocateInfo, pCommandBuffers);
			Debug.Assert (error == Result.SUCCESS);
			Bank.CommandBuffers = pCommandBuffers;

			// NEED TO CREATE A COMMANDBUFFER FOR EVERY FRAMEBUFFER SO THE RIGHT FRAMEBUFFER 
			// IS TARGETED FOR THE DRAW
			for (uint i = 0; i < NO_OF_FRAMEBUFFERS; ++i)
			{
				var cmdBuf = pCommandBuffers [i];

				var beginInfo = new MgCommandBufferBeginInfo {
					Flags = 0,
				};
				cmdBuf.BeginCommandBuffer (beginInfo);
				var passBeginInfo = new MgRenderPassBeginInfo {
					Framebuffer = Bank.FrameBuffers[i],
					RenderPass = Bank.RenderPass,
					RenderArea = new MgRect2D {
						// FIXME: specific screen width
						Extent = new MgExtent2D {
							Width = Bank.Width,
							Height = Bank.Height,
						},
						Offset = new MgOffset2D {
							X = 0,
							Y = 0,
						}
					},
					ClearValues = new [] {
						new MgClearValue {
							Color = new MgClearColorValue (1f, 1f, 1f, 1f),
						},
						new MgClearValue {
							DepthStencil = new MgClearDepthStencilValue (1f, 0),							
						}
					},
				};
				cmdBuf.CmdBeginRenderPass (passBeginInfo, MgSubpassContents.INLINE);
					
				cmdBuf.CmdBindPipeline (MgPipelineBindPoint.GRAPHICS, Bank.GraphicsPipelines [0]);

				cmdBuf.CmdBindVertexBuffers (0, new []{ vertexBuf.Buffer }, new ulong[]{ 0 });
				cmdBuf.CmdBindIndexBuffer (indexBuf.Buffer, 0, MgIndexType.UINT32);
				cmdBuf.CmdDrawIndexed (6, 1, 0, 0, 0);

				cmdBuf.CmdEndRenderPass ();

				cmdBuf.EndCommandBuffer ();
			}

			// create descriptor set for 
				// background image
				// constant buffer SSBO

			IMgDescriptorSet[] descriptorSets;
			var dsAllocateInfo = new MgDescriptorSetAllocateInfo {
				DescriptorPool = mPartition.DescriptorPool,
				SetLayouts = new []
				{
					Bank.SetLayout,
				},
			};
			error = mPartition.Device.AllocateDescriptorSets (dsAllocateInfo, out descriptorSets);
			Debug.Assert (error == Result.SUCCESS);

			var writes = new [] {
//				new MgWriteDescriptorSet
//				{
//					DstSet = descriptorSets[0],
//					DescriptorType = MgDescriptorType.STORAGE_BUFFER,
//					DstBinding = 0,
//					DescriptorCount = 1,
//					BufferInfo = new []
//					{
//						new MgDescriptorBufferInfo
//						{
//							Buffer = buffer,
//							Offset = 0,
//							Range = 0,
//						}
//					}
//				},
				new MgWriteDescriptorSet
				{
					DstSet = descriptorSets[0],
					DescriptorType = MgDescriptorType.SAMPLED_IMAGE,
					DstBinding = 0,
					DescriptorCount = 1,
					ImageInfo = new []
					{
						new MgDescriptorImageInfo
						{
							Sampler = mBackground.Sampler,
							ImageView = mBackground.View,
							ImageLayout = MgImageLayout.GENERAL,
						},
					},
				}
			};
			MgCopyDescriptorSet[] copies = null;
			mPartition.Device.UpdateDescriptorSets (writes, copies);	
		}

		static void Render (IMgThreadPartition partition, uint nextImage)
		{
			// Command buffer to be sumitted to the queue
			var submitInfo = new[] {
				new MgSubmitInfo {
					// DRAW HERE
					CommandBuffers = new IMgCommandBuffer[] {

					},
				}
			};
			//submitInfo.commandBufferCount = 1;
			//submitInfo.pCommandBuffers = &drawCmdBuffers[currentBuffer];
			// Submit to queue
			IMgQueue queue = partition.Queue;
			var err = queue.QueueSubmit (submitInfo, null);
			Debug.Assert (err == Result.SUCCESS);
		}

//		MgPresentationLayer present;
//		void draw2(IMgSemaphore presentComplete)
//		{
//			Result err;
//			var nextImage = present.BeginDraw (presentComplete);
//
//			//Render (nextImage);
//
//			IMgSemaphore renderComplete;
//			present.EndDraw (nextImage, renderComplete);
//		}


		public override void Draw(GameTime gameTime)
		{


			// submit command buffer to queue
			base.Draw (gameTime);	
		}
	}
}
