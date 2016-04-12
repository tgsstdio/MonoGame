using System;

namespace Magnesium 
{
	// INTERFACES
	// Vulkan
	public interface IVulkan
	{
		Result CreateInstance(InstanceCreateInfo createInfo, AllocationCallbacks allocator, out IInstance instance);
		Result EnumerateInstanceLayerProperties(out LayerProperties[] properties);
		Result EnumerateInstanceExtensionProperties(string layerName, out ExtensionProperties[] pProperties);
	}

	// Instance
	public interface IInstance
	{
		void DestroyInstance(AllocationCallbacks allocator);
		Result EnumeratePhysicalDevices(out IPhysicalDevice[] physicalDevices);
		PFN_vkVoidFunction GetInstanceProcAddr(string pName);
		Result CreateDisplayPlaneSurfaceKHR(DisplaySurfaceCreateInfoKHR createInfo, AllocationCallbacks allocator, out SurfaceKHR pSurface);
		void DestroySurfaceKHR(SurfaceKHR surface, AllocationCallbacks allocator);
		Result CreateWin32SurfaceKHR(Win32SurfaceCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out SurfaceKHR pSurface);
		Result CreateDebugReportCallbackEXT(DebugReportCallbackCreateInfoEXT pCreateInfo, AllocationCallbacks allocator, out DebugReportCallbackEXT pCallback);
		void DestroyDebugReportCallbackEXT(DebugReportCallbackEXT callback, AllocationCallbacks allocator);
		void DebugReportMessageEXT(DebugReportFlagBitsEXT flags, DebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
	}

