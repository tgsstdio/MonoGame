using System;

namespace MockVulkan 
{

	// STRUCTS 
	public class Offset2D
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
	}

	public class Offset3D
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public Int32 Z { get; set; }
	}

	public class Extent2D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
	}

	public class Extent3D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Depth { get; set; }
	}

	public class Viewport
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public float MinDepth { get; set; }
		public float MaxDepth { get; set; }
	}

	public class Rect2D
	{
		public Offset2D Offset { get; set; }
		public Extent2D Extent { get; set; }
	}

	public class Rect3D
	{
		public Offset3D Offset { get; set; }
		public Extent3D Extent { get; set; }
	}

	public class ClearRect
	{
		public Rect2D Rect { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}

	public class ComponentMapping
	{
		public ComponentSwizzle R { get; set; }
		public ComponentSwizzle G { get; set; }
		public ComponentSwizzle B { get; set; }
		public ComponentSwizzle A { get; set; }
	}

	public class PhysicalDeviceProperties
	{
		public UInt32 ApiVersion { get; set; }
		public UInt32 DriverVersion { get; set; }
		public UInt32 VendorID { get; set; }
		public UInt32 DeviceID { get; set; }
		public PhysicalDeviceType DeviceType { get; set; }
		public String DeviceName { get; set; }
		public Byte PipelineCacheUUID { get; set; }
		public PhysicalDeviceLimits Limits { get; set; }
		public PhysicalDeviceSparseProperties SparseProperties { get; set; }
	}

	public class ExtensionProperties
	{
		public String ExtensionName { get; set; }
		public UInt32 SpecVersion { get; set; }
	}

	public class LayerProperties
	{
		public String LayerName { get; set; }
		public UInt32 SpecVersion { get; set; }
		public UInt32 ImplementationVersion { get; set; }
		public String Description { get; set; }
	}

	public class ApplicationInfo
	{
		public String ApplicationName { get; set; }
		public UInt32 ApplicationVersion { get; set; }
		public String EngineName { get; set; }
		public UInt32 EngineVersion { get; set; }
		public UInt32 ApiVersion { get; set; }
	}

	public class AllocationCallbacks
	{
		public IntPtr UserData { get; set; }
		public PFN_vkAllocationFunction PfnAllocation { get; set; }
		public PFN_vkReallocationFunction PfnReallocation { get; set; }
		public PFN_vkFreeFunction PfnFree { get; set; }
		public PFN_vkInternalAllocationNotification PfnInternalAllocation { get; set; }
		public PFN_vkInternalFreeNotification PfnInternalFree { get; set; }
	}

	public class DeviceQueueCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
		public UInt32 QueueCount { get; set; }
		public float[] QueuePriorities { get; set; }
	}

	public class DeviceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 QueueCreateInfoCount { get; set; }
		public DeviceQueueCreateInfo[] QueueCreateInfos { get; set; }
		public UInt32 EnabledLayerCount { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public UInt32 EnabledExtensionCount { get; set; }
		public String[] EnabledExtensionNames { get; set; }
		public PhysicalDeviceFeatures EnabledFeatures { get; set; }
	}

	public class InstanceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public ApplicationInfo ApplicationInfo { get; set; }
		public UInt32 EnabledLayerCount { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public UInt32 EnabledExtensionCount { get; set; }
		public String[] EnabledExtensionNames { get; set; }
	}

	public class QueueFamilyProperties
	{
		public UInt32 QueueFlags { get; set; }
		public UInt32 QueueCount { get; set; }
		public UInt32 TimestampValidBits { get; set; }
		public Extent3D MinImageTransferGranularity { get; set; }
	}

	public class PhysicalDeviceMemoryProperties
	{
		public MemoryType[] MemoryTypes { get; set; }
		public MemoryHeap[] MemoryHeaps { get; set; }
	}

	public class MemoryAllocateInfo
	{
		public UInt64 AllocationSize { get; set; }
		public UInt32 MemoryTypeIndex { get; set; }
	}

	public class MemoryRequirements
	{
		public UInt64 Size { get; set; }
		public UInt64 Alignment { get; set; }
		public UInt32 MemoryTypeBits { get; set; }
	}

	public class SparseImageFormatProperties
	{
		public UInt32 AspectMask { get; set; }
		public Extent3D ImageGranularity { get; set; }
		public UInt32 Flags { get; set; }
	}

	public class SparseImageMemoryRequirements
	{
		public SparseImageFormatProperties FormatProperties { get; set; }
		public UInt32 ImageMipTailFirstLod { get; set; }
		public UInt64 ImageMipTailSize { get; set; }
		public UInt64 ImageMipTailOffset { get; set; }
		public UInt64 ImageMipTailStride { get; set; }
	}

	public class MemoryType
	{
		public UInt32 PropertyFlags { get; set; }
		public UInt32 HeapIndex { get; set; }
	}

	public class MemoryHeap
	{
		public UInt64 Size { get; set; }
		public UInt32 Flags { get; set; }
	}

	public class MappedMemoryRange
	{
		public DeviceMemory Memory { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}

	public class FormatProperties
	{
		public UInt32 LinearTilingFeatures { get; set; }
		public UInt32 OptimalTilingFeatures { get; set; }
		public UInt32 BufferFeatures { get; set; }
	}

	public class ImageFormatProperties
	{
		public Extent3D MaxExtent { get; set; }
		public UInt32 MaxMipLevels { get; set; }
		public UInt32 MaxArrayLayers { get; set; }
		public UInt32 SampleCounts { get; set; }
		public UInt64 MaxResourceSize { get; set; }
	}

	public class DescriptorBufferInfo
	{
		public Buffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

	public class DescriptorImageInfo
	{
		public Sampler Sampler { get; set; }
		public ImageView ImageView { get; set; }
		public ImageLayout ImageLayout { get; set; }
	}

	public class WriteDescriptorSet
	{
		public DescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public DescriptorType DescriptorType { get; set; }
		public DescriptorImageInfo[] ImageInfo { get; set; }
		public DescriptorBufferInfo[] BufferInfo { get; set; }
		public BufferView[] TexelBufferView { get; set; }
	}

	public class CopyDescriptorSet
	{
		public DescriptorSet SrcSet { get; set; }
		public UInt32 SrcBinding { get; set; }
		public UInt32 SrcArrayElement { get; set; }
		public DescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
	}

	public class BufferCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt64 Size { get; set; }
		public UInt32 Usage { get; set; }
		public SharingMode SharingMode { get; set; }
		public UInt32 QueueFamilyIndexCount { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
	}

	public class BufferViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Buffer Buffer { get; set; }
		public Format Format { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

	public class ImageSubresource
	{
		public UInt32 AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 ArrayLayer { get; set; }
	}

	public class ImageSubresourceLayers
	{
		public UInt32 AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}

	public class ImageSubresourceRange
	{
		public UInt32 AspectMask { get; set; }
		public UInt32 BaseMipLevel { get; set; }
		public UInt32 LevelCount { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}

	public class MemoryBarrier
	{
		public UInt32 SrcAccessMask { get; set; }
		public UInt32 DstAccessMask { get; set; }
	}

	public class BufferMemoryBarrier
	{
		public UInt32 SrcAccessMask { get; set; }
		public UInt32 DstAccessMask { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public Buffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}

	public class ImageMemoryBarrier
	{
		public UInt32 SrcAccessMask { get; set; }
		public UInt32 DstAccessMask { get; set; }
		public ImageLayout OldLayout { get; set; }
		public ImageLayout NewLayout { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public Image Image { get; set; }
		public ImageSubresourceRange SubresourceRange { get; set; }
	}

	public class ImageCreateInfo
	{
		public UInt32 Flags { get; set; }
		public ImageType ImageType { get; set; }
		public Format Format { get; set; }
		public Extent3D Extent { get; set; }
		public UInt32 MipLevels { get; set; }
		public UInt32 ArrayLayers { get; set; }
		public SampleCountFlagBits Samples { get; set; }
		public ImageTiling Tiling { get; set; }
		public UInt32 Usage { get; set; }
		public SharingMode SharingMode { get; set; }
		public UInt32 QueueFamilyIndexCount { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public ImageLayout InitialLayout { get; set; }
	}

	public class SubresourceLayout
	{
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
		public UInt64 RowPitch { get; set; }
		public UInt64 ArrayPitch { get; set; }
		public UInt64 DepthPitch { get; set; }
	}

	public class ImageViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Image Image { get; set; }
		public ImageViewType ViewType { get; set; }
		public Format Format { get; set; }
		public ComponentMapping Components { get; set; }
		public ImageSubresourceRange SubresourceRange { get; set; }
	}

	public class BufferCopy
	{
		public UInt64 SrcOffset { get; set; }
		public UInt64 DstOffset { get; set; }
		public UInt64 Size { get; set; }
	}

	public class SparseMemoryBind
	{
		public UInt64 ResourceOffset { get; set; }
		public UInt64 Size { get; set; }
		public DeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
		public SparseMemoryBindFlagBits Flags { get; set; }
	}

	public class SparseImageMemoryBind
	{
		public ImageSubresource Subresource { get; set; }
		public Offset3D Offset { get; set; }
		public Extent3D Extent { get; set; }
		public DeviceMemory Memory { get; set; }
		public UInt64 MemoryOffset { get; set; }
		public SparseMemoryBindFlagBits Flags { get; set; }
	}

	public class SparseBufferMemoryBindInfo
	{
		public Buffer Buffer { get; set; }
		public UInt32 BindCount { get; set; }
		public SparseMemoryBind[] Binds { get; set; }
	}

	public class SparseImageOpaqueMemoryBindInfo
	{
		public Image Image { get; set; }
		public UInt32 BindCount { get; set; }
		public SparseMemoryBind[] Binds { get; set; }
	}

	public class SparseImageMemoryBindInfo
	{
		public Image Image { get; set; }
		public UInt32 BindCount { get; set; }
		public SparseImageMemoryBind[] Binds { get; set; }
	}

	public class BindSparseInfo
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public Semaphore[] WaitSemaphores { get; set; }
		public UInt32 BufferBindCount { get; set; }
		public SparseBufferMemoryBindInfo[] BufferBinds { get; set; }
		public UInt32 ImageOpaqueBindCount { get; set; }
		public SparseImageOpaqueMemoryBindInfo[] ImageOpaqueBinds { get; set; }
		public UInt32 ImageBindCount { get; set; }
		public SparseImageMemoryBindInfo[] ImageBinds { get; set; }
		public UInt32 SignalSemaphoreCount { get; set; }
		public Semaphore[] SignalSemaphores { get; set; }
	}

	public class ImageCopy
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D SrcOffset { get; set; }
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D DstOffset { get; set; }
		public Extent3D Extent { get; set; }
	}

	public class ImageBlit
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D[] srcOffsets { get; set; } // 2
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D[] dstOffsets { get; set; } // 2
	}

	public class BufferImageCopy
	{
		public UInt64 BufferOffset { get; set; }
		public UInt32 BufferRowLength { get; set; }
		public UInt32 BufferImageHeight { get; set; }
		public ImageSubresourceLayers ImageSubresource { get; set; }
		public Offset3D ImageOffset { get; set; }
		public Extent3D ImageExtent { get; set; }
	}

	public class ImageResolve
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D SrcOffset { get; set; }
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D DstOffset { get; set; }
		public Extent3D Extent { get; set; }
	}

	public class ShaderModuleCreateInfo
	{
		public UInt32 Flags { get; set; }
		public IntPtr CodeSize { get; set; }
		public UInt32[] Code { get; set; }
	}

	public class DescriptorSetLayoutBinding
	{
		public UInt32 Binding { get; set; }
		public DescriptorType DescriptorType { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public UInt32 StageFlags { get; set; }
		public Sampler[] ImmutableSamplers { get; set; }
	}

	public class DescriptorSetLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 BindingCount { get; set; }
		public DescriptorSetLayoutBinding[] Bindings { get; set; }
	}

	public class DescriptorPoolSize
	{
		public DescriptorType Type { get; set; }
		public UInt32 DescriptorCount { get; set; }
	}

	public class DescriptorPoolCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 MaxSets { get; set; }
		public UInt32 PoolSizeCount { get; set; }
		public DescriptorPoolSize[] PoolSizes { get; set; }
	}

	public class DescriptorSetAllocateInfo
	{
		public DescriptorPool DescriptorPool { get; set; }
		public UInt32 DescriptorSetCount { get; set; }
		public DescriptorSetLayout[] SetLayouts { get; set; }
	}

	public class SpecializationMapEntry
	{
		public UInt32 ConstantID { get; set; }
		public UInt32 Offset { get; set; }
		public IntPtr Size { get; set; }
	}

	public class SpecializationInfo
	{
		public UInt32 MapEntryCount { get; set; }
		public SpecializationMapEntry[] MapEntries { get; set; }
		public IntPtr DataSize { get; set; }
		public IntPtr[] Data { get; set; }
	}

	public class PipelineShaderStageCreateInfo
	{
		public UInt32 Flags { get; set; }
		public ShaderStageFlagBits Stage { get; set; }
		public ShaderModule Module { get; set; }
		public String Name { get; set; }
		public SpecializationInfo SpecializationInfo { get; set; }
	}

	public class ComputePipelineCreateInfo
	{
		public UInt32 Flags { get; set; }
		public PipelineShaderStageCreateInfo Stage { get; set; }
		public PipelineLayout Layout { get; set; }
		public Pipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}

	public class VertexInputBindingDescription
	{
		public UInt32 Binding { get; set; }
		public UInt32 Stride { get; set; }
		public VertexInputRate InputRate { get; set; }
	}

	public class VertexInputAttributeDescription
	{
		public UInt32 Location { get; set; }
		public UInt32 Binding { get; set; }
		public Format Format { get; set; }
		public UInt32 Offset { get; set; }
	}

	public class PipelineVertexInputStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 VertexBindingDescriptionCount { get; set; }
		public VertexInputBindingDescription[] VertexBindingDescriptions { get; set; }
		public UInt32 VertexAttributeDescriptionCount { get; set; }
		public VertexInputAttributeDescription[] VertexAttributeDescriptions { get; set; }
	}

	public class PipelineInputAssemblyStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public PrimitiveTopology Topology { get; set; }
		public bool PrimitiveRestartEnable { get; set; }
	}

	public class PipelineTessellationStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 PatchControlPoints { get; set; }
	}

	public class PipelineViewportStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 ViewportCount { get; set; }
		public Viewport[] Viewports { get; set; }
		public UInt32 ScissorCount { get; set; }
		public Rect2D[] Scissors { get; set; }
	}

	public class PipelineRasterizationStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthClampEnable { get; set; }
		public bool RasterizerDiscardEnable { get; set; }
		public PolygonMode PolygonMode { get; set; }
		public UInt32 CullMode { get; set; }
		public FrontFace FrontFace { get; set; }
		public bool DepthBiasEnable { get; set; }
		public float DepthBiasConstantFactor { get; set; }
		public float DepthBiasClamp { get; set; }
		public float DepthBiasSlopeFactor { get; set; }
		public float LineWidth { get; set; }
	}

	public class PipelineMultisampleStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public SampleCountFlagBits RasterizationSamples { get; set; }
		public bool SampleShadingEnable { get; set; }
		public float MinSampleShading { get; set; }
		public UInt32[] SampleMask { get; set; }
		public bool AlphaToCoverageEnable { get; set; }
		public bool AlphaToOneEnable { get; set; }
	}

	public class PipelineColorBlendAttachmentState
	{
		public bool BlendEnable { get; set; }
		public BlendFactor SrcColorBlendFactor { get; set; }
		public BlendFactor DstColorBlendFactor { get; set; }
		public BlendOp ColorBlendOp { get; set; }
		public BlendFactor SrcAlphaBlendFactor { get; set; }
		public BlendFactor DstAlphaBlendFactor { get; set; }
		public BlendOp AlphaBlendOp { get; set; }
		public UInt32 ColorWriteMask { get; set; }
	}

	public class PipelineColorBlendStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool LogicOpEnable { get; set; }
		public LogicOp LogicOp { get; set; }
		public UInt32 AttachmentCount { get; set; }
		public PipelineColorBlendAttachmentState[] Attachments { get; set; }
		public float[] BlendConstants { get; set; } // 4
	}

	public class PipelineDynamicStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 DynamicStateCount { get; set; }
		public DynamicState[] DynamicStates { get; set; }
	}

	public class StencilOpState
	{
		public StencilOp FailOp { get; set; }
		public StencilOp PassOp { get; set; }
		public StencilOp DepthFailOp { get; set; }
		public CompareOp CompareOp { get; set; }
		public UInt32 CompareMask { get; set; }
		public UInt32 WriteMask { get; set; }
		public UInt32 Reference { get; set; }
	}

	public class PipelineDepthStencilStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthTestEnable { get; set; }
		public bool DepthWriteEnable { get; set; }
		public CompareOp DepthCompareOp { get; set; }
		public bool DepthBoundsTestEnable { get; set; }
		public bool StencilTestEnable { get; set; }
		public StencilOpState Front { get; set; }
		public StencilOpState Back { get; set; }
		public float MinDepthBounds { get; set; }
		public float MaxDepthBounds { get; set; }
	}

	public class GraphicsPipelineCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 StageCount { get; set; }
		public PipelineShaderStageCreateInfo[] Stages { get; set; }
		public PipelineVertexInputStateCreateInfo VertexInputState { get; set; }
		public PipelineInputAssemblyStateCreateInfo InputAssemblyState { get; set; }
		public PipelineTessellationStateCreateInfo TessellationState { get; set; }
		public PipelineViewportStateCreateInfo ViewportState { get; set; }
		public PipelineRasterizationStateCreateInfo RasterizationState { get; set; }
		public PipelineMultisampleStateCreateInfo MultisampleState { get; set; }
		public PipelineDepthStencilStateCreateInfo DepthStencilState { get; set; }
		public PipelineColorBlendStateCreateInfo ColorBlendState { get; set; }
		public PipelineDynamicStateCreateInfo DynamicState { get; set; }
		public PipelineLayout Layout { get; set; }
		public RenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public Pipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}

	public class PipelineCacheCreateInfo
	{
		public UInt32 Flags { get; set; }
		public IntPtr InitialDataSize { get; set; }
		public IntPtr[] InitialData { get; set; }
	}

	public class PushConstantRange
	{
		public UInt32 StageFlags { get; set; }
		public UInt32 Offset { get; set; }
		public UInt32 Size { get; set; }
	}

	public class PipelineLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 SetLayoutCount { get; set; }
		public DescriptorSetLayout[] SetLayouts { get; set; }
		public UInt32 PushConstantRangeCount { get; set; }
		public PushConstantRange[] PushConstantRanges { get; set; }
	}

	public class SamplerCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Filter MagFilter { get; set; }
		public Filter MinFilter { get; set; }
		public SamplerMipmapMode MipmapMode { get; set; }
		public SamplerAddressMode AddressModeU { get; set; }
		public SamplerAddressMode AddressModeV { get; set; }
		public SamplerAddressMode AddressModeW { get; set; }
		public float MipLodBias { get; set; }
		public bool AnisotropyEnable { get; set; }
		public float MaxAnisotropy { get; set; }
		public bool CompareEnable { get; set; }
		public CompareOp CompareOp { get; set; }
		public float MinLod { get; set; }
		public float MaxLod { get; set; }
		public BorderColor BorderColor { get; set; }
		public bool UnnormalizedCoordinates { get; set; }
	}

	public class CommandPoolCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
	}

	public class CommandBufferAllocateInfo
	{
		public CommandPool CommandPool { get; set; }
		public CommandBufferLevel Level { get; set; }
		public UInt32 CommandBufferCount { get; set; }
	}

	public class CommandBufferInheritanceInfo
	{
		public RenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public Framebuffer Framebuffer { get; set; }
		public bool OcclusionQueryEnable { get; set; }
		public UInt32 QueryFlags { get; set; }
		public UInt32 PipelineStatistics { get; set; }
	}

	public class CommandBufferBeginInfo
	{
		public UInt32 Flags { get; set; }
		public CommandBufferInheritanceInfo InheritanceInfo { get; set; }
	}

	public class RenderPassBeginInfo
	{
		public RenderPass RenderPass { get; set; }
		public Framebuffer Framebuffer { get; set; }
		public Rect2D RenderArea { get; set; }
		public UInt32 ClearValueCount { get; set; }
		public ClearValue[] ClearValues { get; set; }
	}

	public class ClearDepthStencilValue
	{
		public float Depth { get; set; }
		public UInt32 Stencil { get; set; }
	}

	public class ClearAttachment
	{
		public UInt32 AspectMask { get; set; }
		public UInt32 ColorAttachment { get; set; }
		public ClearValue ClearValue { get; set; }
	}

	public class AttachmentDescription
	{
		public UInt32 Flags { get; set; }
		public Format Format { get; set; }
		public SampleCountFlagBits Samples { get; set; }
		public AttachmentLoadOp LoadOp { get; set; }
		public AttachmentStoreOp StoreOp { get; set; }
		public AttachmentLoadOp StencilLoadOp { get; set; }
		public AttachmentStoreOp StencilStoreOp { get; set; }
		public ImageLayout InitialLayout { get; set; }
		public ImageLayout FinalLayout { get; set; }
	}

	public class AttachmentReference
	{
		public UInt32 Attachment { get; set; }
		public ImageLayout Layout { get; set; }
	}

	public class SubpassDescription
	{
		public UInt32 Flags { get; set; }
		public PipelineBindPoint PipelineBindPoint { get; set; }
		public UInt32 InputAttachmentCount { get; set; }
		public AttachmentReference[] InputAttachments { get; set; }
		public UInt32 ColorAttachmentCount { get; set; }
		public AttachmentReference[] ColorAttachments { get; set; }
		public AttachmentReference[] ResolveAttachments { get; set; }
		public AttachmentReference DepthStencilAttachment { get; set; }
		public UInt32 PreserveAttachmentCount { get; set; }
		public UInt32[] PreserveAttachments { get; set; }
	}

	public class SubpassDependency
	{
		public UInt32 SrcSubpass { get; set; }
		public UInt32 DstSubpass { get; set; }
		public UInt32 SrcStageMask { get; set; }
		public UInt32 DstStageMask { get; set; }
		public UInt32 SrcAccessMask { get; set; }
		public UInt32 DstAccessMask { get; set; }
		public UInt32 DependencyFlags { get; set; }
	}

	public class RenderPassCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 AttachmentCount { get; set; }
		public AttachmentDescription[] Attachments { get; set; }
		public UInt32 SubpassCount { get; set; }
		public SubpassDescription[] Subpasses { get; set; }
		public UInt32 DependencyCount { get; set; }
		public SubpassDependency[] Dependencies { get; set; }
	}

	public class EventCreateInfo
	{
		public UInt32 Flags { get; set; }
	}

	public class FenceCreateInfo
	{
		public UInt32 Flags { get; set; }
	}

	public class PhysicalDeviceFeatures
	{
		public bool RobustBufferAccess { get; set; }
		public bool FullDrawIndexUint32 { get; set; }
		public bool ImageCubeArray { get; set; }
		public bool IndependentBlend { get; set; }
		public bool GeometryShader { get; set; }
		public bool TessellationShader { get; set; }
		public bool SampleRateShading { get; set; }
		public bool DualSrcBlend { get; set; }
		public bool LogicOp { get; set; }
		public bool MultiDrawIndirect { get; set; }
		public bool DrawIndirectFirstInstance { get; set; }
		public bool DepthClamp { get; set; }
		public bool DepthBiasClamp { get; set; }
		public bool FillModeNonSolid { get; set; }
		public bool DepthBounds { get; set; }
		public bool WideLines { get; set; }
		public bool LargePoints { get; set; }
		public bool AlphaToOne { get; set; }
		public bool MultiViewport { get; set; }
		public bool SamplerAnisotropy { get; set; }
		public bool TextureCompressionETC2 { get; set; }
		public bool TextureCompressionASTC_LDR { get; set; }
		public bool TextureCompressionBC { get; set; }
		public bool OcclusionQueryPrecise { get; set; }
		public bool PipelineStatisticsQuery { get; set; }
		public bool VertexPipelineStoresAndAtomics { get; set; }
		public bool FragmentStoresAndAtomics { get; set; }
		public bool ShaderTessellationAndGeometryPointSize { get; set; }
		public bool ShaderImageGatherExtended { get; set; }
		public bool ShaderStorageImageExtendedFormats { get; set; }
		public bool ShaderStorageImageMultisample { get; set; }
		public bool ShaderStorageImageReadWithoutFormat { get; set; }
		public bool ShaderStorageImageWriteWithoutFormat { get; set; }
		public bool ShaderUniformBufferArrayDynamicIndexing { get; set; }
		public bool ShaderSampledImageArrayDynamicIndexing { get; set; }
		public bool ShaderStorageBufferArrayDynamicIndexing { get; set; }
		public bool ShaderStorageImageArrayDynamicIndexing { get; set; }
		public bool ShaderClipDistance { get; set; }
		public bool ShaderCullDistance { get; set; }
		public bool ShaderFloat64 { get; set; }
		public bool ShaderInt64 { get; set; }
		public bool ShaderInt16 { get; set; }
		public bool ShaderResourceResidency { get; set; }
		public bool ShaderResourceMinLod { get; set; }
		public bool SparseBinding { get; set; }
		public bool SparseResidencyBuffer { get; set; }
		public bool SparseResidencyImage2D { get; set; }
		public bool SparseResidencyImage3D { get; set; }
		public bool SparseResidency2Samples { get; set; }
		public bool SparseResidency4Samples { get; set; }
		public bool SparseResidency8Samples { get; set; }
		public bool SparseResidency16Samples { get; set; }
		public bool SparseResidencyAliased { get; set; }
		public bool VariableMultisampleRate { get; set; }
		public bool InheritedQueries { get; set; }
	}

	public class PhysicalDeviceSparseProperties
	{
		public bool ResidencyStandard2DBlockShape { get; set; }
		public bool ResidencyStandard2DMultisampleBlockShape { get; set; }
		public bool ResidencyStandard3DBlockShape { get; set; }
		public bool ResidencyAlignedMipSize { get; set; }
		public bool ResidencyNonResidentStrict { get; set; }
	}

	public class PhysicalDeviceLimits
	{
		public UInt32 MaxImageDimension1D { get; set; }
		public UInt32 MaxImageDimension2D { get; set; }
		public UInt32 MaxImageDimension3D { get; set; }
		public UInt32 MaxImageDimensionCube { get; set; }
		public UInt32 MaxImageArrayLayers { get; set; }
		public UInt32 MaxTexelBufferElements { get; set; }
		public UInt32 MaxUniformBufferRange { get; set; }
		public UInt32 MaxStorageBufferRange { get; set; }
		public UInt32 MaxPushConstantsSize { get; set; }
		public UInt32 MaxMemoryAllocationCount { get; set; }
		public UInt32 MaxSamplerAllocationCount { get; set; }
		public UInt64 BufferImageGranularity { get; set; }
		public UInt64 SparseAddressSpaceSize { get; set; }
		public UInt32 MaxBoundDescriptorSets { get; set; }
		public UInt32 MaxPerStageDescriptorSamplers { get; set; }
		public UInt32 MaxPerStageDescriptorUniformBuffers { get; set; }
		public UInt32 MaxPerStageDescriptorStorageBuffers { get; set; }
		public UInt32 MaxPerStageDescriptorSampledImages { get; set; }
		public UInt32 MaxPerStageDescriptorStorageImages { get; set; }
		public UInt32 MaxPerStageDescriptorInputAttachments { get; set; }
		public UInt32 MaxPerStageResources { get; set; }
		public UInt32 MaxDescriptorSetSamplers { get; set; }
		public UInt32 MaxDescriptorSetUniformBuffers { get; set; }
		public UInt32 MaxDescriptorSetUniformBuffersDynamic { get; set; }
		public UInt32 MaxDescriptorSetStorageBuffers { get; set; }
		public UInt32 MaxDescriptorSetStorageBuffersDynamic { get; set; }
		public UInt32 MaxDescriptorSetSampledImages { get; set; }
		public UInt32 MaxDescriptorSetStorageImages { get; set; }
		public UInt32 MaxDescriptorSetInputAttachments { get; set; }
		public UInt32 MaxVertexInputAttributes { get; set; }
		public UInt32 MaxVertexInputBindings { get; set; }
		public UInt32 MaxVertexInputAttributeOffset { get; set; }
		public UInt32 MaxVertexInputBindingStride { get; set; }
		public UInt32 MaxVertexOutputComponents { get; set; }
		public UInt32 MaxTessellationGenerationLevel { get; set; }
		public UInt32 MaxTessellationPatchSize { get; set; }
		public UInt32 MaxTessellationControlPerVertexInputComponents { get; set; }
		public UInt32 MaxTessellationControlPerVertexOutputComponents { get; set; }
		public UInt32 MaxTessellationControlPerPatchOutputComponents { get; set; }
		public UInt32 MaxTessellationControlTotalOutputComponents { get; set; }
		public UInt32 MaxTessellationEvaluationInputComponents { get; set; }
		public UInt32 MaxTessellationEvaluationOutputComponents { get; set; }
		public UInt32 MaxGeometryShaderInvocations { get; set; }
		public UInt32 MaxGeometryInputComponents { get; set; }
		public UInt32 MaxGeometryOutputComponents { get; set; }
		public UInt32 MaxGeometryOutputVertices { get; set; }
		public UInt32 MaxGeometryTotalOutputComponents { get; set; }
		public UInt32 MaxFragmentInputComponents { get; set; }
		public UInt32 MaxFragmentOutputAttachments { get; set; }
		public UInt32 MaxFragmentDualSrcAttachments { get; set; }
		public UInt32 MaxFragmentCombinedOutputResources { get; set; }
		public UInt32 MaxComputeSharedMemorySize { get; set; }
		public UInt32[] maxComputeWorkGroupCount { get; set; } //  3
		public UInt32 MaxComputeWorkGroupInvocations { get; set; }
		public UInt32[] maxComputeWorkGroupSize  { get; set; } // 3
		public UInt32 SubPixelPrecisionBits { get; set; }
		public UInt32 SubTexelPrecisionBits { get; set; }
		public UInt32 MipmapPrecisionBits { get; set; }
		public UInt32 MaxDrawIndexedIndexValue { get; set; }
		public UInt32 MaxDrawIndirectCount { get; set; }
		public float MaxSamplerLodBias { get; set; }
		public float MaxSamplerAnisotropy { get; set; }
		public UInt32 MaxViewports { get; set; }
		public UInt32[] maxViewportDimensions { get; set; } // 2
		public UInt32[] viewportBoundsRange { get; set; } // 2
		public UInt32 ViewportSubPixelBits { get; set; }
		public IntPtr MinMemoryMapAlignment { get; set; }
		public UInt64 MinTexelBufferOffsetAlignment { get; set; }
		public UInt64 MinUniformBufferOffsetAlignment { get; set; }
		public UInt64 MinStorageBufferOffsetAlignment { get; set; }
		public Int32 MinTexelOffset { get; set; }
		public UInt32 MaxTexelOffset { get; set; }
		public Int32 MinTexelGatherOffset { get; set; }
		public UInt32 MaxTexelGatherOffset { get; set; }
		public float MinInterpolationOffset { get; set; }
		public float MaxInterpolationOffset { get; set; }
		public UInt32 SubPixelInterpolationOffsetBits { get; set; }
		public UInt32 MaxFramebufferWidth { get; set; }
		public UInt32 MaxFramebufferHeight { get; set; }
		public UInt32 MaxFramebufferLayers { get; set; }
		public UInt32 FramebufferColorSampleCounts { get; set; }
		public UInt32 FramebufferDepthSampleCounts { get; set; }
		public UInt32 FramebufferStencilSampleCounts { get; set; }
		public UInt32 FramebufferNoAttachmentsSampleCounts { get; set; }
		public UInt32 MaxColorAttachments { get; set; }
		public UInt32 SampledImageColorSampleCounts { get; set; }
		public UInt32 SampledImageIntegerSampleCounts { get; set; }
		public UInt32 SampledImageDepthSampleCounts { get; set; }
		public UInt32 SampledImageStencilSampleCounts { get; set; }
		public UInt32 StorageImageSampleCounts { get; set; }
		public UInt32 MaxSampleMaskWords { get; set; }
		public bool TimestampComputeAndGraphics { get; set; }
		public float TimestampPeriod { get; set; }
		public UInt32 MaxClipDistances { get; set; }
		public UInt32 MaxCullDistances { get; set; }
		public UInt32 MaxCombinedClipAndCullDistances { get; set; }
		public UInt32 DiscreteQueuePriorities { get; set; }
		public float[] pointSizeRange  { get; set; } // 2
		public float[] lineWidthRange  { get; set; } // 2
		public float PointSizeGranularity { get; set; }
		public float LineWidthGranularity { get; set; }
		public bool StrictLines { get; set; }
		public bool StandardSampleLocations { get; set; }
		public UInt64 OptimalBufferCopyOffsetAlignment { get; set; }
		public UInt64 OptimalBufferCopyRowPitchAlignment { get; set; }
		public UInt64 NonCoherentAtomSize { get; set; }
	}

	public class SemaphoreCreateInfo
	{
		public UInt32 Flags { get; set; }
	}

	public class QueryPoolCreateInfo
	{
		public UInt32 Flags { get; set; }
		public QueryType QueryType { get; set; }
		public UInt32 QueryCount { get; set; }
		public UInt32 PipelineStatistics { get; set; }
	}

	public class FramebufferCreateInfo
	{
		public UInt32 Flags { get; set; }
		public RenderPass RenderPass { get; set; }
		public UInt32 AttachmentCount { get; set; }
		public ImageView[] Attachments { get; set; }
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Layers { get; set; }
	}

	public class DrawIndirectCommand
	{
		public UInt32 VertexCount { get; set; }
		public UInt32 InstanceCount { get; set; }
		public UInt32 FirstVertex { get; set; }
		public UInt32 FirstInstance { get; set; }
	}

	public class DrawIndexedIndirectCommand
	{
		public UInt32 IndexCount { get; set; }
		public UInt32 InstanceCount { get; set; }
		public UInt32 FirstIndex { get; set; }
		public Int32 VertexOffset { get; set; }
		public UInt32 FirstInstance { get; set; }
	}

	public class DispatchIndirectCommand
	{
		public UInt32 X { get; set; }
		public UInt32 Y { get; set; }
		public UInt32 Z { get; set; }
	}

	public class SubmitInfo
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public Semaphore[] WaitSemaphores { get; set; }
		public PipelineStageFlagBits[] WaitDstStageMask { get; set; }
		public UInt32 CommandBufferCount { get; set; }
		public ICommandBuffer[] CommandBuffers { get; set; }
		public UInt32 SignalSemaphoreCount { get; set; }
		public Semaphore[] SignalSemaphores { get; set; }
	}

	public class DisplayPropertiesKHR
	{
		public DisplayKHR Display { get; set; }
		public String DisplayName { get; set; }
		public Extent2D PhysicalDimensions { get; set; }
		public Extent2D PhysicalResolution { get; set; }
		public UInt32 SupportedTransforms { get; set; }
		public bool PlaneReorderPossible { get; set; }
		public bool PersistentContent { get; set; }
	}

	public class DisplayPlanePropertiesKHR
	{
		public DisplayKHR CurrentDisplay { get; set; }
		public UInt32 CurrentStackIndex { get; set; }
	}

	public class DisplayModeParametersKHR
	{
		public Extent2D VisibleRegion { get; set; }
		public UInt32 RefreshRate { get; set; }
	}

	public class DisplayModePropertiesKHR
	{
		public DisplayModeKHR DisplayMode { get; set; }
		public DisplayModeParametersKHR Parameters { get; set; }
	}

	public class DisplayModeCreatefInfoKHR
	{
		public UInt32 Flags { get; set; }
		public DisplayModeParametersKHR Parameters { get; set; }
	}

	public class DisplayPlaneCapabilitiesKHR
	{
		public UInt32 SupportedAlpha { get; set; }
		public Offset2D MinSrcPosition { get; set; }
		public Offset2D MaxSrcPosition { get; set; }
		public Extent2D MinSrcExtent { get; set; }
		public Extent2D MaxSrcExtent { get; set; }
		public Offset2D MinDstPosition { get; set; }
		public Offset2D MaxDstPosition { get; set; }
		public Extent2D MinDstExtent { get; set; }
		public Extent2D MaxDstExtent { get; set; }
	}

	public class DisplaySurfaceCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public DisplayModeKHR DisplayMode { get; set; }
		public UInt32 PlaneIndex { get; set; }
		public UInt32 PlaneStackIndex { get; set; }
		public SurfaceTransformFlagBitsKHR Transform { get; set; }
		public float GlobalAlpha { get; set; }
		public DisplayPlaneAlphaFlagBitsKHR AlphaMode { get; set; }
		public Extent2D ImageExtent { get; set; }
	}

	public class DisplayPresentInfoKHR
	{
		public Rect2D SrcRect { get; set; }
		public Rect2D DstRect { get; set; }
		public bool Persistent { get; set; }
	}

	public class SurfaceCapabilitiesKHR
	{
		public UInt32 MinImageCount { get; set; }
		public UInt32 MaxImageCount { get; set; }
		public Extent2D CurrentExtent { get; set; }
		public Extent2D MinImageExtent { get; set; }
		public Extent2D MaxImageExtent { get; set; }
		public UInt32 MaxImageArrayLayers { get; set; }
		public UInt32 SupportedTransforms { get; set; }
		public SurfaceTransformFlagBitsKHR CurrentTransform { get; set; }
		public UInt32 SupportedCompositeAlpha { get; set; }
		public UInt32 SupportedUsageFlags { get; set; }
	}

	public class Win32SurfaceCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public IntPtr Hinstance { get; set; }
		public IntPtr Hwnd { get; set; }
	}

	public class SurfaceFormatKHR
	{
		public Format Format { get; set; }
		public ColorSpaceKHR ColorSpace { get; set; }
	}

	public class SwapchainCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public SurfaceKHR Surface { get; set; }
		public UInt32 MinImageCount { get; set; }
		public Format ImageFormat { get; set; }
		public ColorSpaceKHR ImageColorSpace { get; set; }
		public Extent2D ImageExtent { get; set; }
		public UInt32 ImageArrayLayers { get; set; }
		public UInt32 ImageUsage { get; set; }
		public SharingMode ImageSharingMode { get; set; }
		public UInt32 QueueFamilyIndexCount { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public SurfaceTransformFlagBitsKHR PreTransform { get; set; }
		public CompositeAlphaFlagBitsKHR CompositeAlpha { get; set; }
		public PresentModeKHR PresentMode { get; set; }
		public bool Clipped { get; set; }
		public SwapchainKHR OldSwapchain { get; set; }
	}

	public class PresentInfoKHR
	{
		public UInt32 WaitSemaphoreCount { get; set; }
		public Semaphore[] WaitSemaphores { get; set; }
		public UInt32 SwapchainCount { get; set; }
		public SwapchainKHR[] Swapchains { get; set; }
		public UInt32[] ImageIndices { get; set; }
		public Result[] Results { get; set; }
	}

	public class DebugReportCallbackCreateInfoEXT
	{
		public UInt32 Flags { get; set; }
		public PFN_vkDebugReportCallbackEXT PfnCallback { get; set; }
		public IntPtr UserData { get; set; }
	}


	// DELEGATES
	public delegate void PFN_vkInternalAllocationNotification(IntPtr pUserData, IntPtr size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);

	public delegate void PFN_vkInternalFreeNotification(IntPtr pUserData, IntPtr size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);

	public delegate void PFN_vkReallocationFunction(IntPtr pUserData, IntPtr pOriginal, IntPtr size, IntPtr alignment, SystemAllocationScope allocationScope);

	public delegate void PFN_vkAllocationFunction(IntPtr pUserData, IntPtr size, IntPtr alignment, SystemAllocationScope allocationScope);

	public delegate void PFN_vkFreeFunction(IntPtr pUserData, IntPtr pMemory);

	public delegate void PFN_vkVoidFunction();

	public delegate void PFN_vkDebugReportCallbackEXT(DebugReportFlagBitsEXT flags, DebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, String pLayerPrefix, String pMessage, IntPtr pUserData);
}