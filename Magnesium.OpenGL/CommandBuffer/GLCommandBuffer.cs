using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCommandBuffer : IMgCommandBuffer
	{
		private bool mIsRecording = false;
		private bool mIsExecutable = false;
		private bool mCanBeManuallyReset;

		private GLCmdComputeCommand mIncompleteComputeCommand;
		private GLCmdRenderPassCommand mIncompleteRenderPass;
		private List<GLCmdRenderPassCommand> mRenderPasses = new List<GLCmdRenderPassCommand>();
		private List<GLCmdDrawCommand> mIncompleteDraws = new List<GLCmdDrawCommand>();

		private GLCmdBufferRepository mRepository;
		public GLCommandBuffer (bool canBeManuallyReset, GLCmdBufferRepository repository)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mCanBeManuallyReset = canBeManuallyReset;
			mRepository = repository;
		}

		#region IMgCommandBuffer implementation

		public MgCommandBufferUsageFlagBits SubmissionRule { get; private set; }
		public bool IsQueueReady { get; set; }
		public Result BeginCommandBuffer (MgCommandBufferBeginInfo pBeginInfo)
		{
			SubmissionRule = pBeginInfo.Flags;
			IsQueueReady = true;

			mIsRecording = true;

			return Result.SUCCESS;
		}

		public struct GLQueueRenderPass
		{
			public byte Index { get; set; }
			public MgClearValue[] ClearValues { get; set; }
		}

		public GLQueueRenderPass[] Passes { get; private set; }
		public GLQueueDrawItem[] DrawItems { get ; private set; }
		public Result EndCommandBuffer ()
		{
			mIsRecording = false;
			mIsExecutable = true;

			// Generate commands here
			var drawItems = new List<GLQueueDrawItem>();
			var passes = new List<GLQueueRenderPass> ();

			// set up defaults
			var defaultPass = new GLQueueRenderPass { Index = 0 } ;
			passes.Add (defaultPass);


			if (mRenderPasses.Count > 0)
			{
				foreach (var pass in mRenderPasses)
				{
					var rp = new GLQueueRenderPass { 
						Index = (byte) passes.Count,
						ClearValues = pass.ClearValues,
					};
					passes.Add (rp);

					foreach (var command in pass.DrawCommands)
					{
						var pipeline = mRepository.GraphicsPipelines.Items [command.Pipeline];

						var depth = new GLQueueDepthState {
							
						};

						var stencil = new GLQueueStencilState {

						};

						var blend = new GLQueueBlendState {

						};
							

						var drawItem = new GLQueueDrawItem {
							PassIndex = rp.Index,
							ProgramIndex = (ushort) pipeline.ProgramId,
							Topology = pipeline.Topology,
							Mode = pipeline.PolygonMode,
							StencilValues = new GLQueueStencilState{

							},
						};

						drawItems.Add (drawItem);

					}
				}


			}
			else
			{

			}


			DrawItems = drawItems.ToArray ();
			

			return Result.SUCCESS;
		}

		public void ResetAllData ()
		{
			mIncompleteRenderPass = null;
			mIncompleteComputeCommand = null;
			mRepository.Clear ();
			mRenderPasses.Clear ();
			mIncompleteDraws.Clear ();
		}

		public Result ResetCommandBuffer (MgCommandBufferResetFlagBits flags)
		{
			if (mCanBeManuallyReset)
			{				
				ResetAllData ();
				// OTHERWISE WAIT FOR BULK RESET VIA COMMAND POOL
			}
			return Result.SUCCESS;
		}

		public void CmdBindPipeline (MgPipelineBindPoint pipelineBindPoint, IMgPipeline pipeline)
		{
			if (pipelineBindPoint == MgPipelineBindPoint.COMPUTE)
			{
				mIncompleteComputeCommand.Pipeline = pipeline;
			}
			else
			{
				mRepository.GraphicsPipelines.Add (pipeline as GLGraphicsPipeline);
			}
		}

		public void CmdSetViewport (uint firstViewport, uint viewportCount, MgViewport[] pViewports)
		{
			var param = new GLCmdViewportParameter ();

			param.first = (int)firstViewport;
			param.values = new float[4 * viewportCount];

			for (uint i = 0; i < viewportCount; ++i)
			{
				param.values [0 + 4 * i] = pViewports [i].X;
				param.values [1 + 4 * i] = pViewports [i].Y;
				param.values [2 + 4 * i] = pViewports [i].Width;
				param.values [3 + 4 * i] = pViewports [i].Height;
			}

			mRepository.Viewports.Add (param);
		}

		public void CmdSetScissor (uint firstScissor, uint scissorCount, MgRect2D[] pScissors)
		{
			var param = new GLCmdScissorParameter ();

			param.index = firstScissor;
			param.count = scissorCount;
			param.values = new float[4 * scissorCount];

			for (uint i = 0; i < scissorCount; ++i)
			{
				param.values [0 + 4 * i] = pScissors [i].Offset.X;
				param.values [1 + 4 * i] = pScissors [i].Offset.Y;
				param.values [2 + 4 * i] = pScissors [i].Extent.Width;
				param.values [3 + 4 * i] = pScissors [i].Extent.Height;
			}

			mRepository.Scissors.Add (param);
		}

		public void CmdSetLineWidth (float lineWidth)
		{
			mRepository.LineWidth = lineWidth;
		}

		public void CmdSetDepthBias (float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			mRepository.DepthBiasConstantFactor = depthBiasConstantFactor;
			mRepository.DepthBiasClamp = depthBiasClamp;
			mRepository.DepthBiasSlopeFactor = depthBiasSlopeFactor;
		}

		public void CmdSetBlendConstants (float[] blendConstants)
		{
			mRepository.BlendConstants = blendConstants;
		}

		public void CmdSetDepthBounds (float minDepthBounds, float maxDepthBounds)
		{
			mRepository.MinDepthBounds = minDepthBounds;
			mRepository.MaxDepthBounds = maxDepthBounds;
		}

		public void CmdSetStencilCompareMask (MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			mRepository.CompareFace = faceMask;
			mRepository.CompareMask = compareMask;
		}

		public void CmdSetStencilWriteMask (MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			mRepository.WriteFace = faceMask;
			mRepository.WriteMask = writeMask; 
		}

		public void CmdSetStencilReference (MgStencilFaceFlagBits faceMask, uint reference)
		{
			mRepository.ReferenceFace = faceMask;
			mRepository.Reference = reference; 
		}

		public void CmdBindDescriptorSets (
			MgPipelineBindPoint pipelineBindPoint,
			IMgPipelineLayout layout,
			uint firstSet,
			uint descriptorSetCount,
			MgDescriptorSet[] pDescriptorSets,
			uint[] pDynamicOffsets)
		{
			var parameter = new GLCmdDescriptorSetParameter ();		
			parameter.bindpoint = pipelineBindPoint;
			parameter.firstSet = firstSet;
			parameter.dynamicOffsets = pDynamicOffsets;
			mRepository.DescriptorSets.Add (parameter);
		}

		public void CmdBindIndexBuffer (IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			var param = new GLCmdIndexBufferParameter ();
			param.buffer = buffer;
			param.offset = offset;
			param.indexType = indexType;
			mRepository.IndexBuffers.Add (param);
		}

		public void CmdBindVertexBuffers (uint firstBinding, uint bindingCount, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			var param = new GLCmdVertexBufferParameter ();
			param.firstBinding = firstBinding;
			param.bindingCount = bindingCount;
			param.pBuffers = pBuffers;
			param.pOffsets = pOffsets;
			mRepository.VertexBuffers.Add (param);
		}

		void StoreDrawCommand (GLCmdDrawCommand command)
		{
			if (mRepository.MapRepositoryFields (ref command))
			{
				if (mIncompleteRenderPass != null)
				{
					mIncompleteRenderPass.DrawCommands.Add (command);
				} 
				else
				{
					mIncompleteDraws.Add (command);
				}
			}
		}

		public void CmdDraw (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			//void glDrawArraysInstancedBaseInstance(GLenum mode​, GLint first​, GLsizei count​, GLsizei primcount​, GLuint baseinstance​);
			//mDrawCommands.Add (mIncompleteDrawCommand);
			// first => firstVertex
			// count => vertexCount
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// baseinstance => firstInstance Specifies the base instance for use in fetching instanced vertex attributes.

			var command = new GLCmdDrawCommand ();
			command.CommandType = GLCmdDrawCommand.DrawType.Draw;
			command.vertexCount = vertexCount;
			command.instanceCount = instanceCount;
			command.firstVertex = firstVertex;
			command.firstInstance = firstInstance;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndexed (uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			// void glDrawElementsInstancedBaseVertex(GLenum mode​, GLsizei count​, GLenum type​, GLvoid *indices​, GLsizei primcount​, GLint basevertex​);
			// count => indexCount Specifies the number of elements to be rendered. (divide by elements)
			// indices => firstIndex Specifies a byte offset (cast to a pointer type) (multiple by data size)
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// basevertex => vertexOffset Specifies a constant that should be added to each element of indices​ when chosing elements from the enabled vertex arrays.
				// TODO : need to handle negetive offset
			//mDrawCommands.Add (mIncompleteDrawCommand);

			var command = new GLCmdDrawCommand ();
			command.CommandType = GLCmdDrawCommand.DrawType.DrawIndexed;
			command.indexCount = indexCount;
			command.instanceCount = instanceCount;
			command.firstIndex = firstIndex;
			command.vertexOffset = vertexOffset;
			command.firstInstance = firstInstance;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndirect (IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			// ARB_multi_draw_indirect
//			typedef struct VkDrawIndirectCommand {
//				uint32_t    vertexCount;
//				uint32_t    instanceCount; 
//				uint32_t    firstVertex; 
//				uint32_t    firstInstance;
//			} VkDrawIndirectCommand;
			// glMultiDrawArraysIndirect 
			//void glMultiDrawArraysIndirect(GLenum mode​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect => buffer + offset IntPtr
			// drawCount => drawCount
			// stride => stride
//			typedef  struct {
//				uint  count;
//				uint  instanceCount;
//				uint  first;
//				uint  baseInstance;
//			} DrawArraysIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);

			var command = new GLCmdDrawCommand ();
			command.CommandType = GLCmdDrawCommand.DrawType.DrawIndirect;
			command.offset = offset;
			command.drawCount = drawCount;
			command.stride = stride;

			StoreDrawCommand(command);
		}

		public void CmdDrawIndexedIndirect (IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
//			typedef struct VkDrawIndexedIndirectCommand {
//				uint32_t    indexCount;
//				uint32_t    instanceCount;
//				uint32_t    firstIndex;
//				int32_t     vertexOffset;
//				uint32_t    firstInstance;
//			} VkDrawIndexedIndirectCommand;
			// void glMultiDrawElementsIndirect(GLenum mode​, GLenum type​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect  => buffer + offset (IntPtr)
			// drawcount => drawcount
			// stride => stride
//			glDrawElementsInstancedBaseVertexBaseInstance(mode,
//				cmd->count,
//				type,
//				cmd->firstIndex * size-of-type,
//				cmd->instanceCount,
//				cmd->baseVertex,
//				cmd->baseInstance);
//			typedef  struct {
//				uint  count;
//				uint  instanceCount;
//				uint  firstIndex;
//				uint  baseVertex; // TODO: negetive index
//				uint  baseInstance;
//			} DrawElementsIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);
			var command = new GLCmdDrawCommand ();
			command.CommandType = GLCmdDrawCommand.DrawType.DrawIndexedIndirect;
			command.offset = offset;
			command.drawCount = drawCount;
			command.stride = stride;

			StoreDrawCommand(command);
		}

		public void CmdDispatch (uint x, uint y, uint z)
		{
			throw new NotImplementedException ();
		}

		public void CmdDispatchIndirect (IMgBuffer buffer, ulong offset)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyBuffer (IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdBlitImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageBlit[] pRegions, MgFilter filter)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyBufferToImage (IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyImageToBuffer (IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdUpdateBuffer (IMgBuffer dstBuffer, ulong dstOffset, UIntPtr dataSize, IntPtr pData)
		{
			throw new NotImplementedException ();
		}

		public void CmdFillBuffer (IMgBuffer dstBuffer, ulong dstOffset, ulong size, uint data)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearColorImage (IMgImage image, MgImageLayout imageLayout, MgClearColorValue pColor, MgImageSubresourceRange[] pRanges)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearDepthStencilImage (IMgImage image, MgImageLayout imageLayout, MgClearDepthStencilValue pDepthStencil, MgImageSubresourceRange[] pRanges)
		{
			throw new NotImplementedException ();
		}

		public void CmdClearAttachments (MgClearAttachment[] pAttachments, MgClearRect[] pRects)
		{
			throw new NotImplementedException ();
		}

		public void CmdResolveImage (IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdSetEvent (MgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException ();
		}

		public void CmdResetEvent (MgEvent @event, MgPipelineStageFlagBits stageMask)
		{
			throw new NotImplementedException ();
		}

		public void CmdWaitEvents (MgEvent[] pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			throw new NotImplementedException ();
		}

		public void CmdPipelineBarrier (MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			// TODO : need barrier here
			//throw new NotImplementedException ();
		}

		public void CmdBeginQuery (MgQueryPool queryPool, uint query, MgQueryControlFlagBits flags)
		{
			throw new NotImplementedException ();
		}

		public void CmdEndQuery (MgQueryPool queryPool, uint query)
		{
			throw new NotImplementedException ();
		}

		public void CmdResetQueryPool (MgQueryPool queryPool, uint firstQuery, uint queryCount)
		{
			throw new NotImplementedException ();
		}

		public void CmdWriteTimestamp (MgPipelineStageFlagBits pipelineStage, MgQueryPool queryPool, uint query)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyQueryPoolResults (MgQueryPool queryPool, uint firstQuery, uint queryCount, IMgBuffer dstBuffer, ulong dstOffset, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}

		public void CmdPushConstants (IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues)
		{
			throw new NotImplementedException ();
		}

		public void CmdBeginRenderPass (MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			mIncompleteRenderPass = new GLCmdRenderPassCommand ();
			mIncompleteRenderPass.Origin = pRenderPassBegin.RenderPass;
			mIncompleteRenderPass.ClearValues = pRenderPassBegin.ClearValues;
			mIncompleteRenderPass.Contents = contents;
		}

		public void CmdNextSubpass (MgSubpassContents contents)
		{
			throw new NotImplementedException ();
		}

		public void CmdEndRenderPass ()
		{
			mRenderPasses.Add (mIncompleteRenderPass);
		}

		public void CmdExecuteCommands (IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

