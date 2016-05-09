using System;

namespace Magnesium.OpenGL
{
	public class GLDevice : IMgDevice
	{
		#region IMgDevice implementation
		public PFN_vkVoidFunction GetDeviceProcAddr (string pName)
		{
			throw new NotImplementedException ();
		}
		public void DestroyDevice (MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void GetDeviceQueue (uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			throw new NotImplementedException ();
		}
		public Result DeviceWaitIdle ()
		{
			throw new NotImplementedException ();
		}
		public Result AllocateMemory (MgMemoryAllocateInfo pAllocateInfo, MgAllocationCallbacks allocator, out MgDeviceMemory pMemory)
		{
			throw new NotImplementedException ();
		}
		public void FreeMemory (MgDeviceMemory memory, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result MapMemory (MgDeviceMemory memory, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			throw new NotImplementedException ();
		}
		public void UnmapMemory (MgDeviceMemory memory)
		{
			throw new NotImplementedException ();
		}
		public Result FlushMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public Result InvalidateMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public void GetDeviceMemoryCommitment (MgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			throw new NotImplementedException ();
		}
		public void GetBufferMemoryRequirements (MgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result BindBufferMemory (MgBuffer buffer, MgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException ();
		}
		public void GetImageMemoryRequirements (MgImage image, out MgMemoryRequirements memoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result BindImageMemory (MgImage image, MgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException ();
		}
		public void GetImageSparseMemoryRequirements (MgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result CreateFence (MgFenceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFence fence)
		{
			throw new NotImplementedException ();
		}
		public void DestroyFence (MgFence fence, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result ResetFences (MgFence[] pFences)
		{
			throw new NotImplementedException ();
		}
		public Result GetFenceStatus (MgFence fence)
		{
			throw new NotImplementedException ();
		}
		public Result WaitForFences (MgFence[] pFences, bool waitAll, ulong timeout)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSemaphore (MgSemaphoreCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgSemaphore pSemaphore)
		{
			throw new NotImplementedException ();
		}
		public void DestroySemaphore (MgSemaphore semaphore, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateEvent (MgEventCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public void DestroyEvent (MgEvent @event, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetEventStatus (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result SetEvent (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result ResetEvent (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result CreateQueryPool (MgQueryPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgQueryPool queryPool)
		{
			throw new NotImplementedException ();
		}
		public void DestroyQueryPool (MgQueryPool queryPool, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetQueryPoolResults (MgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}
		public Result CreateBuffer (MgBufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBuffer pBuffer)
		{
			throw new NotImplementedException ();
		}
		public void DestroyBuffer (MgBuffer buffer, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateBufferView (MgBufferViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBufferView pView)
		{
			throw new NotImplementedException ();
		}
		public void DestroyBufferView (MgBufferView bufferView, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateImage (MgImageCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgImage pImage)
		{
			throw new NotImplementedException ();
		}
		public void DestroyImage (MgImage image, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void GetImageSubresourceLayout (MgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			throw new NotImplementedException ();
		}
		public Result CreateImageView (MgImageViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgImageView pView)
		{
			throw new NotImplementedException ();
		}
		public void DestroyImageView (MgImageView imageView, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateShaderModule (MgShaderModuleCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgShaderModule pShaderModule)
		{
			throw new NotImplementedException ();
		}
		public void DestroyShaderModule (MgShaderModule shaderModule, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreatePipelineCache (MgPipelineCacheCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException ();
		}
		public void DestroyPipelineCache (MgPipelineCache pipelineCache, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetPipelineCacheData (MgPipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData)
		{
			throw new NotImplementedException ();
		}
		public Result MergePipelineCaches (MgPipelineCache dstCache, MgPipelineCache[] pSrcCaches)
		{
			throw new NotImplementedException ();
		}
		public Result CreateGraphicsPipelines (MgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out MgPipeline[] pPipelines)
		{
			throw new NotImplementedException ();
		}
		public Result CreateComputePipelines (MgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out MgPipeline[] pPipelines)
		{
			throw new NotImplementedException ();
		}
		public void DestroyPipeline (MgPipeline pipeline, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreatePipelineLayout (MgPipelineLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgPipelineLayout pPipelineLayout)
		{
			throw new NotImplementedException ();
		}
		public void DestroyPipelineLayout (MgPipelineLayout pipelineLayout, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSampler (MgSamplerCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgSampler pSampler)
		{
			throw new NotImplementedException ();
		}
		public void DestroySampler (MgSampler sampler, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgDescriptorSetLayout pSetLayout)
		{
			throw new NotImplementedException ();
		}
		public void DestroyDescriptorSetLayout (MgDescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateDescriptorPool (MgDescriptorPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgDescriptorPool pDescriptorPool)
		{
			throw new NotImplementedException ();
		}
		public void DestroyDescriptorPool (MgDescriptorPool descriptorPool, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result ResetDescriptorPool (MgDescriptorPool descriptorPool, uint flags)
		{
			throw new NotImplementedException ();
		}
		public Result AllocateDescriptorSets (MgDescriptorSetAllocateInfo pAllocateInfo, MgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException ();
		}
		public Result FreeDescriptorSets (MgDescriptorPool descriptorPool, MgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException ();
		}
		public void UpdateDescriptorSets (MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			throw new NotImplementedException ();
		}
		public Result CreateFramebuffer (MgFramebufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFramebuffer pFramebuffer)
		{
			throw new NotImplementedException ();
		}
		public void DestroyFramebuffer (MgFramebuffer framebuffer, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateRenderPass (MgRenderPassCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgRenderPass pRenderPass)
		{
			throw new NotImplementedException ();
		}
		public void DestroyRenderPass (MgRenderPass renderPass, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void GetRenderAreaGranularity (MgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			throw new NotImplementedException ();
		}
		public Result CreateCommandPool (MgCommandPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgCommandPool pCommandPool)
		{
			throw new NotImplementedException ();
		}
		public void DestroyCommandPool (MgCommandPool commandPool, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result ResetCommandPool (MgCommandPool commandPool, MgCommandPoolResetFlagBits flags)
		{
			throw new NotImplementedException ();
		}
		public Result AllocateCommandBuffers (MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}
		public void FreeCommandBuffers (MgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSharedSwapchainsKHR (MgSwapchainCreateInfoKHR[] pCreateInfos, MgAllocationCallbacks allocator, out MgSwapchainKHR[] pSwapchains)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSwapchainKHR (MgSwapchainCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSwapchainKHR pSwapchain)
		{
			throw new NotImplementedException ();
		}
		public void DestroySwapchainKHR (MgSwapchainKHR swapchain, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetSwapchainImagesKHR (MgSwapchainKHR swapchain, out MgImage[] pSwapchainImages)
		{
			throw new NotImplementedException ();
		}
		public Result AcquireNextImageKHR (MgSwapchainKHR swapchain, ulong timeout, MgSemaphore semaphore, MgFence fence, out uint pImageIndex)
		{
			throw new NotImplementedException ();
		}
		#endregion
		
	}
}

