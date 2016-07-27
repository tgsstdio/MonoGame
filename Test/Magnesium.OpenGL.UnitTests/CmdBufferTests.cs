using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class CmdBufferTests
	{
		ICmdVBOCapabilities mFactory;
		ICmdBufferInstructionSetComposer mComposer;
		GLCmdBufferRepository mRepository;

		[SetUp]
		public void Initialise()
		{
			mFactory = new MockVertexBufferFactory ();
			mRepository = new GLCmdBufferRepository ();
			mComposer = new Transformer (mFactory, mRepository);
		}

		[TearDown]
		public void Cleanup()
		{
			mFactory = null;
			mComposer = null;
			mRepository = null;
		}


		[TestCase]
		public void NullPointer()
		{
			var output = mComposer.Compose (null, null);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (0, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (0, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);
		}

		[TestCase]
		public void EmptyRepository()
		{			
			var output = mComposer.Compose (mRepository, null);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (0, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (0, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);
		}


		[TestCase]
		public void SingleDrawItem()
		{			
			var pipeline = new MockGLGraphicsPipeline ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[] { };

			pipeline.VertexInput = new GLVertexBufferBinder(bindings, attributes);
			pipeline.Viewports = new GLCmdViewportParameter (0, new MgViewport[]{ });
			pipeline.Scissors = new GLCmdScissorParameter (0, new MgRect2D[]{ });

			mRepository.PushGraphicsPipeline (pipeline);

			mRepository.VertexBuffers.Add (
				new GLCmdVertexBufferParameter{					
					pBuffers = new IMgBuffer[]{},
				}
			);

			var arrayDraw = new GLCmdInternalDraw {
				firstInstance = 3,
				firstVertex = 5,
				instanceCount = 7,
				vertexCount = 11,
			};

			var drawCommands = new List<GLCmdDrawCommand> ();

			drawCommands.Add (
				new GLCmdDrawCommand{
					Pipeline = 0,
					VertexBuffer = 0,
					Draw = arrayDraw,
				}
			);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand {
				Origin = origin,
				DrawCommands = drawCommands,
			};

			var passes = new []{ pass };
			CmdBufferInstructionSet output = mComposer.Compose (mRepository, passes);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (1, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (1, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);

			var actual = output.DrawItems[0];
			Assert.AreEqual(arrayDraw.firstInstance, actual.FirstInstance);
			Assert.AreEqual(arrayDraw.firstVertex, actual.First);
			Assert.AreEqual(arrayDraw.instanceCount, actual.InstanceCount);
			Assert.AreEqual(arrayDraw.vertexCount, actual.Count);
			// UNUSED VALUES
			Assert.AreEqual(IntPtr.Zero, actual.Buffer);
			Assert.AreEqual((GLCommandBufferFlagBits) 0, actual.Command);
			Assert.AreEqual(0, actual.Stride);
			Assert.AreEqual(0, actual.Offset);
			Assert.AreEqual(0, actual.VertexOffset);
		}

		[TestCase]
		public void SingleDrawIndexed()
		{			
			var info = new MgGraphicsPipelineCreateInfo {				
				VertexInputState = new MgPipelineVertexInputStateCreateInfo
				{
					VertexBindingDescriptions = new MgVertexInputBindingDescription[]{ },
					VertexAttributeDescriptions = new MgVertexInputAttributeDescription[]{ },
				},
				InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
				{
					Topology = MgPrimitiveTopology.TRIANGLE_LIST,
				},
				RasterizationState = new MgPipelineRasterizationStateCreateInfo
				{
					CullMode = MgCullModeFlagBits.BACK_BIT,
					DepthBiasClamp = 1f,
					DepthBiasConstantFactor = 1f,
					DepthBiasEnable = true,
					DepthBiasSlopeFactor = 1f,
					DepthClampEnable = true,
					FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
					LineWidth = 1f,
					RasterizerDiscardEnable = false,
				}
			};

			var pipeline = new GLGraphicsPipeline (0, info, null);

//			var bindings = new GLVertexBufferBinding[]{ };
//			var attributes = new GLVertexInputAttribute[] { };
//
//			pipeline.VertexInput = new GLVertexBufferBinder(bindings, attributes);
//
//			var viewport = new MgViewport {
//				X = 100,
//				Y = 200, 
//				Width = 400,
//				Height = 600,
//				MinDepth = 0.5f,
//				MaxDepth = 1.5f
//			};
//
//			pipeline.Viewports = new GLCmdViewportParameter (0, new [] { viewport });
//
//			pipeline.StencilState = new GLQueueStencilState {
//				BackDepthBufferFail = MgStencilOp.DECREMENT_AND_CLAMP,
//				BackStencilFail = MgStencilOp.DECREMENT_AND_CLAMP,
//				BackStencilFunction = MgCompareOp.ALWAYS,
//				BackStencilPass = MgStencilOp.DECREMENT_AND_CLAMP,
//
//				FrontDepthBufferFail = MgStencilOp.DECREMENT_AND_CLAMP,
//				FrontStencilFail = MgStencilOp.DECREMENT_AND_CLAMP,
//				FrontStencilFunction = MgCompareOp.ALWAYS,
//				FrontStencilPass = MgStencilOp.DECREMENT_AND_CLAMP,
//			};					
//
//			pipeline.Front = new GLQueueStencilMasks {
//				CompareMask = 200,
//				Reference = 300,
//				WriteMask = 100,
//			};
//
//			pipeline.Back = new GLQueueStencilMasks {
//				CompareMask = 1200,
//				Reference = 1300,
//				WriteMask = 1100,
//			};

			mRepository.PushGraphicsPipeline (pipeline);

			mRepository.VertexBuffers.Add (
				new GLCmdVertexBufferParameter{					
					pBuffers = new IMgBuffer[]{},
				}
			);

			var indexedDraw = new GLCmdInternalDrawIndexed {
				firstInstance = 3,
				firstIndex = 9,
				indexCount = 5,
				instanceCount = 7,
				vertexOffset = 11,
			};

			var drawCommands = new List<GLCmdDrawCommand> ();

			drawCommands.Add (
				new GLCmdDrawCommand{
					Pipeline = 0,
					VertexBuffer = 0,
					DrawIndexed = indexedDraw,
					Viewports = 0,
					Scissors = 0,

					DescriptorSet = null,

					BackReference = 0,
					BackWriteMask = 0,
					BackCompareMask = 0,

					FrontReference = 0,
					FrontWriteMask = 0,
					FrontCompareMask = 0,

					DepthBias = 0,
					DepthBounds = 0,

					LineWidth = 0,
					BlendConstants = 0,
				}
			);

			var origin = new MockIGLRenderPass (); 
			var pass = new GLCmdRenderPassCommand {
				Origin = origin,
				DrawCommands = drawCommands,
			};

			var passes = new []{ pass };
			CmdBufferInstructionSet output = mComposer.Compose (mRepository, passes);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (1, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (1, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);

			{
				var changeArray = output.Viewports;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);

				var actual = output.Viewports [0];
				Assert.AreEqual (0, actual.First);
				Assert.AreEqual (0, actual.Count);
				Assert.AreEqual (0, actual.Viewport.Values.Length);
				Assert.AreEqual (0, actual.DepthRange.Values.Length);
			}

			{
				var changeArray = output.BackCompareMasks;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.FrontCompareMasks;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.BackReferences;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.FrontReferences;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.BackWriteMasks;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.FrontWriteMasks;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.LineWidths;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.Scissors;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);

				var actual = output.Scissors [0];
				Assert.AreEqual (0, actual.Parameters.First);
				Assert.AreEqual (0, actual.Parameters.Count);
				Assert.AreEqual (0, actual.Parameters.Values.Length);
			}

			{
				var changeArray = output.BlendConstants;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.DescriptorSets;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.DepthBias;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var changeArray = output.DepthBounds;
				Assert.IsNotNull (changeArray);
				Assert.AreEqual (1, changeArray.Length);
			}

			{
				var actual = output.DrawItems [0];
				Assert.AreEqual (indexedDraw.firstInstance, actual.FirstInstance);
				Assert.AreEqual (indexedDraw.firstIndex, actual.First);
				Assert.AreEqual (indexedDraw.instanceCount, actual.InstanceCount);
				Assert.AreEqual (indexedDraw.vertexOffset, actual.VertexOffset);
				Assert.AreEqual (indexedDraw.indexCount, actual.Count);
				// UNUSED VALUES
				Assert.AreEqual (IntPtr.Zero, actual.Buffer);
				Assert.AreEqual (GLCommandBufferFlagBits.CmdDrawIndexed, actual.Command);
				Assert.AreEqual (0, actual.Stride);
				Assert.AreEqual (0, actual.Offset);
			}
		}

		[TestCase]
		public void SingleDrawIndirect()
		{			
			var pipeline = new MockGLGraphicsPipeline ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[] { };

			pipeline.VertexInput = new GLVertexBufferBinder(bindings, attributes);
			pipeline.Viewports = new GLCmdViewportParameter (0, new MgViewport[]{ });
			pipeline.Scissors = new GLCmdScissorParameter (0, new MgRect2D[]{ });

			mRepository.PushGraphicsPipeline (pipeline);

			mRepository.VertexBuffers.Add (
				new GLCmdVertexBufferParameter{					
					pBuffers = new IMgBuffer[]{},
				}
			);

			var indirectBuffer = new MockGLIndirectBuffer ();
			indirectBuffer.Source = new IntPtr (123456);

			var indirectDraw = new GLCmdInternalDrawIndirect {
				buffer = indirectBuffer,
				drawCount = 9,
				offset = 5,
				stride = 7,
			};

			var drawCommands = new List<GLCmdDrawCommand> ();

			drawCommands.Add (
				new GLCmdDrawCommand{
					Pipeline = 0,
					VertexBuffer = 0,
					DrawIndirect = indirectDraw,
				}
			);

			var origin = new MockIGLRenderPass (); 
			var pass = new GLCmdRenderPassCommand {
				Origin = origin,
				DrawCommands = drawCommands,
			};

			var passes = new []{ pass };
			CmdBufferInstructionSet output = mComposer.Compose (mRepository, passes);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (1, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (1, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);

			var actual = output.DrawItems[0];
			Assert.AreEqual(indirectBuffer.Source, actual.Buffer);
			Assert.AreEqual(indirectDraw.drawCount, actual.Count);
			Assert.AreEqual(indirectDraw.stride, actual.Stride);
			Assert.AreEqual(indirectDraw.offset, actual.Offset);

			// UNUSED VALUES
			Assert.AreEqual(GLCommandBufferFlagBits.CmdDrawIndirect, actual.Command);
			Assert.AreEqual(0, actual.First);
			Assert.AreEqual(0, actual.InstanceCount);
			Assert.AreEqual(0, actual.VertexOffset);
		}

		[TestCase]
		public void SingleDrawIndexedIndirect()
		{			
			var pipeline = new MockGLGraphicsPipeline ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[] { };

			pipeline.VertexInput = new GLVertexBufferBinder(bindings, attributes);
			pipeline.Viewports = new GLCmdViewportParameter (0, new MgViewport[]{ });
			pipeline.Scissors = new GLCmdScissorParameter (0, new MgRect2D[]{ });

			mRepository.PushGraphicsPipeline (pipeline);

			mRepository.VertexBuffers.Add (
				new GLCmdVertexBufferParameter{					
					pBuffers = new IMgBuffer[]{},
				}
			);

			var indirectBuffer = new MockGLIndirectBuffer ();
			indirectBuffer.Source = new IntPtr (123456);

			var indexedIndirectDraw = new GLCmdInternalDrawIndexedIndirect {
				buffer = indirectBuffer,
				drawCount = 9,
				offset = 5,
				stride = 7,
			};

			var drawCommands = new List<GLCmdDrawCommand> ();

			drawCommands.Add (
				new GLCmdDrawCommand{
					Pipeline = 0,
					VertexBuffer = 0,
					DrawIndexedIndirect = indexedIndirectDraw,
				}
			);

			var origin = new MockIGLRenderPass (); 
			var pass = new GLCmdRenderPassCommand {
				Origin = origin,
				DrawCommands = drawCommands,
			};

			var passes = new []{ pass };
			CmdBufferInstructionSet output = mComposer.Compose (mRepository, passes);

			Assert.IsNotNull (output);
			Assert.IsNotNull (output.DrawItems);
			Assert.AreEqual (1, output.DrawItems.Length);
			Assert.IsNotNull (output.Pipelines);
			Assert.AreEqual (1, output.Pipelines.Length);
			Assert.IsNotNull (output.VBOs);
			Assert.AreEqual (1, output.VBOs.Length);

			var actual = output.DrawItems[0];
			Assert.AreEqual(indirectBuffer.Source, actual.Buffer);
			Assert.AreEqual(indexedIndirectDraw.drawCount, actual.Count);
			Assert.AreEqual(indexedIndirectDraw.stride, actual.Stride);
			Assert.AreEqual(indexedIndirectDraw.offset, actual.Offset);

			// UNUSED VALUES
			Assert.AreEqual(GLCommandBufferFlagBits.CmdDrawIndexedIndirect, actual.Command);
			Assert.AreEqual(0, actual.First);
			Assert.AreEqual(0, actual.InstanceCount);
			Assert.AreEqual(0, actual.VertexOffset);
		}


		public void Test1()
		{
			MgCommandBufferBeginInfo cmdBufInfo = new MgCommandBufferBeginInfo {

			};
			//	vkTools::initializers::commandBufferBeginInfo();

			uint width = 200;
			uint height = 400;

			IMgRenderPass pass;

			MgRenderPassBeginInfo renderPassBeginInfo = new MgRenderPassBeginInfo
			{
				RenderPass = new MockRenderPass(),
				RenderArea = new MgRect2D
				{
					Offset = new MgOffset2D
					{
						X = 0,
						Y = 0,
					},
					Extent = new MgExtent2D
					{
						Width = width,
						Height = height,
					},
				},
				ClearValues = new []
				{
					new MgClearValue
					{
						Color = new MgClearColorValue(),
					},
					new MgClearValue
					{
					 	DepthStencil = new MgClearDepthStencilValue{
							Depth = 1f,						
							Stencil = 0,
						},
					}
				},
			};
				//vkTools::initializers::renderPassBeginInfo();


			Result err;

			GLCmdBufferRepository repository = new GLCmdBufferRepository ();
			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			IMgCommandBuffer cmdBuf = new GLCommandBuffer (true, repository, vbo);

			// Set target frame buffer
			IMgFramebuffer frameBuffer = new MockMgFrameBuffer();
			renderPassBeginInfo.Framebuffer = frameBuffer;

			err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
			Assert.AreEqual(Result.SUCCESS, err);

			cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);

			var viewport = new MgViewport[] {
				new MgViewport
				{
					Width = (float) width,
					Height = (float) height,
					MinDepth = 0f,
					MaxDepth = 1f,
				}
			};
			cmdBuf.CmdSetViewport(0, viewport);

			var scissor = new MgRect2D[]{
				new MgRect2D{
					Extent = new MgExtent2D
					{
						Width = width,
						Height = height,
					},
					Offset = new MgOffset2D
					{
						X =0,
						Y = 0
					},	
				},
			};
			cmdBuf.CmdSetScissor(0, scissor);

			IMgPipeline solid = new MockGLGraphicsPipeline ();

			IMgPipelineLayout pipelineLayout = new MockMgPipelineLayout ();

			uint firstSet = 0;
			uint descriptorSetCount = 1;
			IMgDescriptorSet[] descriptorSets = new IMgDescriptorSet[]{ };
			cmdBuf.CmdBindDescriptorSets(MgPipelineBindPoint.GRAPHICS, pipelineLayout, firstSet, descriptorSetCount, descriptorSets, null);
			cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, solid);

			uint firstBinding = 0;
			uint bindingCount = 1;

			IMgBuffer[] vertices = new IMgBuffer[]{ };

			var offsets = new ulong[]{ 0 };
			cmdBuf.CmdBindVertexBuffers(firstBinding, vertices, offsets);

			IMgBuffer indicesBuf = new MockMgIndexBuffer ();
			uint indicesCount = 0;

			cmdBuf.CmdBindIndexBuffer(indicesBuf, 0, MgIndexType.UINT32);

			cmdBuf.CmdDrawIndexed(indicesCount, 1, 0, 0, 0);

			cmdBuf.CmdEndRenderPass();

			err = cmdBuf.EndCommandBuffer();
			Assert.AreEqual(Result.SUCCESS, err);

		}
	}
}

