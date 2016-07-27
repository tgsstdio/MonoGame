using Microsoft.Xna.Framework;
using Magnesium;
using MonoGame.Content;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace HelloMagnesium
{
	public class HelloMagnesiumGame : Game
	{
		private readonly IMgThreadPartition mPartition;
		private readonly IMgBaseTextureLoader mTex2D;

		private IPresentationParameters mPresentation;
		private IContentStreamer mContent;

		private readonly GraphicsBank Bank = new GraphicsBank ();

		readonly IMgPresentationLayer mPresentationLayer;

		IMgGraphicsDeviceManager mManager;

		public HelloMagnesiumGame(
			IMgGraphicsDeviceManager manager,
			IMgThreadPartition partition,
			IMgBaseTextureLoader tex2DLoader,
			IPresentationParameters presentation,
			IMgSwapchainCollection swapChain,
			IMgPresentationLayer presentationLayer,
			IContentStreamer content
		)
		{
			mTex2D = tex2DLoader;
			mPartition = partition;
			mManager = manager;
			mPresentation = presentation;
			mContent = content;
			mPresentationLayer = presentationLayer;

			// Create device
			//manager.CreateDevice();
//
//			Bank.Width = (uint)mPresentation.BackBufferWidth;
//			Bank.Height = (uint)mPresentation.BackBufferHeight;
//
			const int NO_OF_BUFFERS = 2;
			IMgCommandBuffer[] buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
			MgCommandBufferAllocateInfo pAllocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_BUFFERS,
				CommandPool = mPartition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY,
			};

			mPartition.Device.AllocateCommandBuffers (pAllocateInfo, buffers);

			Bank.PrePresentBarrierCmd = buffers [0];
			Bank.PostPresentBarrierCmd = buffers [1];
//
//			var dsCreateInfo = new MgGraphicsDeviceCreateInfo
//			{
//				Command = buffers[2],
//				Color = MgFormat.R8G8B8A8_UINT,
//				DepthStencil = MgFormat.D24_UNORM_S8_UINT,
//				Width = Bank.Width,
//				Height = Bank.Height,
//				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
//				Swapchains = swapChain,
//			};
			mManager.CreateDevice();

			// Create synchronization objects

			// Create a semaphore used to synchronize image presentation
			// Ensures that the image is displayed before we start submitting new commands to the queu
			IMgSemaphore presentComplete;
			var err = mPartition.Device.CreateSemaphore(new MgSemaphoreCreateInfo(), null, out presentComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.PresentComplete = presentComplete;

			// Create a semaphore used to synchronize command submission
			// Ensures that the image is not presented until all commands have been sumbitted and executed
			IMgSemaphore renderComplete;
			err = mPartition.Device.CreateSemaphore(new MgSemaphoreCreateInfo(), null, out renderComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.RenderComplete = renderComplete;

			batch = new MgSpriteBatch (mPartition, content);
			batch.GenerateEffectPipeline (mManager.Device);
		}

		MgSpriteBatch batch;

		private MgBaseTexture mBackground;
		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent()
		{
			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "LoadContent (BEFORE) : " + error);
			}

			mBackground = mTex2D.Load (new AssetIdentifier { AssetId = 0x80000001 });

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "mTex2D.Load (AFTER) : " + error);
			}

			// create vertex buffer set of quad
			// vertex buffer of vertices, tex2d
			// instance buffer 

			const uint NO_OF_VERTICES = 4;

			var vertexBuf = new VkBuffer (mPartition, MgBufferUsageFlagBits.VERTEX_BUFFER_BIT, (5 * sizeof(float)) * NO_OF_VERTICES);

			float[] vertexData = {
				-1f, -1f, 0,
				1.0f, 1.0f,

				0.9f, -1f, 0,
				0.9f, 0.9f,

				1f, 1f, 0,
				0.9f, 0.9f,

				-1f, 0.9f, 0,
				0.9f, 0.9f,
			};

			vertexBuf.SetData(mPartition.Device, vertexBuf.BufferSize, vertexData, 0, vertexData.Length);


			// create index buffer of quad
			const int NO_OF_INDICES = 6;
			var indexBuf = new VkBuffer (mPartition, MgBufferUsageFlagBits.INDEX_BUFFER_BIT, sizeof(Int32) * NO_OF_INDICES);

			uint[] elementData = {
				0, 1, 2,
				2, 3, 0,
			};

			indexBuf.SetData(mPartition.Device, indexBuf.BufferSize, elementData, 0, elementData.Length);

			if (mManager.Device == null)
				return;

			// create command buffer for quad

			uint NO_OF_FRAMEBUFFERS = (uint) mManager.Device.Framebuffers.Length;

			var pCommandBuffers = new IMgCommandBuffer[NO_OF_FRAMEBUFFERS];
			var allocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_FRAMEBUFFERS,
				CommandPool = mPartition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY
			};
			var result = mPartition.Device.AllocateCommandBuffers (allocateInfo, pCommandBuffers);
			Debug.Assert (result == Result.SUCCESS);
			Bank.CommandBuffers = pCommandBuffers;

			Bank.Renderer = new GraphRenderer ();

			var frameInstances = new List<SubmitInfoGraphNode> ();

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
					Framebuffer = mManager.Device.Framebuffers[i],
					RenderPass = mManager.Device.Renderpass,
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
					
				cmdBuf.CmdBindPipeline (MgPipelineBindPoint.GRAPHICS, batch.GraphicsPipelines [0]);

				cmdBuf.CmdBindVertexBuffers (0, new []{ vertexBuf.Buffer }, new ulong[]{ 0 });
				cmdBuf.CmdBindIndexBuffer (indexBuf.Buffer, 0, MgIndexType.UINT32);
				cmdBuf.CmdDrawIndexed (6, 1, 0, 0, 0);

				cmdBuf.CmdEndRenderPass ();

				cmdBuf.EndCommandBuffer ();

				var node = new SubmitInfoGraphNode {
					Submit = new MgSubmitInfo
					{
						CommandBuffers = new [] {cmdBuf},
						SignalSemaphores = new [] {Bank.RenderComplete},
					},
					Fence = null,
				};
				frameInstances.Add (node);
			}

			var graphNode = new PrecompiledGraphNode (frameInstances.ToArray());
			Bank.Renderer.Renderables.Add (graphNode);

			// create descriptor set for 
				// background image
				// constant buffer SSBO

			IMgDescriptorSet[] descriptorSets;
			var dsAllocateInfo = new MgDescriptorSetAllocateInfo {
				DescriptorPool = mPartition.DescriptorPool,
				SetLayouts = new []
				{
					batch.DescriptorSetLayout,
				},
			};
			result = mPartition.Device.AllocateDescriptorSets (dsAllocateInfo, out descriptorSets);
			Debug.Assert (result == Result.SUCCESS);

			var writes = new MgWriteDescriptorSet [] {
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
//				new MgWriteDescriptorSet
//				{
//					DstSet = descriptorSets[0],
//					DescriptorType = MgDescriptorType.SAMPLED_IMAGE,
//					DstBinding = 0,
//					DescriptorCount = 1,
//					ImageInfo = new []
//					{
//						new MgDescriptorImageInfo
//						{
//							Sampler = mBackground.Sampler,
//							ImageView = mBackground.View,
//							ImageLayout = MgImageLayout.GENERAL,
//						},
//					},
//				}
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
			uint frameIndex = mPresentationLayer.BeginDraw (Bank.PostPresentBarrierCmd, Bank.PresentComplete);

			Bank.Renderer.Render (mPartition.Queue, gameTime, frameIndex);

			mPresentationLayer.EndDraw (frameIndex, Bank.PrePresentBarrierCmd, null);			
			// submit command buffer to queue
			base.Draw (gameTime);	
		}
	}
}
