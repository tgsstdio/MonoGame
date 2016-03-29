using System;

namespace MonoGame.Graphics
{
	using VkImage = MonoGame.Graphics.Image;

	// Device
	public interface IDevice
	{
		PFN_vkVoidFunction GetDeviceProcAddr(string pName);
		void DestroyDevice(MgAllocationCallbacks allocator);
		void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IQueue pQueue);
		Result DeviceWaitIdle();
		Result AllocateMemory(MemoryAllocateInfo pAllocateInfo, MgAllocationCallbacks allocator, out DeviceMemory pMemory);
		void FreeMemory(DeviceMemory memory, MgAllocationCallbacks allocator);
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
		Result CreateFence(FenceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Fence fence);
		void DestroyFence(Fence fence, MgAllocationCallbacks allocator);
		Result ResetFences(Fence[] pFences);
		Result GetFenceStatus(Fence fence);
		Result WaitForFences(Fence[] pFences, bool waitAll, UInt64 timeout);
		Result CreateSemaphore(SemaphoreCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Semaphore pSemaphore);
		void DestroySemaphore(Semaphore semaphore, MgAllocationCallbacks allocator);
		Result CreateEvent(EventCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Event @event);
		void DestroyEvent(Event @event, MgAllocationCallbacks allocator);
		Result GetEventStatus(Event @event);
		Result SetEvent(Event @event);
		Result ResetEvent(Event @event);
		Result CreateQueryPool(QueryPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out QueryPool queryPool);
		void DestroyQueryPool(QueryPool queryPool, MgAllocationCallbacks allocator);
		Result GetQueryPoolResults(QueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, QueryResultFlagBits flags);
		Result CreateBuffer(BufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Buffer pBuffer);
		void DestroyBuffer(Buffer buffer, MgAllocationCallbacks allocator);
		Result CreateBufferView(BufferViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out BufferView pView);
		void DestroyBufferView(BufferView bufferView, MgAllocationCallbacks allocator);
		Result CreateImage(ImageCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out VkImage pImage);
		void DestroyImage(VkImage image, MgAllocationCallbacks allocator);
		void GetImageSubresourceLayout(Image image, ImageSubresource pSubresource, out SubresourceLayout pLayout);
		Result CreateImageView(ImageViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out ImageView pView);
		void DestroyImageView(ImageView imageView, MgAllocationCallbacks allocator);
		Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out ShaderModule pShaderModule);
		void DestroyShaderModule(ShaderModule shaderModule, MgAllocationCallbacks allocator);
		Result CreatePipelineCache(PipelineCacheCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out PipelineCache pPipelineCache);
		void DestroyPipelineCache(PipelineCache pipelineCache, MgAllocationCallbacks allocator);
		Result GetPipelineCacheData(PipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData);
		Result MergePipelineCaches(PipelineCache dstCache, PipelineCache[] pSrcCaches);
		Result CreateGraphicsPipelines(PipelineCache pipelineCache, GraphicsPipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out Pipeline[] pPipelines);
		Result CreateComputePipelines(PipelineCache pipelineCache, ComputePipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out Pipeline[] pPipelines);
		void DestroyPipeline(Pipeline pipeline, MgAllocationCallbacks allocator);
		Result CreatePipelineLayout(PipelineLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out PipelineLayout pPipelineLayout);
		void DestroyPipelineLayout(PipelineLayout pipelineLayout, MgAllocationCallbacks allocator);
		Result CreateSampler(SamplerCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Sampler pSampler);
		void DestroySampler(Sampler sampler, MgAllocationCallbacks allocator);
		Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out DescriptorSetLayout pSetLayout);
		void DestroyDescriptorSetLayout(DescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator);
		Result CreateDescriptorPool(DescriptorPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out DescriptorPool pDescriptorPool);
		void DestroyDescriptorPool(DescriptorPool descriptorPool, MgAllocationCallbacks allocator);
		Result ResetDescriptorPool(DescriptorPool descriptorPool, UInt32 flags);
		Result AllocateDescriptorSets(DescriptorSetAllocateInfo pAllocateInfo, DescriptorSet[] pDescriptorSets);
		Result FreeDescriptorSets(DescriptorPool descriptorPool, DescriptorSet[] pDescriptorSets);
		void UpdateDescriptorSets(WriteDescriptorSet[] pDescriptorWrites, CopyDescriptorSet[] pDescriptorCopies);
		Result CreateFramebuffer(FramebufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out Framebuffer pFramebuffer);
		void DestroyFramebuffer(Framebuffer framebuffer, MgAllocationCallbacks allocator);
		Result CreateRenderPass(RenderPassCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out RenderPass pRenderPass);
		void DestroyRenderPass(RenderPass renderPass, MgAllocationCallbacks allocator);
		void GetRenderAreaGranularity(RenderPass renderPass, out Extent2D pGranularity);
		Result CreateCommandPool(CommandPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out CommandPool pCommandPool);
		void DestroyCommandPool(CommandPool commandPool, MgAllocationCallbacks allocator);
		Result ResetCommandPool(CommandPool commandPool, CommandPoolResetFlagBits flags);
		Result AllocateCommandBuffers(CommandBufferAllocateInfo pAllocateInfo, ICommandBuffer[] pCommandBuffers);
		void FreeCommandBuffers(CommandPool commandPool, ICommandBuffer[] pCommandBuffers);
		Result CreateSharedSwapchainsKHR(SwapchainCreateInfoKHR[] pCreateInfos, MgAllocationCallbacks allocator, out SwapchainKHR[] pSwapchains);
		Result CreateSwapchainKHR(SwapchainCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out SwapchainKHR pSwapchain);
		void DestroySwapchainKHR(SwapchainKHR swapchain, MgAllocationCallbacks allocator);
		Result GetSwapchainImagesKHR(SwapchainKHR swapchain, out Image[] pSwapchainImages);
		Result AcquireNextImageKHR(SwapchainKHR swapchain, UInt64 timeout, Semaphore semaphore, Fence fence, out UInt32 pImageIndex);
	}
}

