using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCommandBuffer : IMgCommandBuffer
	{
		public class GLDrawCommand
		{
			public GLRenderPassCommand RenderPass { get; set; }
		}

		public class GLViewportParameter
		{
			public int first;
			public int count;
			public float[] values;
		}

		public class GLDescriptorSetParameter
		{
			public uint[] dynamicOffsets {
				get;
				set;
			}

			public uint firstSet {
				get;
				set;
			}

			public MgPipelineBindPoint bindpoint {
				get;
				set;
			}
		}

		public class GLScissorParameter
		{
			public uint count {
				get;
				set;
			}

			public uint index;
			// MULTIPLES OF 4
			public float[] values;
		}

		public class GLDrawRepository
		{
			public GLDrawRepository ()
			{
				GraphicsPipelines = new List<GLGraphicsPipeline>();
				Viewports = new List<GLViewportParameter>();
				DescriptorSets = new List<GLDescriptorSetParameter>();
			}

			public List<GLGraphicsPipeline> GraphicsPipelines { get; private set; }
			public List<GLViewportParameter> Viewports { get; private set; }
			public List<GLDescriptorSetParameter> DescriptorSets { get; private set; }
			public List<GLScissorParameter> Scissors {get; private set;}

			public void Clear ()
			{
				GraphicsPipelines.Clear ();
				Viewports.Clear ();
				DescriptorSets.Clear ();
				Scissors.Clear ();

				// clear nullable fields
				Reference = null;
				ReferenceFace = null;
				WriteFace = null;
				WriteMask = null;
				CompareMask = null;
				CompareFace = null;
				MinDepthBounds = null;
				MaxDepthBounds = null;
				DepthBiasConstantFactor = null;
				DepthBiasClamp = null;
				LineWidth = null;
			}

			#region Nullable fields 

			public uint? Reference {
				get;
				set;
			}

			public MgStencilFaceFlagBits? ReferenceFace {
				get;
				set;
			}

			public MgStencilFaceFlagBits? WriteFace {
				get;
				set;
			}

			public uint? WriteMask {
				get;
				set;
			}

			public uint? CompareMask {
				get;
				set;
			}

			public MgStencilFaceFlagBits? CompareFace {
				get;
				set;
			}

			public float? MinDepthBounds {
				get;
				set;
			}

			public float? MaxDepthBounds {
				get;
				set;
			}

			public float? DepthBiasConstantFactor {
				get;
				set;
			}

			public float? DepthBiasClamp {
				get;
				set;
			}

			public float DepthBiasSlopeFactor {
				get;
				set;
			}

			public float? LineWidth {
				get;
				set;
			}

			#endregion
		}

		public class GLComputeCommand
		{
			public IMgPipeline Pipeline {
				get;
				set;
			}
		}

		public class GLRenderPassCommand
		{
			public MgSubpassContents Contents {
				get;
				set;
			}

			public MgClearValue[] ClearValues {
				get;
				set;
			}

			public MgRenderPass Origin {
				get;
				set;
			}

			public GLDrawCommand[] DrawCommands;
			public GLComputeCommand[] ComputeCommands;
		}

		private bool mIsRecording;
		private bool mIsExecutable;
		private bool mResetOnBegin;

		private GLDrawCommand mIncompleteDrawCommand;
		private GLComputeCommand mIncompleteComputeCommand;
		private GLRenderPassCommand mIncompleteRenderPass;
		private List<GLRenderPassCommand> mRenderPasses = new List<GLRenderPassCommand>();

		private GLDrawRepository mRepository;
		public GLCommandBuffer (bool resetOnBegin)
		{
			mIsRecording = false;
			mIsExecutable = false;
			mResetOnBegin = resetOnBegin;
			mRepository = new GLDrawRepository();
		}

		#region IMgCommandBuffer implementation

		public Result BeginCommandBuffer (MgCommandBufferBeginInfo pBeginInfo)
		{
			if (mResetOnBegin)
			{
				mIncompleteRenderPass = null;
				mIncompleteDrawCommand = null;
				mIncompleteComputeCommand = null;
				mRepository.Clear ();

				mRenderPasses.Clear ();
			}

			mIsRecording = true;

			return Result.SUCCESS;
		}

		public Result EndCommandBuffer ()
		{
			mIsRecording = false;
			mIsExecutable = true;
			return Result.SUCCESS;
		}

		public Result ResetCommandBuffer (MgCommandBufferResetFlagBits flags)
		{
			throw new NotImplementedException ();
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
			var param = new GLViewportParameter ();

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
			var param = new GLScissorParameter ();

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
			throw new NotImplementedException ();
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
			var parameter = new GLDescriptorSetParameter ();
			parameter.bindpoint = pipelineBindPoint;
			parameter.firstSet = firstSet;
			parameter.dynamicOffsets = pDynamicOffsets;
			mRepository.DescriptorSets.Add (parameter);
		}

		public void CmdBindIndexBuffer (MgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			throw new NotImplementedException ();
		}

		public void CmdBindVertexBuffers (uint firstBinding, uint bindingCount, MgBuffer[] pBuffers, ulong[] pOffsets)
		{
			throw new NotImplementedException ();
		}

		public void CmdDraw (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			//mDrawCommands.Add (mIncompleteDrawCommand);
		}

		public void CmdDrawIndexed (uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			//mDrawCommands.Add (mIncompleteDrawCommand);
		}

		public void CmdDrawIndirect (MgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			//mDrawCommands.Add (mIncompleteDrawCommand);
		}

		public void CmdDrawIndexedIndirect (MgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			//mDrawCommands.Add (mIncompleteDrawCommand);
		}

		public void CmdDispatch (uint x, uint y, uint z)
		{
			throw new NotImplementedException ();
		}

		public void CmdDispatchIndirect (MgBuffer buffer, ulong offset)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyBuffer (MgBuffer srcBuffer, MgBuffer dstBuffer, MgBufferCopy[] pRegions)
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

		public void CmdCopyBufferToImage (MgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdCopyImageToBuffer (IMgImage srcImage, MgImageLayout srcImageLayout, MgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			throw new NotImplementedException ();
		}

		public void CmdUpdateBuffer (MgBuffer dstBuffer, ulong dstOffset, UIntPtr dataSize, IntPtr pData)
		{
			throw new NotImplementedException ();
		}

		public void CmdFillBuffer (MgBuffer dstBuffer, ulong dstOffset, ulong size, uint data)
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
			throw new NotImplementedException ();
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

		public void CmdCopyQueryPoolResults (MgQueryPool queryPool, uint firstQuery, uint queryCount, MgBuffer dstBuffer, ulong dstOffset, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}

		public void CmdPushConstants (IMgPipelineLayout layout, MgShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues)
		{
			throw new NotImplementedException ();
		}

		public void CmdBeginRenderPass (MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			mIncompleteRenderPass = new GLRenderPassCommand ();
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

