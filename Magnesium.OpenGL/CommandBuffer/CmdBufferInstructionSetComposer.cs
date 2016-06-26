using System.Collections.Generic;
using System;

namespace Magnesium.OpenGL
{
	public class CmdBufferInstructionSetComposer : ICmdBufferInstructionSetComposer
	{
		private ICmdVBOCapabilities mVBO;
		public CmdBufferInstructionSetComposer (ICmdVBOCapabilities vbo)
		{
			mVBO = vbo;
		}

		#region IComposer implementation

		public GLCmdVertexBufferObject GenerateVBO (GLCmdBufferRepository repository, GLCmdDrawCommand command, IGLGraphicsPipeline pipeline)
		{
			var vertexData = repository.VertexBuffers.At(command.VertexBuffer.Value);
			var noOfBindings = pipeline.VertexInput.Bindings.Length;
			var bufferIds = new int[noOfBindings];
			var offsets = new long[noOfBindings];

			for (uint i = 0; i < vertexData.bindingCount; ++i)
			{
				uint index = i + vertexData.firstBinding;
				var buffer = vertexData.pBuffers [index] as GLIndirectBuffer;
				// SILENT error
				if (buffer.BufferType == GLMemoryBufferType.VERTEX)
				{
					bufferIds [i] = buffer.BufferId;
					offsets [i] = (long)vertexData.pOffsets [i];
				}
				else
				{
					bufferIds [i] = 0;
				}
			}

			int vbo = mVBO.GenerateVBO ();
			foreach (var attribute in pipeline.VertexInput.Attributes)
			{	
				var bufferId = bufferIds [attribute.Binding];
				var binding = pipeline.VertexInput.Bindings [attribute.Binding];
				mVBO.AssociateBufferToLocation (vbo, attribute.Location, bufferId, offsets[attribute.Binding], binding.Stride);
				// GL.VertexArrayVertexBuffer (vertexArray, location, bufferId, new IntPtr (offset), (int)binding.Stride);

				if (attribute.Function == GLVertexAttribFunction.FLOAT)
				{
					// direct state access
					mVBO.BindFloatVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);

					//GL.VertexArrayAttribFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);
				}
				else if (attribute.Function == GLVertexAttribFunction.INT)
				{
					mVBO.BindIntVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);

					//GL.VertexArrayAttribIFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
				}
				else if (attribute.Function == GLVertexAttribFunction.DOUBLE)
				{
					mVBO.BindDoubleVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
					//GL.VertexArrayAttribLFormat (vbo, attribute.Location, attribute.Size, (All)attribute.PointerType, attribute.Offset);
				}
				mVBO.SetupVertexAttributeDivisor(vbo, attribute.Location, attribute.Divisor);
				//GL.VertexArrayBindingDivisor (vbo, attribute.Location, attribute.Divisor);
			}

			if (command.IndexBuffer.HasValue)
			{
				var indexData = repository.IndexBuffers.At(command.IndexBuffer.Value);
				var indexBuffer = indexData.buffer as GLIndirectBuffer;
				if (indexBuffer != null && indexBuffer.BufferType == GLMemoryBufferType.INDEX)
				{
					mVBO.BindIndexBuffer  (vbo, indexBuffer.BufferId);
					//GL.VertexArrayElementBuffer (vbo, indexBuffer.BufferId);
				}
			}

