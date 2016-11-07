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
	public class HelloMagnesiumGame : Game
	{
		private readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		private readonly IMgBaseTextureLoader mTex2D;

		private IPresentationParameters mPresentation;
		private IShaderContentStreamer mContent;

		private readonly GraphicsBank Bank = new GraphicsBank ();

		readonly IMgPresentationLayer mPresentationLayer;

		IGraphicsDeviceManager mManager;

		#region IDisposable implementation

		protected override void ReleaseUnmanagedResources ()
		{
            if (mGraphicsConfiguration != null)
            {
                Bank.Destroy(mGraphicsConfiguration.Partition);
                batch.Dispose();
            }
		}

        protected override void ReleaseManagedResources ()
		{

		}

		#endregion

		readonly IMgSwapchainCollection mSwapchain;


		public HelloMagnesiumGame(
			IGraphicsDeviceManager manager,
            MgDriverContext driverContext,
			IMgGraphicsConfiguration graphicsConfiguration,
			IMgBaseTextureLoader tex2DLoader,
			IPresentationParameters presentation,
			IMgSwapchainCollection swapChain,
			IMgPresentationLayer presentationLayer,
			IShaderContentStreamer content
		)
		{
            mDriverContext = driverContext;
			mTex2D = tex2DLoader;
			mGraphicsConfiguration = graphicsConfiguration;
			mManager = manager;
			mPresentation = presentation;
			mContent = content;
			mPresentationLayer = presentationLayer;
			mSwapchain = swapChain;

            var errorCode = mDriverContext.Initialize(
                new MgApplicationInfo
                {
                    ApplicationName = "HelloMagnesium",
                    EngineName = "Magnesium",
                    ApplicationVersion = 1,
                    EngineVersion = 1,
                    ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                },
                  MgInstanceExtensionOptions.ALL
             );

            if (errorCode != Result.SUCCESS)
            {
                throw new InvalidOperationException("mDriverContext error : " + errorCode);
            }

            var width = (uint) mPresentation.BackBufferWidth;
            var height = (uint) mPresentation.BackBufferHeight;

            mGraphicsConfiguration.Initialize(width, height);

            // Create device

            var dsCreateInfo = new MgGraphicsDeviceCreateInfo
            {
                Color = MgFormat.R8G8B8A8_UINT,
                DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = width,
                Height = height,
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
            };
            mManager.CreateDevice(dsCreateInfo);

            Debug.Assert(mGraphicsConfiguration.Partition != null);

			const int NO_OF_BUFFERS = 2;
			var buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
			var pAllocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_BUFFERS,
				CommandPool = mGraphicsConfiguration.Partition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY,
			};

			mGraphicsConfiguration.Device.AllocateCommandBuffers (pAllocateInfo, buffers);

			Bank.PrePresentBarrierCmd = buffers [0];
			Bank.PostPresentBarrierCmd = buffers [1];

			// Create synchronization objects

			// Create a semaphore used to synchronize image presentation
			// Ensures that the image is displayed before we start submitting new commands to the queu
			IMgSemaphore presentComplete;
			var err = mGraphicsConfiguration.Device.CreateSemaphore(new MgSemaphoreCreateInfo(), null, out presentComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.PresentComplete = presentComplete;

			// Create a semaphore used to synchronize command submission
			// Ensures that the image is not presented until all commands have been sumbitted and executed
			IMgSemaphore renderComplete;
			err = mGraphicsConfiguration.Device.CreateSemaphore(new MgSemaphoreCreateInfo(), null, out renderComplete);
			Debug.Assert(err == Result.SUCCESS);
			Bank.RenderComplete = renderComplete;

			batch = new MgSpriteBatch (mGraphicsConfiguration.Partition, content);
			batch.GenerateEffectPipeline (mManager.Device);
		}

		MgSpriteBatch batch;

		private MgBaseTexture mBackground;
        private MgDriverContext mDriverContext;


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

            Debug.Assert(mGraphicsConfiguration.Partition != null);
            var vertexBuf = new VkBuffer (mGraphicsConfiguration.Partition, MgBufferUsageFlagBits.VERTEX_BUFFER_BIT, (4 * sizeof(float)) * NO_OF_VERTICES);

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

			vertexBuf.SetData(mGraphicsConfiguration.Device, vertexBuf.BufferSize, vertexData, 0, vertexData.Length);


			// create index buffer of quad
			const int NO_OF_INDICES = 6;
			var indexBuf = new VkBuffer (mGraphicsConfiguration.Partition, MgBufferUsageFlagBits.INDEX_BUFFER_BIT, sizeof(Int32) * NO_OF_INDICES);

			uint[] elementData = {
				0, 1, 2,
				2, 3, 0,
			};

			indexBuf.SetData(mGraphicsConfiguration.Device, indexBuf.BufferSize, elementData, 0, elementData.Length);

			if (mManager.Device == null)
				return;

			// create command buffer for quad

			uint NO_OF_FRAMEBUFFERS = (uint) mManager.Device.Framebuffers.Length;

			var pCommandBuffers = new IMgCommandBuffer[NO_OF_FRAMEBUFFERS];
			var allocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_FRAMEBUFFERS,
				CommandPool = mGraphicsConfiguration.Partition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY
			};
			var result = mGraphicsConfiguration.Device.AllocateCommandBuffers (allocateInfo, pCommandBuffers);
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
                    ClearValues = new[] {
                        MgClearValue.FromColorAndFormat(mSwapchain.Format, new MgColor4f(1f, 0, 1f, 1f)),
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

				var err = cmdBuf.EndCommandBuffer ();
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");

				var node = new SubmitInfoGraphNode {
					Submit = new MgSubmitInfo
					{
                        WaitSemaphores = new MgSubmitInfoWaitSemaphoreInfo[]
                        {
                            new MgSubmitInfoWaitSemaphoreInfo
                            {
                                WaitSemaphore = Bank.PresentComplete,
                                WaitDstStageMask = MgPipelineStageFlagBits.ALL_GRAPHICS_BIT,
                            },
                        },
						CommandBuffers = new [] {cmdBuf},
						SignalSemaphores = new [] {Bank.RenderComplete},
					},
					Fence = null,
				};
				frameInstances.Add (node);
			}

			var graphNode = new PrecompiledGraphNode (frameInstances.ToArray());
			Bank.Renderer.Renderables.Add (graphNode);
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
			Bank.Renderer.Render (mGraphicsConfiguration.Queue, gameTime, frameIndex);
			// should use semaphores instead
			mGraphicsConfiguration.Queue.QueueWaitIdle ();

			mPresentationLayer.EndDraw (new uint[] { frameIndex }, Bank.PrePresentBarrierCmd, new[] { Bank.RenderComplete });
		}

		//void ExplicitSwapbuffers ()
		//{ ONLY FOR OPENGL
		//	GL.ClearColor (0f, 0f, 0f, 0f);
		//	GL.Viewport (0, 0, (int)mSwapchain.Width, (int)mSwapchain.Height);
		//	GL.ColorMask (true, false, true, true);
		//	GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		//	GL.Begin(OpenTK.Graphics.OpenGL.PrimitiveType.Quads);
		//		GL.Color3(1.0f, 1.0f, 1.0);
		//		GL.Vertex2(-1f, -1f);
		//		GL.Color3(0.9f, 0.9f, 0.9f);
		//		GL.Vertex2(-1f, 1f);
		//		GL.Color3(0.9f, 0.9f, 0.9f);
		//		GL.Vertex2(1f, 1f);
		//		GL.Color3(0.9f, 0.9f, 0.9f);
		//		GL.Vertex2(1f, -1f);
		//	GL.End();

		//	mInternalChain.SwapBuffers();
		//}
	}
}
