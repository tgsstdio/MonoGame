using System;

namespace MonoGame.Graphics
{
	using VkImage = MonoGame.Graphics.Image;

	// CommandBuffer
	public interface ICommandBuffer
	{
		Result BeginCommandBuffer(CommandBufferBeginInfo pBeginInfo);
		Result EndCommandBuffer();
		Result ResetCommandBuffer(CommandBufferResetFlagBits flags);
		void CmdBindPipeline(PipelineBindPoint pipelineBindPoint, Pipeline pipeline);
		void CmdSetViewport(UInt32 firstViewport, UInt32 viewportCount, Viewport[] pViewports);
		void CmdSetScissor(UInt32 firstScissor, UInt32 scissorCount, Rect2D[] pScissors);
		void CmdSetLineWidth(float lineWidth);
		void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		void CmdSetBlendConstants(float[] blendConstants); // 4
		void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds);
		void CmdSetStencilCompareMask(StencilFaceFlagBits faceMask, UInt32 compareMask);
		void CmdSetStencilWriteMask(StencilFaceFlagBits faceMask, UInt32 writeMask);
		void CmdSetStencilReference(StencilFaceFlagBits faceMask, UInt32 reference);
		void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, DescriptorSet[] pDescriptorSets, UInt32[] pDynamicOffsets);
		void CmdBindIndexBuffer(Buffer buffer, UInt64 offset, IndexType indexType);
		void CmdBindVertexBuffers(UInt32 firstBinding, UInt32 bindingCount, Buffer[] pBuffers, UInt64[] pOffsets);
		void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance);
		void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance);
		void CmdDrawIndirect(Buffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDrawIndexedIndirect(Buffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDispatch(UInt32 x, UInt32 y, UInt32 z);
		void CmdDispatchIndirect(Buffer buffer, UInt64 offset);
		void CmdCopyBuffer(Buffer srcBuffer, Buffer dstBuffer, BufferCopy[] pRegions);
		void CmdCopyImage(VkImage srcImage, ImageLayout srcImageLayout, VkImage dstImage, ImageLayout dstImageLayout, ImageCopy[] pRegions);
		void CmdBlitImage(VkImage srcImage, ImageLayout srcImageLayout, VkImage dstImage, ImageLayout dstImageLayout, ImageBlit[] pRegions, Filter filter);
		void CmdCopyBufferToImage(Buffer srcBuffer, VkImage dstImage, ImageLayout dstImageLayout, BufferImageCopy[] pRegions);
		void CmdCopyImageToBuffer(VkImage srcImage, ImageLayout srcImageLayout, Buffer dstBuffer, BufferImageCopy[] pRegions);
		void CmdUpdateBuffer(Buffer dstBuffer, UInt64 dstOffset, UIntPtr dataSize, IntPtr pData);
		void CmdFillBuffer(Buffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);
		void CmdClearColorImage(VkImage image, ImageLayout imageLayout, ClearColorValue pColor, ImageSubresourceRange[] pRanges);
		void CmdClearDepthStencilImage(VkImage image, ImageLayout imageLayout, ClearDepthStencilValue pDepthStencil, ImageSubresourceRange[] pRanges);
		void CmdClearAttachments(ClearAttachment[] pAttachments, ClearRect[] pRects);
		void CmdResolveImage(VkImage srcImage, ImageLayout srcImageLayout, VkImage dstImage, ImageLayout dstImageLayout, ImageResolve[] pRegions);
		void CmdSetEvent(Event @event, PipelineStageFlagBits stageMask);
		void CmdResetEvent(Event @event, PipelineStageFlagBits stageMask);
		void CmdWaitEvents(Event[] pEvents, PipelineStageFlagBits srcStageMask, PipelineStageFlagBits dstStageMask, MemoryBarrier[] pMemoryBarriers, BufferMemoryBarrier[] pBufferMemoryBarriers, ImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdPipelineBarrier(PipelineStageFlagBits srcStageMask, PipelineStageFlagBits dstStageMask, DependencyFlagBits dependencyFlags, MemoryBarrier[] pMemoryBarriers, BufferMemoryBarrier[] pBufferMemoryBarriers, ImageMemoryBarrier[] pImageMemoryBarriers);
		void CmdBeginQuery(QueryPool queryPool, UInt32 query, QueryControlFlagBits flags);
		void CmdEndQuery(QueryPool queryPool, UInt32 query);
		void CmdResetQueryPool(QueryPool queryPool, UInt32 firstQuery, UInt32 queryCount);
		void CmdWriteTimestamp(PipelineStageFlagBits pipelineStage, QueryPool queryPool, UInt32 query);
		void CmdCopyQueryPoolResults(QueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, Buffer dstBuffer, UInt64 dstOffset, UInt64 stride, QueryResultFlagBits flags);
		void CmdPushConstants(PipelineLayout layout, ShaderStageFlagBits stageFlags, UInt32 offset, UInt32 size, IntPtr pValues);
		void CmdBeginRenderPass(RenderPassBeginInfo pRenderPassBegin, SubpassContents contents);
		void CmdNextSubpass(SubpassContents contents);
		void CmdEndRenderPass();
		void CmdExecuteCommands(ICommandBuffer[] pCommandBuffers);
	}
}