			return new GLCmdVertexBufferObject(vbo, command.VertexBuffer.Value, command.IndexBuffer, mVBO);
		}

		public GLCommandBufferFlagBits ExtractIndexType (GLCmdIndexBufferParameter indexBuffer, GLCommandBufferFlagBits commandType)
		{
			return (indexBuffer != null)
				&& ((commandType & GLCommandBufferFlagBits.UseIndexBuffer) == GLCommandBufferFlagBits.UseIndexBuffer)
				&& (indexBuffer.indexType == MgIndexType.UINT16)
				? GLCommandBufferFlagBits.Index16BitMode : 0;
		}

		public GLCmdBufferPipelineItem GeneratePipelineItem (IGLGraphicsPipeline pipeline)
		{
			QueueDrawItemBitFlags flags = pipeline.Flags;

			var pipelineItem = new GLCmdBufferPipelineItem {
				Flags = flags,
				DepthState = pipeline.DepthState,
				StencilState = pipeline.StencilState,
			};
			return pipelineItem;
		}

		static IntPtr ExtractIndirectBuffer (IMgBuffer buffer)
		{
			var indirectBuffer = buffer as IGLIndirectBuffer;
			return (indirectBuffer != null && indirectBuffer.BufferType == GLMemoryBufferType.INDIRECT) ? indirectBuffer.Source : IntPtr.Zero;
		}

		static byte GetStoreViaNullableIndex<TData>(List<TData> dest, IGLCmdBufferStore<TData> src, int? index)
		{
			if (index.HasValue)
			{
				var data = src.At (index.Value);

				if (dest.Count == 0)
				{
					dest.Add (data);
					return 0;
				}
				else
				{					
					var topIndex = dest.Count - 1;
					var top = dest [topIndex];
					if (data.Equals (top))
					{
						return (byte)topIndex;
					} 
					else
					{
						dest.Add (data);
						return (byte)(dest.Count - 1);
					}
				}
			} 
			else
			{
				return 0;
			}
		}

		static GLCommandBufferFlagBits ExtractPolygonMode (IGLGraphicsPipeline pipeline)
		{
			return (pipeline.PolygonMode == MgPolygonMode.LINE) ? GLCommandBufferFlagBits.AsLinesMode : ((pipeline.PolygonMode == MgPolygonMode.POINT) ? GLCommandBufferFlagBits.AsPointsMode : 0);
		}

		public CmdBufferInstructionSet Compose (GLCmdBufferRepository repository, IEnumerable<GLCmdRenderPassCommand> passes)
		{
			var output = new CmdBufferInstructionSet ();

			//var passes = new List<GLQueueRenderPass> ();
			var pipelines = new List<GLCmdBufferPipelineItem> ();
			var drawItems = new List<GLCmdBufferDrawItem> ();
			var vertexArrays = new List<GLCmdVertexBufferObject> ();

			var viewports = new List<GLCmdViewportParameter> ();
			var backCompareMasks = new List<int> ();
			var frontCompareMasks = new List<int> ();
			var backReferences = new List<int> ();
			var frontReferences = new List<int> ();
			var backWriteMasks = new List<int> ();
			var frontWriteMasks = new List<int> ();
			var lineWidths = new List<float> ();
			var scissors = new List<GLCmdScissorParameter>();
			var blendConstants = new List<MgColor4f>();
			var descriptorSets = new List<GLCmdDescriptorSetParameter>();
			var depthBias = new List<GLCmdDepthBiasParameter> ();
			var depthBounds = new List<GLCmdDepthBoundsParameter> ();

			// Generate commands here

			// set up defaults
//			var defaultPass = new GLQueueRenderPass { Index = 0 } ;
//			passes.Add (defaultPass);

			GLCmdVertexBufferObject currentVertexArray = null;

			bool isFirst = true;
			int pastPipelineIndex = 0;
			int currentPipelineIndex = 0;
			GLCmdBufferPipelineItem pipelineItem;

			if (passes != null)
			{
				foreach (var pass in passes)
				{
//					var rp = new GLQueueRenderPass { 
//						Index = (byte) passes.Count,
//						ClearValues = pass.ClearValues,
//					};
//					passes.Add (rp);

					foreach (var command in pass.DrawCommands)
					{
						if (command.Pipeline.HasValue)
						{
							currentPipelineIndex = command.Pipeline.Value;

							var pipeline = repository.GraphicsPipelines.At(currentPipelineIndex);

							if (isFirst)
							{
								isFirst = false;

								pipelineItem = GeneratePipelineItem (pipeline);
								pipelineItem.DepthState = pipeline.DepthState;
								pipelineItem.StencilState = pipeline.StencilState;
								pipelines.Add (pipelineItem);

							}
							else
							{
								if (pastPipelineIndex != currentPipelineIndex)
								{		
									pipelineItem = GeneratePipelineItem (pipeline);
									pipelineItem.DepthState = pipeline.DepthState;
									pipelineItem.StencilState = pipeline.StencilState;
									pipelines.Add (pipelineItem);
								}
							}

							var item = new GLCmdBufferDrawItem {
								Command = ExtractPolygonMode (pipeline), 
								ProgramID = pipeline.ProgramID,
								Topology = pipeline.Topology,
								Pipeline = (byte) currentPipelineIndex,
								DescriptorSet = GetStoreViaNullableIndex<GLCmdDescriptorSetParameter>(descriptorSets, repository.DescriptorSets, command.DescriptorSet),
								Viewport = GetStoreViaNullableIndex<GLCmdViewportParameter>(viewports, repository.Viewports, command.Viewports), 
								BlendConstants = GetStoreViaNullableIndex<MgColor4f>(blendConstants, repository.BlendConstants, command.BlendConstants),
								LineWidth = GetStoreViaNullableIndex<float>(lineWidths, repository.LineWidths, command.LineWidth),
								DepthBias = GetStoreViaNullableIndex<GLCmdDepthBiasParameter>(depthBias, repository.DepthBias, command.DepthBias),
								DepthBounds = GetStoreViaNullableIndex<GLCmdDepthBoundsParameter>(depthBounds, repository.DepthBounds, command.DepthBounds),
								FrontStencilCompareMask = GetStoreViaNullableIndex<int>(frontCompareMasks, repository.FrontCompareMasks, command.FrontCompareMask),
								BackStencilCompareMask = GetStoreViaNullableIndex<int>(backCompareMasks, repository.BackCompareMasks, command.BackCompareMask),
								FrontStencilReference= GetStoreViaNullableIndex<int>(frontReferences, repository.FrontReferences, command.FrontReference),
								BackStencilReference = GetStoreViaNullableIndex<int>(backReferences, repository.BackReferences, command.BackReference),
								FrontStencilWriteMask = GetStoreViaNullableIndex<int>(frontWriteMasks, repository.FrontWriteMasks, command.FrontWriteMask),
								BackStencilWriteMask = GetStoreViaNullableIndex<int>(backWriteMasks, repository.BackWriteMasks, command.BackWriteMask),
								Scissor = GetStoreViaNullableIndex<GLCmdScissorParameter>(scissors, repository.Scissors, command.Scissors),
							};

							pastPipelineIndex = currentPipelineIndex;

							if (command.DrawIndexedIndirect != null)
							{
								item.Command |= GLCommandBufferFlagBits.CmdDrawIndexedIndirect;

								var drawCommand = command.DrawIndexedIndirect;
			
								item.Buffer = ExtractIndirectBuffer (drawCommand.buffer);							

								item.Count = drawCommand.drawCount;
								item.Offset  = drawCommand.offset;
								item.Stride = drawCommand.stride;
							}
							else if (command.DrawIndirect != null)
							{
								item.Command |= GLCommandBufferFlagBits.CmdDrawIndirect;

								var drawCommand = command.DrawIndirect;

								item.Buffer = ExtractIndirectBuffer (drawCommand.buffer);

								item.Count = drawCommand.drawCount;
								item.Offset = drawCommand.offset;
								item.Stride = drawCommand.stride;
							}
							else if (command.DrawIndexed != null)
							{
								item.Command |= GLCommandBufferFlagBits.CmdDrawIndexed;

								var drawCommand = command.DrawIndexed;
								item.First = drawCommand.firstIndex;
								item.FirstInstance = drawCommand.firstInstance;
								item.Count = drawCommand.indexCount;
								item.VertexOffset = drawCommand.vertexOffset;
								item.InstanceCount = drawCommand.instanceCount;
							}
							else if (command.Draw != null)
							{
								var drawCommand = command.Draw;

								item.FirstInstance = drawCommand.firstInstance;
								item.First = drawCommand.firstVertex;
								item.InstanceCount = drawCommand.instanceCount;
								item.Count = drawCommand.vertexCount;
							}
							else 
							{
								throw new InvalidOperationException();
							}

							if (command.VertexBuffer.HasValue)
							{

								// create new vbo
								if (currentVertexArray == null)
								{
									currentVertexArray = GenerateVBO (repository, command, pipeline);
									vertexArrays.Add (currentVertexArray);
								}
								else
								{
									bool useSameBuffers = (command.IndexBuffer.HasValue)
										? currentVertexArray.Matches (
						                      command.VertexBuffer.Value,
						                      command.IndexBuffer.Value)
										: currentVertexArray.Matches (
										     command.VertexBuffer.Value);

									if (!useSameBuffers)
									{
										currentVertexArray = GenerateVBO (repository, command, pipeline);
										vertexArrays.Add (currentVertexArray);
									}
								}

								item.VBO = currentVertexArray.VBO;
							}
							if (command.IndexBuffer.HasValue)
							{
								var indexBuffer = repository.IndexBuffers.At (command.IndexBuffer.Value);
								item.Command |= ExtractIndexType (indexBuffer, item.Command);
							}

							// build drawable 
							drawItems.Add (item);
						}

					}
				}
			}

			output.DrawItems = drawItems.ToArray ();
			output.Pipelines = pipelines.ToArray ();
			output.VBOs = vertexArrays.ToArray ();

			output.Viewports = viewports.ToArray ();
			output.BackCompareMasks = backCompareMasks.ToArray ();
			output.FrontCompareMasks = frontCompareMasks.ToArray ();
			output.BackReferences = backReferences.ToArray ();
			output.FrontReferences = frontReferences.ToArray ();
			output.BackWriteMasks = backWriteMasks.ToArray ();
			output.FrontWriteMasks = frontWriteMasks.ToArray ();
			output.LineWidths = lineWidths.ToArray ();
			output.Scissors = scissors.ToArray ();
			output.BlendConstants = blendConstants.ToArray ();
			output.DescriptorSets = descriptorSets.ToArray ();
			output.DepthBias = depthBias.ToArray ();
			output.DepthBounds = depthBounds.ToArray ();

			return output;
		}
		#endregion
	}

}