	// Device
	public interface IDevice
	{
		PFN_vkVoidFunction GetDeviceProcAddr(string pName);
		void DestroyDevice(AllocationCallbacks allocator);
		void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IQueue pQueue);
		Result DeviceWaitIdle();
		Result AllocateMemory(MemoryAllocateInfo pAllocateInfo, AllocationCallbacks allocator, out DeviceMemory pMemory);
		void FreeMemory(DeviceMemory memory, AllocationCallbacks allocator);
		Result MapMemory(DeviceMemory memory, UInt64 offset, UInt64 size, UInt32 flags, IntPtr ppData);
		void UnmapMemory(DeviceMemory memory);
		Result FlushMappedMemoryRanges(MappedMemoryRange[] pMemoryRanges);
		Result InvalidateMappedMemoryRanges(MappedMemoryRange[] pMemoryRanges);
		void GetDeviceMemoryCommitment(DeviceMemory memory, ref UInt64 pCommittedMemoryInBytes);
		void GetBufferMemoryRequirements(Buffer buffer, out MemoryRequirements pMemoryRequirements);
		Result BindBufferMemory(Buffer buffer, DeviceMemory memory, UInt64 memoryOffset);
		void GetImageMemoryRequirements(Image image, out MemoryRequirements memoryRequirements);
		Result BindImageMemory(Image image, DeviceMemory memory, UInt64 memoryOffset);
		void GetImageSparseMemoryRequirements(Image image, out SparseImageMemoryRequirements[] sparseMemoryRequirements);
		Result CreateFence(FenceCreateInfo pCreateInfo, AllocationCallbacks allocator, out Fence fence);
		void DestroyFence(Fence fence, AllocationCallbacks allocator);
		Result ResetFences(Fence[] pFences);
		Result GetFenceStatus(Fence fence);
		Result WaitForFences(Fence[] pFences, bool waitAll, UInt64 timeout);
		Result CreateSemaphore(SemaphoreCreateInfo pCreateInfo, AllocationCallbacks allocator, out Semaphore pSemaphore);
		void DestroySemaphore(Semaphore semaphore, AllocationCallbacks allocator);
		Result CreateEvent(EventCreateInfo pCreateInfo, AllocationCallbacks allocator, out Event @event);
		void DestroyEvent(Event @event, AllocationCallbacks allocator);
		Result GetEventStatus(Event @event);
		Result SetEvent(Event @event);
		Result ResetEvent(Event @event);
		Result CreateQueryPool(QueryPoolCreateInfo pCreateInfo, AllocationCallbacks allocator, out QueryPool queryPool);
		void DestroyQueryPool(QueryPool queryPool, AllocationCallbacks allocator);
		Result GetQueryPoolResults(QueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, QueryResultFlagBits flags);
		Result CreateBuffer(BufferCreateInfo pCreateInfo, AllocationCallbacks allocator, out Buffer pBuffer);
		void DestroyBuffer(Buffer buffer, AllocationCallbacks allocator);
		Result CreateBufferView(BufferViewCreateInfo pCreateInfo, AllocationCallbacks allocator, out BufferView pView);
		void DestroyBufferView(BufferView bufferView, AllocationCallbacks allocator);
		Result CreateImage(ImageCreateInfo pCreateInfo, AllocationCallbacks allocator, out Image pImage);
		void DestroyImage(Image image, AllocationCallbacks allocator);
		void GetImageSubresourceLayout(Image image, ImageSubresource pSubresource, out SubresourceLayout pLayout);
		Result CreateImageView(ImageViewCreateInfo pCreateInfo, AllocationCallbacks allocator, out ImageView pView);
		void DestroyImageView(ImageView imageView, AllocationCallbacks allocator);
		Result CreateShaderModule(ShaderModuleCreateInfo pCreateInfo, AllocationCallbacks allocator, out ShaderModule pShaderModule);
		void DestroyShaderModule(ShaderModule shaderModule, AllocationCallbacks allocator);
		Result CreatePipelineCache(PipelineCacheCreateInfo pCreateInfo, AllocationCallbacks allocator, out PipelineCache pPipelineCache);
		void DestroyPipelineCache(PipelineCache pipelineCache, AllocationCallbacks allocator);
		Result GetPipelineCacheData(PipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData);
		Result MergePipelineCaches(PipelineCache dstCache, PipelineCache[] pSrcCaches);
		Result CreateGraphicsPipelines(PipelineCache pipelineCache, GraphicsPipelineCreateInfo[] pCreateInfos, AllocationCallbacks allocator, out Pipeline[] pPipelines);
		Result CreateComputePipelines(PipelineCache pipelineCache, ComputePipelineCreateInfo[] pCreateInfos, AllocationCallbacks allocator, out Pipeline[] pPipelines);
		void DestroyPipeline(Pipeline pipeline, AllocationCallbacks allocator);
		Result CreatePipelineLayout(PipelineLayoutCreateInfo pCreateInfo, AllocationCallbacks allocator, out PipelineLayout pPipelineLayout);
		void DestroyPipelineLayout(PipelineLayout pipelineLayout, AllocationCallbacks allocator);
		Result CreateSampler(SamplerCreateInfo pCreateInfo, AllocationCallbacks allocator, out Sampler pSampler);
		void DestroySampler(Sampler sampler, AllocationCallbacks allocator);
		Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo pCreateInfo, AllocationCallbacks allocator, out DescriptorSetLayout pSetLayout);
		void DestroyDescriptorSetLayout(DescriptorSetLayout descriptorSetLayout, AllocationCallbacks allocator);
		Result CreateDescriptorPool(DescriptorPoolCreateInfo pCreateInfo, AllocationCallbacks allocator, out DescriptorPool pDescriptorPool);
		void DestroyDescriptorPool(DescriptorPool descriptorPool, AllocationCallbacks allocator);
		Result ResetDescriptorPool(DescriptorPool descriptorPool, UInt32 flags);
		Result AllocateDescriptorSets(DescriptorSetAllocateInfo pAllocateInfo, DescriptorSet[] pDescriptorSets);
		Result FreeDescriptorSets(DescriptorPool descriptorPool, DescriptorSet[] pDescriptorSets);
		void UpdateDescriptorSets(UInt32 descriptorWriteCount, WriteDescriptorSet pDescriptorWrites, UInt32 descriptorCopyCount, CopyDescriptorSet pDescriptorCopies);
		Result CreateFramebuffer(FramebufferCreateInfo pCreateInfo, AllocationCallbacks allocator, out Framebuffer pFramebuffer);
		void DestroyFramebuffer(Framebuffer framebuffer, AllocationCallbacks allocator);
		Result CreateRenderPass(RenderPassCreateInfo pCreateInfo, AllocationCallbacks allocator, out RenderPass pRenderPass);
		void DestroyRenderPass(RenderPass renderPass, AllocationCallbacks allocator);
		void GetRenderAreaGranularity(RenderPass renderPass, out Extent2D pGranularity);
		Result CreateCommandPool(CommandPoolCreateInfo pCreateInfo, AllocationCallbacks allocator, out CommandPool pCommandPool);
		void DestroyCommandPool(CommandPool commandPool, AllocationCallbacks allocator);
		Result ResetCommandPool(CommandPool commandPool, CommandPoolResetFlagBits flags);
		Result AllocateCommandBuffers(CommandBufferAllocateInfo pAllocateInfo, ICommandBuffer[] pCommandBuffers);
		void FreeCommandBuffers(CommandPool commandPool, ICommandBuffer[] pCommandBuffers);
		Result CreateSharedSwapchainsKHR(SwapchainCreateInfoKHR[] pCreateInfos, AllocationCallbacks allocator, out SwapchainKHR[] pSwapchains);
		Result CreateSwapchainKHR(SwapchainCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out SwapchainKHR pSwapchain);
		void DestroySwapchainKHR(SwapchainKHR swapchain, AllocationCallbacks allocator);
		Result GetSwapchainImagesKHR(SwapchainKHR swapchain, out Image[] pSwapchainImages);
		Result AcquireNextImageKHR(SwapchainKHR swapchain, UInt64 timeout, Semaphore semaphore, Fence fence, out UInt32 pImageIndex);
	}

	// PhysicalDevice
	public interface IPhysicalDevice
	{
		void GetPhysicalDeviceProperties(out PhysicalDeviceProperties pProperties);
		void GetPhysicalDeviceQueueFamilyProperties(out QueueFamilyProperties[] pQueueFamilyProperties);
		void GetPhysicalDeviceMemoryProperties(out PhysicalDeviceMemoryProperties pMemoryProperties);
		void GetPhysicalDeviceFeatures(out PhysicalDeviceFeatures pFeatures);
		void GetPhysicalDeviceFormatProperties(Format format, out FormatProperties pFormatProperties);
		Result GetPhysicalDeviceImageFormatProperties(Format format, ImageType type, ImageTiling tiling, ImageUsageFlagBits usage, ImageCreateFlagBits flags, out ImageFormatProperties pImageFormatProperties);
		Result CreateDevice(DeviceCreateInfo pCreateInfo, AllocationCallbacks allocator, out IDevice pDevice);
		Result EnumerateDeviceLayerProperties(out LayerProperties[] pProperties);
		Result EnumerateDeviceExtensionProperties(string pLayerName, out ExtensionProperties[] pProperties);
		void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlagBits samples, ImageUsageFlagBits usage, ImageTiling tiling, out SparseImageFormatProperties[] pProperties);
		Result GetPhysicalDeviceDisplayPropertiesKHR(out DisplayPropertiesKHR[] pProperties);
		Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out DisplayPlanePropertiesKHR[] pProperties);
		Result GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out DisplayKHR[] pDisplays);
		Result GetDisplayModePropertiesKHR(DisplayKHR display, out DisplayModePropertiesKHR[] pProperties);
		//Result CreateDisplayModeKHR(DisplayKHR display, DisplayModeCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out DisplayModeKHR pMode);
		Result GetDisplayPlaneCapabilitiesKHR(DisplayModeKHR mode, UInt32 planeIndex, out DisplayPlaneCapabilitiesKHR pCapabilities);
		Result GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, SurfaceKHR surface, ref bool pSupported);
		Result GetPhysicalDeviceSurfaceCapabilitiesKHR(SurfaceKHR surface, out SurfaceCapabilitiesKHR pSurfaceCapabilities);
		Result GetPhysicalDeviceSurfaceFormatsKHR(SurfaceKHR surface, out SurfaceFormatKHR[] pSurfaceFormats);
		Result GetPhysicalDeviceSurfacePresentModesKHR(SurfaceKHR surface, out PresentModeKHR[] pPresentModes);
		bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex);
	}

	// Queue
	public interface IQueue
	{
		Result QueueSubmit(SubmitInfo[] pSubmits, Fence fence);
		Result QueueWaitIdle();
		Result QueueBindSparse(BindSparseInfo[] pBindInfo, Fence fence);
		Result QueuePresentKHR(PresentInfoKHR pPresentInfo);
	}

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
		void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, UInt32 firstSet, UInt32 descriptorSetCount, DescriptorSet[] pDescriptorSets, UInt32 dynamicOffsetCount, UInt32[] pDynamicOffsets);
		void CmdBindIndexBuffer(Buffer buffer, UInt64 offset, IndexType indexType);
		void CmdBindVertexBuffers(UInt32 firstBinding, UInt32 bindingCount, Buffer[] pBuffers, UInt64[] pOffsets);
		void CmdDraw(UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance);
		void CmdDrawIndexed(UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance);
		void CmdDrawIndirect(Buffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDrawIndexedIndirect(Buffer buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);
		void CmdDispatch(UInt32 x, UInt32 y, UInt32 z);
		void CmdDispatchIndirect(Buffer buffer, UInt64 offset);
		void CmdCopyBuffer(Buffer srcBuffer, Buffer dstBuffer, BufferCopy[] pRegions);
		void CmdCopyImage(Image srcImage, ImageLayout srcImageLayout, Image dstImage, ImageLayout dstImageLayout, ImageCopy[] pRegions);
		void CmdBlitImage(Image srcImage, ImageLayout srcImageLayout, Image dstImage, ImageLayout dstImageLayout, ImageBlit[] pRegions, Filter filter);
		void CmdCopyBufferToImage(Buffer srcBuffer, Image dstImage, ImageLayout dstImageLayout, BufferImageCopy[] pRegions);
		void CmdCopyImageToBuffer(Image srcImage, ImageLayout srcImageLayout, Buffer dstBuffer, BufferImageCopy[] pRegions);
		void CmdUpdateBuffer(Buffer dstBuffer, UInt64 dstOffset, UIntPtr dataSize, IntPtr pData);
		void CmdFillBuffer(Buffer dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);
		void CmdClearColorImage(Image image, ImageLayout imageLayout, ClearColorValue pColor, ImageSubresourceRange[] pRanges);
		void CmdClearDepthStencilImage(Image image, ImageLayout imageLayout, ClearDepthStencilValue pDepthStencil, ImageSubresourceRange[] pRanges);
		void CmdClearAttachments(ClearAttachment[] pAttachments, ClearRect[] pRects);
		void CmdResolveImage(Image srcImage, ImageLayout srcImageLayout, Image dstImage, ImageLayout dstImageLayout, ImageResolve[] pRegions);
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