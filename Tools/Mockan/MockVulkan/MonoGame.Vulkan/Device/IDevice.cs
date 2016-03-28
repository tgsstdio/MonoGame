using System;

namespace MonoGame.Graphics.Vk
{
	using VkImage = MonoGame.Graphics.Vk.Image;

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
		Result CreateImage(ImageCreateInfo pCreateInfo, AllocationCallbacks allocator, out VkImage pImage);
		void DestroyImage(VkImage image, AllocationCallbacks allocator);
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
		void UpdateDescriptorSets(WriteDescriptorSet[] pDescriptorWrites, CopyDescriptorSet[] pDescriptorCopies);
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
}

