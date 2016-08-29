using Microsoft.Xna.Framework;
using Magnesium;
using MonoGame.Content;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using Magnesium.OpenGL.DesktopGL;

namespace HelloMagnesium
{
	public class HelloMagnesiumGame : Game, IDisposable
	{
		private readonly IMgThreadPartition mPartition;
		private readonly IMgBaseTextureLoader mTex2D;

		private IPresentationParameters mPresentation;
		private IContentStreamer mContent;

		private readonly GraphicsBank Bank = new GraphicsBank ();

		readonly IMgPresentationLayer mPresentationLayer;

		IGraphicsDeviceManager mManager;

		#region IDisposable implementation

		~HelloMagnesiumGame()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources ()
		{
			if (mPartition != null)
				Bank.Destroy (mPartition);
		}

		void ReleaseManagedResources ()
		{

		}

		bool mDisposing = false;
		public void Dispose (bool disposing)
		{
			if (mDisposing)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}

			mDisposing = true;
		}

		#endregion

		readonly IMgSwapchainCollection mSwapchain;

		IOpenTKSwapchainKHR mInternalChain;

		public HelloMagnesiumGame(
			IGraphicsDeviceManager manager,
			IMgThreadPartition partition,
			IMgBaseTextureLoader tex2DLoader,
			IPresentationParameters presentation,
			IMgSwapchainCollection swapChain,
			IMgPresentationLayer presentationLayer,
			IContentStreamer content,
			IOpenTKSwapchainKHR internalChain
		)
		{
			mTex2D = tex2DLoader;
			mPartition = partition;
			mManager = manager;
			mPresentation = presentation;
			mContent = content;
			mPresentationLayer = presentationLayer;
			mSwapchain = swapChain;
			mInternalChain = internalChain;

			// Create device
			mManager.CreateDevice();
//
//			Bank.Width = (uint)mPresentation.BackBufferWidth;
//			Bank.Height = (uint)mPresentation.BackBufferHeight;
//
			const int NO_OF_BUFFERS = 2;
			var buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
			var pAllocateInfo = new MgCommandBufferAllocateInfo {
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
			//mManager.CreateDevice();

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

			var vertexBuf = new VkBuffer (mPartition, MgBufferUsageFlagBits.VERTEX_BUFFER_BIT, (4 * sizeof(float)) * NO_OF_VERTICES);

			float[] vertexData = {
				-1f, -1f,
				1.0f, 1.0f,

				0.9f, -1f,
				0.9f, 0.9f,

				1f, 1f,
				0.9f, 0.9f,

				-1f, 0.9f, 
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
							Width = mSwapchain.Width,
							Height = mSwapchain.Height,
						},
						Offset = new MgOffset2D {
							X = 0,
							Y = 0,
						}
					},
					ClearValues = new [] {
						new MgClearValue {
							Color = new MgClearColorValue (uint.MaxValue, 0, uint.MaxValue, uint.MaxValue),
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

		public override void Draw(GameTime gameTime)
		{			
			UseRendergraph (gameTime);

			//ExplicitSwapbuffers ();

			// submit command buffer to queue
			base.Draw (gameTime);	
		}

		void UseRendergraph (GameTime gameTime)
		{
			uint frameIndex = mPresentationLayer.BeginDraw (Bank.PostPresentBarrierCmd, Bank.PresentComplete);
			//GL.ColorMask (true, true, true, true);
			Bank.Renderer.Render (mPartition.Queue, gameTime, frameIndex);
			// should use semaphores instead
			mPartition.Queue.QueueWaitIdle ();

			mPresentationLayer.EndDraw (frameIndex, Bank.PrePresentBarrierCmd, null);
		}

		void ExplicitSwapbuffers ()
		{
			GL.ClearColor (0f, 0f, 0f, 0f);
			GL.Viewport (0, 0, (int)mSwapchain.Width, (int)mSwapchain.Height);
			GL.ColorMask (true, false, true, true);
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Begin(OpenTK.Graphics.OpenGL.PrimitiveType.Quads);
				GL.Color3(1.0f, 1.0f, 1.0);
				GL.Vertex2(-1f, -1f);
				GL.Color3(0.9f, 0.9f, 0.9f);
				GL.Vertex2(-1f, 1f);
				GL.Color3(0.9f, 0.9f, 0.9f);
				GL.Vertex2(1f, 1f);
				GL.Color3(0.9f, 0.9f, 0.9f);
				GL.Vertex2(1f, -1f);
			GL.End();

			mInternalChain.SwapBuffers();
		}
	}
}
