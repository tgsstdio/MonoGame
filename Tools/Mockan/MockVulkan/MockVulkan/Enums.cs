using System;

namespace Magnesium 
{
	// ENUMS

	public enum ImageLayout : byte
	{
		// Implicit layout an image is when its contents are undefined due to various reasons (e.g. right after creation)
		UNDEFINED = 0,
		// General layout when image can be used for any kind of access
		GENERAL = 1,
		// Optimal layout when image is only used for color attachment read/write
		COLOR_ATTACHMENT_OPTIMAL = 2,
		// Optimal layout when image is only used for depth/stencil attachment read/write
		DEPTH_STENCIL_ATTACHMENT_OPTIMAL = 3,
		// Optimal layout when image is used for read only depth/stencil attachment and shader access
		DEPTH_STENCIL_READ_ONLY_OPTIMAL = 4,
		// Optimal layout when image is used for read only shader access
		SHADER_READ_ONLY_OPTIMAL = 5,
		// Optimal layout when image is used only as source of transfer operations
		TRANSFER_SRC_OPTIMAL = 6,
		// Optimal layout when image is used only as destination of transfer operations
		TRANSFER_DST_OPTIMAL = 7,
		// Initial layout used when the data is populated by the CPU
		PREINITIALIZED = 8,
	};


	public enum AttachmentLoadOp : byte
	{
		LOAD = 0,
		CLEAR = 1,
		DONT_CARE = 2,
	};


	public enum AttachmentStoreOp : byte
	{
		STORE = 0,
		DONT_CARE = 1,
	};


	public enum ImageType : byte
	{
		TYPE_1D = 0,
		TYPE_2D = 1,
		TYPE_3D = 2,
	};


	public enum ImageTiling : byte
	{
		OPTIMAL = 0,
		LINEAR = 1,
	};


	public enum ImageViewType : byte
	{
		TYPE_1D = 0,
		TYPE_2D = 1,
		TYPE_3D = 2,
		TYPE_CUBE = 3,
		TYPE_1D_ARRAY = 4,
		TYPE_2D_ARRAY = 5,
		TYPE_CUBE_ARRAY = 6,
	};


	public enum CommandBufferLevel : byte
	{
		PRIMARY = 0,
		SECONDARY = 1,
	};


	public enum ComponentSwizzle : byte
	{
		IDENTITY = 0,
		ZERO = 1,
		ONE = 2,
		R = 3,
		G = 4,
		B = 5,
		A = 6,
	};


	public enum DescriptorType : byte
	{
		SAMPLER = 0,
		COMBINED_IMAGE_SAMPLER = 1,
		SAMPLED_IMAGE = 2,
		STORAGE_IMAGE = 3,
		UNIFORM_TEXEL_BUFFER = 4,
		STORAGE_TEXEL_BUFFER = 5,
		UNIFORM_BUFFER = 6,
		STORAGE_BUFFER = 7,
		UNIFORM_BUFFER_DYNAMIC = 8,
		STORAGE_BUFFER_DYNAMIC = 9,
		INPUT_ATTACHMENT = 10,
	};


	public enum QueryType : byte
	{
		OCCLUSION = 0,
		PIPELINE_STATISTICS = 1,
		TIMESTAMP = 2,
	};


	public enum BorderColor : byte
	{
		FLOAT_TRANSPARENT_BLACK = 0,
		INT_TRANSPARENT_BLACK = 1,
		FLOAT_OPAQUE_BLACK = 2,
		INT_OPAQUE_BLACK = 3,
		FLOAT_OPAQUE_WHITE = 4,
		INT_OPAQUE_WHITE = 5,
	};


	public enum PipelineBindPoint : byte
	{
		GRAPHICS = 0,
		COMPUTE = 1,
	};


	public enum PipelineCacheHeaderVersion : byte
	{
		ONE = 1,
	};


	public enum PrimitiveTopology : byte
	{
		POINT_LIST = 0,
		LINE_LIST = 1,
		LINE_STRIP = 2,
		TRIANGLE_LIST = 3,
		TRIANGLE_STRIP = 4,
		TRIANGLE_FAN = 5,
		LINE_LIST_WITH_ADJACENCY = 6,
		LINE_STRIP_WITH_ADJACENCY = 7,
		TRIANGLE_LIST_WITH_ADJACENCY = 8,
		TRIANGLE_STRIP_WITH_ADJACENCY = 9,
		PATCH_LIST = 10,
	};


	public enum SharingMode : byte
	{
		EXCLUSIVE = 0,
		CONCURRENT = 1,
	};


	public enum IndexType : byte
	{
		UINT16 = 0,
		UINT32 = 1,
	};


	public enum Filter : byte
	{
		NEAREST = 0,
		LINEAR = 1,
	};


	public enum SamplerMipmapMode : byte
	{
		// Choose nearest mip level
		NEAREST = 0,
		// Linear filter between mip levels
		LINEAR = 1,
	};


	public enum SamplerAddressMode : byte
	{
		REPEAT = 0,
		MIRRORED_REPEAT = 1,
		CLAMP_TO_EDGE = 2,
		CLAMP_TO_BORDER = 3,
		MIRROR_CLAMP_TO_EDGE = 4,
	};


	public enum CompareOp : byte
	{
		NEVER = 0,
		LESS = 1,
		EQUAL = 2,
		LESS_OR_EQUAL = 3,
		GREATER = 4,
		NOT_EQUAL = 5,
		GREATER_OR_EQUAL = 6,
		ALWAYS = 7,
	};


	public enum PolygonMode : byte
	{
		FILL = 0,
		LINE = 1,
		POINT = 2,
	};

	[Flags] 
	public enum CullModeFlagBits : byte
	{
		NONE = 0,
		FRONT_BIT = 1 << 0,
		BACK_BIT = 1 << 1,
		FRONT_AND_BACK = 0x3,
	};


	public enum FrontFace : byte
	{
		COUNTER_CLOCKWISE = 0,
		CLOCKWISE = 1,
	};


	public enum BlendFactor : byte
	{
		ZERO = 0,
		ONE = 1,
		SRC_COLOR = 2,
		ONE_MINUS_SRC_COLOR = 3,
		DST_COLOR = 4,
		ONE_MINUS_DST_COLOR = 5,
		SRC_ALPHA = 6,
		ONE_MINUS_SRC_ALPHA = 7,
		DST_ALPHA = 8,
		ONE_MINUS_DST_ALPHA = 9,
		CONSTANT_COLOR = 10,
		ONE_MINUS_CONSTANT_COLOR = 11,
		CONSTANT_ALPHA = 12,
		ONE_MINUS_CONSTANT_ALPHA = 13,
		SRC_ALPHA_SATURATE = 14,
		SRC1_COLOR = 15,
		ONE_MINUS_SRC1_COLOR = 16,
		SRC1_ALPHA = 17,
		ONE_MINUS_SRC1_ALPHA = 18,
	};


	public enum BlendOp : byte
	{
		ADD = 0,
		SUBTRACT = 1,
		REVERSE_SUBTRACT = 2,
		MIN = 3,
		MAX = 4,
	};


	public enum StencilOp : byte
	{
		KEEP = 0,
		ZERO = 1,
		REPLACE = 2,
		INCREMENT_AND_CLAMP = 3,
		DECREMENT_AND_CLAMP = 4,
		INVERT = 5,
		INCREMENT_AND_WRAP = 6,
		DECREMENT_AND_WRAP = 7,
	};


	public enum LogicOp : byte
	{
		CLEAR = 0,
		AND = 1,
		AND_REVERSE = 2,
		COPY = 3,
		AND_INVERTED = 4,
		NO_OP = 5,
		XOR = 6,
		OR = 7,
		NOR = 8,
		EQUIVALENT = 9,
		INVERT = 10,
		OR_REVERSE = 11,
		COPY_INVERTED = 12,
		OR_INVERTED = 13,
		NAND = 14,
		SET = 15,
	};


	public enum InternalAllocationType : byte
	{
		EXECUTABLE = 0,
	};


	public enum SystemAllocationScope : byte
	{
		COMMAND = 0,
		OBJECT = 1,
		CACHE = 2,
		DEVICE = 3,
		INSTANCE = 4,
	};


	public enum PhysicalDeviceType : byte
	{
		OTHER = 0,
		INTEGRATED_GPU = 1,
		DISCRETE_GPU = 2,
		VIRTUAL_GPU = 3,
		CPU = 4,
	};


	public enum VertexInputRate : byte
	{
		VERTEX = 0,
		INSTANCE = 1,
	};


	public enum Format : byte
	{
		UNDEFINED = 0,
		R4G4_UNORM_PACK8 = 1,
		R4G4B4A4_UNORM_PACK16 = 2,
		B4G4R4A4_UNORM_PACK16 = 3,
		R5G6B5_UNORM_PACK16 = 4,
		B5G6R5_UNORM_PACK16 = 5,
		R5G5B5A1_UNORM_PACK16 = 6,
		B5G5R5A1_UNORM_PACK16 = 7,
		A1R5G5B5_UNORM_PACK16 = 8,
		R8_UNORM = 9,
		R8_SNORM = 10,
		R8_USCALED = 11,
		R8_SSCALED = 12,
		R8_UINT = 13,
		R8_SINT = 14,
		R8_SRGB = 15,
		R8G8_UNORM = 16,
		R8G8_SNORM = 17,
		R8G8_USCALED = 18,
		R8G8_SSCALED = 19,
		R8G8_UINT = 20,
		R8G8_SINT = 21,
		R8G8_SRGB = 22,
		R8G8B8_UNORM = 23,
		R8G8B8_SNORM = 24,
		R8G8B8_USCALED = 25,
		R8G8B8_SSCALED = 26,
		R8G8B8_UINT = 27,
		R8G8B8_SINT = 28,
		R8G8B8_SRGB = 29,
		B8G8R8_UNORM = 30,
		B8G8R8_SNORM = 31,
		B8G8R8_USCALED = 32,
		B8G8R8_SSCALED = 33,
		B8G8R8_UINT = 34,
		B8G8R8_SINT = 35,
		B8G8R8_SRGB = 36,
		R8G8B8A8_UNORM = 37,
		R8G8B8A8_SNORM = 38,
		R8G8B8A8_USCALED = 39,
		R8G8B8A8_SSCALED = 40,
		R8G8B8A8_UINT = 41,
		R8G8B8A8_SINT = 42,
		R8G8B8A8_SRGB = 43,
		B8G8R8A8_UNORM = 44,
		B8G8R8A8_SNORM = 45,
		B8G8R8A8_USCALED = 46,
		B8G8R8A8_SSCALED = 47,
		B8G8R8A8_UINT = 48,
		B8G8R8A8_SINT = 49,
		B8G8R8A8_SRGB = 50,
		A8B8G8R8_UNORM_PACK32 = 51,
		A8B8G8R8_SNORM_PACK32 = 52,
		A8B8G8R8_USCALED_PACK32 = 53,
		A8B8G8R8_SSCALED_PACK32 = 54,
		A8B8G8R8_UINT_PACK32 = 55,
		A8B8G8R8_SINT_PACK32 = 56,
		A8B8G8R8_SRGB_PACK32 = 57,
		A2R10G10B10_UNORM_PACK32 = 58,
		A2R10G10B10_SNORM_PACK32 = 59,
		A2R10G10B10_USCALED_PACK32 = 60,
		A2R10G10B10_SSCALED_PACK32 = 61,
		A2R10G10B10_UINT_PACK32 = 62,
		A2R10G10B10_SINT_PACK32 = 63,
		A2B10G10R10_UNORM_PACK32 = 64,
		A2B10G10R10_SNORM_PACK32 = 65,
		A2B10G10R10_USCALED_PACK32 = 66,
		A2B10G10R10_SSCALED_PACK32 = 67,
		A2B10G10R10_UINT_PACK32 = 68,
		A2B10G10R10_SINT_PACK32 = 69,
		R16_UNORM = 70,
		R16_SNORM = 71,
		R16_USCALED = 72,
		R16_SSCALED = 73,
		R16_UINT = 74,
		R16_SINT = 75,
		R16_SFLOAT = 76,
		R16G16_UNORM = 77,
		R16G16_SNORM = 78,
		R16G16_USCALED = 79,
		R16G16_SSCALED = 80,
		R16G16_UINT = 81,
		R16G16_SINT = 82,
		R16G16_SFLOAT = 83,
		R16G16B16_UNORM = 84,
		R16G16B16_SNORM = 85,
		R16G16B16_USCALED = 86,
		R16G16B16_SSCALED = 87,
		R16G16B16_UINT = 88,
		R16G16B16_SINT = 89,
		R16G16B16_SFLOAT = 90,
		R16G16B16A16_UNORM = 91,
		R16G16B16A16_SNORM = 92,
		R16G16B16A16_USCALED = 93,
		R16G16B16A16_SSCALED = 94,
		R16G16B16A16_UINT = 95,
		R16G16B16A16_SINT = 96,
		R16G16B16A16_SFLOAT = 97,
		R32_UINT = 98,
		R32_SINT = 99,
		R32_SFLOAT = 100,
		R32G32_UINT = 101,
		R32G32_SINT = 102,
		R32G32_SFLOAT = 103,
		R32G32B32_UINT = 104,
		R32G32B32_SINT = 105,
		R32G32B32_SFLOAT = 106,
		R32G32B32A32_UINT = 107,
		R32G32B32A32_SINT = 108,
		R32G32B32A32_SFLOAT = 109,
		R64_UINT = 110,
		R64_SINT = 111,
		R64_SFLOAT = 112,
		R64G64_UINT = 113,
		R64G64_SINT = 114,
		R64G64_SFLOAT = 115,
		R64G64B64_UINT = 116,
		R64G64B64_SINT = 117,
		R64G64B64_SFLOAT = 118,
		R64G64B64A64_UINT = 119,
		R64G64B64A64_SINT = 120,
		R64G64B64A64_SFLOAT = 121,
		B10G11R11_UFLOAT_PACK32 = 122,
		E5B9G9R9_UFLOAT_PACK32 = 123,
		D16_UNORM = 124,
		X8_D24_UNORM_PACK32 = 125,
		D32_SFLOAT = 126,
		S8_UINT = 127,
		D16_UNORM_S8_UINT = 128,
		D24_UNORM_S8_UINT = 129,
		D32_SFLOAT_S8_UINT = 130,
		BC1_RGB_UNORM_BLOCK = 131,
		BC1_RGB_SRGB_BLOCK = 132,
		BC1_RGBA_UNORM_BLOCK = 133,
		BC1_RGBA_SRGB_BLOCK = 134,
		BC2_UNORM_BLOCK = 135,
		BC2_SRGB_BLOCK = 136,
		BC3_UNORM_BLOCK = 137,
		BC3_SRGB_BLOCK = 138,
		BC4_UNORM_BLOCK = 139,
		BC4_SNORM_BLOCK = 140,
		BC5_UNORM_BLOCK = 141,
		BC5_SNORM_BLOCK = 142,
		BC6H_UFLOAT_BLOCK = 143,
		BC6H_SFLOAT_BLOCK = 144,
		BC7_UNORM_BLOCK = 145,
		BC7_SRGB_BLOCK = 146,
		ETC2_R8G8B8_UNORM_BLOCK = 147,
		ETC2_R8G8B8_SRGB_BLOCK = 148,
		ETC2_R8G8B8A1_UNORM_BLOCK = 149,
		ETC2_R8G8B8A1_SRGB_BLOCK = 150,
		ETC2_R8G8B8A8_UNORM_BLOCK = 151,
		ETC2_R8G8B8A8_SRGB_BLOCK = 152,
		EAC_R11_UNORM_BLOCK = 153,
		EAC_R11_SNORM_BLOCK = 154,
		EAC_R11G11_UNORM_BLOCK = 155,
		EAC_R11G11_SNORM_BLOCK = 156,
		ASTC_4x4_UNORM_BLOCK = 157,
		ASTC_4x4_SRGB_BLOCK = 158,
		ASTC_5x4_UNORM_BLOCK = 159,
		ASTC_5x4_SRGB_BLOCK = 160,
		ASTC_5x5_UNORM_BLOCK = 161,
		ASTC_5x5_SRGB_BLOCK = 162,
		ASTC_6x5_UNORM_BLOCK = 163,
		ASTC_6x5_SRGB_BLOCK = 164,
		ASTC_6x6_UNORM_BLOCK = 165,
		ASTC_6x6_SRGB_BLOCK = 166,
		ASTC_8x5_UNORM_BLOCK = 167,
		ASTC_8x5_SRGB_BLOCK = 168,
		ASTC_8x6_UNORM_BLOCK = 169,
		ASTC_8x6_SRGB_BLOCK = 170,
		ASTC_8x8_UNORM_BLOCK = 171,
		ASTC_8x8_SRGB_BLOCK = 172,
		ASTC_10x5_UNORM_BLOCK = 173,
		ASTC_10x5_SRGB_BLOCK = 174,
		ASTC_10x6_UNORM_BLOCK = 175,
		ASTC_10x6_SRGB_BLOCK = 176,
		ASTC_10x8_UNORM_BLOCK = 177,
		ASTC_10x8_SRGB_BLOCK = 178,
		ASTC_10x10_UNORM_BLOCK = 179,
		ASTC_10x10_SRGB_BLOCK = 180,
		ASTC_12x10_UNORM_BLOCK = 181,
		ASTC_12x10_SRGB_BLOCK = 182,
		ASTC_12x12_UNORM_BLOCK = 183,
		ASTC_12x12_SRGB_BLOCK = 184,
	};


	public enum StructureType : byte
	{
		APPLICATION_INFO = 0,
		INSTANCE_CREATE_INFO = 1,
		DEVICE_QUEUE_CREATE_INFO = 2,
		DEVICE_CREATE_INFO = 3,
		SUBMIT_INFO = 4,
		MEMORY_ALLOCATE_INFO = 5,
		MAPPED_MEMORY_RANGE = 6,
		BIND_SPARSE_INFO = 7,
		FENCE_CREATE_INFO = 8,
		SEMAPHORE_CREATE_INFO = 9,
		EVENT_CREATE_INFO = 10,
		QUERY_POOL_CREATE_INFO = 11,
		BUFFER_CREATE_INFO = 12,
		BUFFER_VIEW_CREATE_INFO = 13,
		IMAGE_CREATE_INFO = 14,
		IMAGE_VIEW_CREATE_INFO = 15,
		SHADER_MODULE_CREATE_INFO = 16,
		PIPELINE_CACHE_CREATE_INFO = 17,
		PIPELINE_SHADER_STAGE_CREATE_INFO = 18,
		PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO = 19,
		PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO = 20,
		PIPELINE_TESSELLATION_STATE_CREATE_INFO = 21,
		PIPELINE_VIEWPORT_STATE_CREATE_INFO = 22,
		PIPELINE_RASTERIZATION_STATE_CREATE_INFO = 23,
		PIPELINE_MULTISAMPLE_STATE_CREATE_INFO = 24,
		PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO = 25,
		PIPELINE_COLOR_BLEND_STATE_CREATE_INFO = 26,
		PIPELINE_DYNAMIC_STATE_CREATE_INFO = 27,
		GRAPHICS_PIPELINE_CREATE_INFO = 28,
		COMPUTE_PIPELINE_CREATE_INFO = 29,
		PIPELINE_LAYOUT_CREATE_INFO = 30,
		SAMPLER_CREATE_INFO = 31,
		DESCRIPTOR_SET_LAYOUT_CREATE_INFO = 32,
		DESCRIPTOR_POOL_CREATE_INFO = 33,
		DESCRIPTOR_SET_ALLOCATE_INFO = 34,
		WRITE_DESCRIPTOR_SET = 35,
		COPY_DESCRIPTOR_SET = 36,
		FRAMEBUFFER_CREATE_INFO = 37,
		RENDER_PASS_CREATE_INFO = 38,
		COMMAND_POOL_CREATE_INFO = 39,
		COMMAND_BUFFER_ALLOCATE_INFO = 40,
		COMMAND_BUFFER_INHERITANCE_INFO = 41,
		COMMAND_BUFFER_BEGIN_INFO = 42,
		RENDER_PASS_BEGIN_INFO = 43,
		BUFFER_MEMORY_BARRIER = 44,
		IMAGE_MEMORY_BARRIER = 45,
		MEMORY_BARRIER = 46,
		LOADER_INSTANCE_CREATE_INFO = 47,
		LOADER_DEVICE_CREATE_INFO = 48,
	};


	public enum SubpassContents : byte
	{
		INLINE = 0,
		SECONDARY_COMMAND_BUFFERS = 1,
	};


	public enum Result : Int32
	{
		// Command completed successfully
		SUCCESS = 0,
		// A fence or query has not yet completed
		NOT_READY = 1,
		// A wait operation has not completed in the specified time
		TIMEOUT = 2,
		// An event is signaled
		EVENT_SET = 3,
		// An event is unsignalled
		EVENT_RESET = 4,
		// A return array was too small for the resul
		INCOMPLETE = 5,
		// A host memory allocation has failed
		ERROR_OUT_OF_HOST_MEMORY = -1,
		// A device memory allocation has failed
		ERROR_OUT_OF_DEVICE_MEMORY = -2,
		// The logical device has been lost. See <<devsandqueues-lost-device>>
		ERROR_INITIALIZATION_FAILED = -3,
		// Initialization of a object has failed
		ERROR_DEVICE_LOST = -4,
		// Mapping of a memory object has failed
		ERROR_MEMORY_MAP_FAILED = -5,
		// Layer specified does not exist
		ERROR_LAYER_NOT_PRESENT = -6,
		// Extension specified does not exist
		ERROR_EXTENSION_NOT_PRESENT = -7,
		// Requested feature is not available on this device
		ERROR_FEATURE_NOT_PRESENT = -8,
		// Unable to find a Vulkan driver
		ERROR_INCOMPATIBLE_DRIVER = -9,
		// Too many objects of the type have already been created
		ERROR_TOO_MANY_OBJECTS = -10,
		// Requested format is not supported on this device
		ERROR_FORMAT_NOT_SUPPORTED = -11,
	};


	public enum DynamicState : byte
	{
		VIEWPORT = 0,
		SCISSOR = 1,
		LINE_WIDTH = 2,
		DEPTH_BIAS = 3,
		BLEND_CONSTANTS = 4,
		DEPTH_BOUNDS = 5,
		STENCIL_COMPARE_MASK = 6,
		STENCIL_WRITE_MASK = 7,
		STENCIL_REFERENCE = 8,
	};

	[Flags] 
	public enum QueueFlagBits : byte
	{
		// Queue supports graphics operations
		GRAPHICS_BIT = 1 << 0,
		// Queue supports compute operations
		COMPUTE_BIT = 1 << 1,
		// Queue supports transfer operations
		TRANSFER_BIT = 1 << 2,
		// Queue supports sparse resource memory management operations
		SPARSE_BINDING_BIT = 1 << 3,
	};

	[Flags] 
	public enum MemoryPropertyFlagBits : byte
	{
		// If otherwise stated, then allocate memory on device
		DEVICE_LOCAL_BIT = 1 << 0,
		// Memory is mappable by host
		HOST_VISIBLE_BIT = 1 << 1,
		// Memory will have i/o coherency. If not set, application may need to use vkFlushMappedMemoryRanges and vkInvalidateMappedMemoryRanges to flush/invalidate host cache
		HOST_COHERENT_BIT = 1 << 2,
		// Memory will be cached by the host
		HOST_CACHED_BIT = 1 << 3,
		// Memory may be allocated by the driver when it is required
		LAZILY_ALLOCATED_BIT = 1 << 4,
	};

	[Flags] 
	public enum MemoryHeapFlagBits : byte
	{
		// If set, heap represents device memory
		DEVICE_LOCAL_BIT = 1 << 0,
	};

	[Flags] 
	public enum AccessFlagBits : UInt32
	{
		// Controls coherency of indirect command reads
		INDIRECT_COMMAND_READ_BIT = 1 << 0,
		// Controls coherency of index reads
		INDEX_READ_BIT = 1 << 1,
		// Controls coherency of vertex attribute reads
		VERTEX_ATTRIBUTE_READ_BIT = 1 << 2,
		// Controls coherency of uniform buffer reads
		UNIFORM_READ_BIT = 1 << 3,
		// Controls coherency of input attachment reads
		INPUT_ATTACHMENT_READ_BIT = 1 << 4,
		// Controls coherency of shader reads
		SHADER_READ_BIT = 1 << 5,
		// Controls coherency of shader writes
		SHADER_WRITE_BIT = 1 << 6,
		// Controls coherency of color attachment reads
		COLOR_ATTACHMENT_READ_BIT = 1 << 7,
		// Controls coherency of color attachment writes
		COLOR_ATTACHMENT_WRITE_BIT = 1 << 8,
		// Controls coherency of depth/stencil attachment reads
		DEPTH_STENCIL_ATTACHMENT_READ_BIT = 1 << 9,
		// Controls coherency of depth/stencil attachment writes
		DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 1 << 10,
		// Controls coherency of transfer reads
		TRANSFER_READ_BIT = 1 << 11,
		// Controls coherency of transfer writes
		TRANSFER_WRITE_BIT = 1 << 12,
		// Controls coherency of host reads
		HOST_READ_BIT = 1 << 13,
		// Controls coherency of host writes
		HOST_WRITE_BIT = 1 << 14,
		// Controls coherency of memory reads
		MEMORY_READ_BIT = 1 << 15,
		// Controls coherency of memory writes
		MEMORY_WRITE_BIT = 1 << 16,
	};

	[Flags] 
	public enum BufferUsageFlagBits : ushort
	{
		// Can be used as a source of transfer operations
		TRANSFER_SRC_BIT = 1 << 0,
		// Can be used as a destination of transfer operations
		TRANSFER_DST_BIT = 1 << 1,
		// Can be used as TBO
		UNIFORM_TEXEL_BUFFER_BIT = 1 << 2,
		// Can be used as IBO
		STORAGE_TEXEL_BUFFER_BIT = 1 << 3,
		// Can be used as UBO
		UNIFORM_BUFFER_BIT = 1 << 4,
		// Can be used as SSBO
		STORAGE_BUFFER_BIT = 1 << 5,
		// Can be used as source of fixed-function index fetch (index buffer)
		INDEX_BUFFER_BIT = 1 << 6,
		// Can be used as source of fixed-function vertex fetch (VBO)
		VERTEX_BUFFER_BIT = 1 << 7,
		// Can be the source of indirect parameters (e.g. indirect buffer, parameter buffer)
		INDIRECT_BUFFER_BIT = 1 << 8,
	};

	[Flags] 
	public enum BufferCreateFlagBits : byte
	{
		// Buffer should support sparse backing
		BINDING_BIT = 1 << 0,
		// Buffer should support sparse backing with partial residency
		RESIDENCY_BIT = 1 << 1,
		// Buffer should support constent data access to physical memory blocks mapped into multiple locations of sparse buffers
		ALIASED_BIT = 1 << 2,
	};

	[Flags] 
	public enum ShaderStageFlagBits : UInt32
	{
		VERTEX_BIT = 1 << 0,
		TESSELLATION_CONTROL_BIT = 1 << 1,
		TESSELLATION_EVALUATION_BIT = 1 << 2,
		GEOMETRY_BIT = 1 << 3,
		FRAGMENT_BIT = 1 << 4,
		COMPUTE_BIT = 1 << 5,
		ALL_GRAPHICS = 0x1F,
		ALL = 0x7FFFFFFF,
	};

	[Flags] 
	public enum ImageUsageFlagBits : byte
	{
		// Can be used as a source of transfer operations
		TRANSFER_SRC_BIT = 1 << 0,
		// Can be used as a destination of transfer operations
		TRANSFER_DST_BIT = 1 << 1,
		// Can be sampled from (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
		SAMPLED_BIT = 1 << 2,
		// Can be used as storage image (STORAGE_IMAGE descriptor type)
		STORAGE_BIT = 1 << 3,
		// Can be used as framebuffer color attachment
		COLOR_ATTACHMENT_BIT = 1 << 4,
		// Can be used as framebuffer depth/stencil attachment
		DEPTH_STENCIL_ATTACHMENT_BIT = 1 << 5,
		// Image data not needed outside of rendering
		TRANSIENT_ATTACHMENT_BIT = 1 << 6,
		// Can be used as framebuffer input attachment
		INPUT_ATTACHMENT_BIT = 1 << 7,
	};

	[Flags] 
	public enum ImageCreateFlagBits : byte
	{
		// Image should support sparse backing
		SPARSE_BINDING_BIT = 1 << 0,
		// Image should support sparse backing with partial residency
		SPARSE_RESIDENCY_BIT = 1 << 1,
		// Image should support constent data access to physical memory blocks mapped into multiple locations of sparse images
		SPARSE_ALIASED_BIT = 1 << 2,
		// Allows image views to have different format than the base image
		MUTABLE_FORMAT_BIT = 1 << 3,
		// Allows creating image views with cube type from the created image
		CUBE_COMPATIBLE_BIT = 1 << 4,
	};

	[Flags] 
	public enum PipelineCreateFlagBits : byte
	{
		DISABLE_OPTIMIZATION_BIT = 1 << 0,
		ALLOW_DERIVATIVES_BIT = 1 << 1,
		DERIVATIVE_BIT = 1 << 2,
	};

	[Flags] 
	public enum ColorComponentFlagBits : byte
	{
		R_BIT = 1 << 0,
		G_BIT = 1 << 1,
		B_BIT = 1 << 2,
		A_BIT = 1 << 3,
	};

	[Flags] 
	public enum FenceCreateFlagBits : byte
	{
		SIGNALED_BIT = 1 << 0,
	};

	[Flags] 
	public enum FormatFeatureFlagBits : ushort
	{
		// Format can be used for sampled images (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
		SAMPLED_IMAGE_BIT = 1 << 0,
		// Format can be used for storage images (STORAGE_IMAGE descriptor type)
		STORAGE_IMAGE_BIT = 1 << 1,
		// Format supports atomic operations in case it's used for storage images
		STORAGE_IMAGE_ATOMIC_BIT = 1 << 2,
		// Format can be used for uniform texel buffers (TBOs)
		UNIFORM_TEXEL_BUFFER_BIT = 1 << 3,
		// Format can be used for storage texel buffers (IBOs)
		STORAGE_TEXEL_BUFFER_BIT = 1 << 4,
		// Format supports atomic operations in case it's used for storage texel buffers
		STORAGE_TEXEL_BUFFER_ATOMIC_BIT = 1 << 5,
		// Format can be used for vertex buffers (VBOs)
		VERTEX_BUFFER_BIT = 1 << 6,
		// Format can be used for color attachment images
		COLOR_ATTACHMENT_BIT = 1 << 7,
		// Format supports blending in case it's used for color attachment images
		COLOR_ATTACHMENT_BLEND_BIT = 1 << 8,
		// Format can be used for depth/stencil attachment images
		DEPTH_STENCIL_ATTACHMENT_BIT = 1 << 9,
		// Format can be used as the source image of blits with vkCmdBlitImage
		BLIT_SRC_BIT = 1 << 10,
		// Format can be used as the destination image of blits with vkCmdBlitImage
		BLIT_DST_BIT = 1 << 11,
		// Format can be filtered with VK_FILTER_LINEAR when being sampled
		SAMPLED_IMAGE_FILTER_LINEAR_BIT = 1 << 12,
	};

	[Flags] 
	public enum QueryControlFlagBits : byte
	{
		// Require precise results to be collected by the query
		PRECISE_BIT = 1 << 0,
	};

	[Flags] 
	public enum QueryResultFlagBits : byte
	{
		// Results of the queries are written to the destination buffer as 64-bit values
		RESULT_64_BIT = 1 << 0,
		// Results of the queries are waited on before proceeding with the result copy
		RESULT_WAIT_BIT = 1 << 1,
		// Besides the results of the query, the availability of the results is also written
		RESULT_WITH_AVAILABILITY_BIT = 1 << 2,
		// Copy the partial results of the query even if the final results aren't available
		RESULT_PARTIAL_BIT = 1 << 3,
	};

	[Flags] 
	public enum CommandBufferUsageFlagBits : byte
	{
		ONE_TIME_SUBMIT_BIT = 1 << 0,
		RENDER_PASS_CONTINUE_BIT = 1 << 1,
		// Command buffer may be submitted/executed more than once simultaneously
		SIMULTANEOUS_USE_BIT = 1 << 2,
	};

	[Flags] 
	public enum QueryPipelineStatisticFlagBits : ushort
	{
		INPUT_ASSEMBLY_VERTICES_BIT = 1 << 0,
		INPUT_ASSEMBLY_PRIMITIVES_BIT = 1 << 1,
		VERTEX_SHADER_INVOCATIONS_BIT = 1 << 2,
		GEOMETRY_SHADER_INVOCATIONS_BIT = 1 << 3,
		GEOMETRY_SHADER_PRIMITIVES_BIT = 1 << 4,
		CLIPPING_INVOCATIONS_BIT = 1 << 5,
		CLIPPING_PRIMITIVES_BIT = 1 << 6,
		FRAGMENT_SHADER_INVOCATIONS_BIT = 1 << 7,
		TESSELLATION_CONTROL_SHADER_PATCHES_BIT = 1 << 8,
		TESSELLATION_EVALUATION_SHADER_INVOCATIONS_BIT = 1 << 9,
		COMPUTE_SHADER_INVOCATIONS_BIT = 1 << 10,
	};

	[Flags] 
	public enum ImageAspectFlagBits : byte
	{
		COLOR_BIT = 1 << 0,
		DEPTH_BIT = 1 << 1,
		STENCIL_BIT = 1 << 2,
		METADATA_BIT = 1 << 3,
	};

	[Flags] 
	public enum SparseImageFormatFlagBits : byte
	{
		// Image uses a single miptail region for all array layers
		SINGLE_MIPTAIL_BIT = 1 << 0,
		// Image requires mip levels to be an exact multiple of the sparse image block size for non-miptail levels.
		ALIGNED_MIP_SIZE_BIT = 1 << 1,
		// Image uses a non-standard sparse block size
		NONSTANDARD_BLOCK_SIZE_BIT = 1 << 2,
	};

	[Flags] 
	public enum SparseMemoryBindFlagBits : byte
	{
		// Operation binds resource metadata to memory
		METADATA_BIT = 1 << 0,
	};

	[Flags] 
	public enum PipelineStageFlagBits : UInt32
	{
		// Before subsequent commands are processed
		TOP_OF_PIPE_BIT = 1 << 0,
		// Draw/DispatchIndirect command fetch
		DRAW_INDIRECT_BIT = 1 << 1,
		// Vertex/index fetch
		VERTEX_INPUT_BIT = 1 << 2,
		// Vertex shading
		VERTEX_SHADER_BIT = 1 << 3,
		// Tessellation control shading
		TESSELLATION_CONTROL_SHADER_BIT = 1 << 4,
		// Tessellation evaluation shading
		TESSELLATION_EVALUATION_SHADER_BIT = 1 << 5,
		// Geometry shading
		GEOMETRY_SHADER_BIT = 1 << 6,
		// Fragment shading
		FRAGMENT_SHADER_BIT = 1 << 7,
		// Early fragment (depth and stencil) tests
		EARLY_FRAGMENT_TESTS_BIT = 1 << 8,
		// Late fragment (depth and stencil) tests
		LATE_FRAGMENT_TESTS_BIT = 1 << 9,
		// Color attachment writes
		COLOR_ATTACHMENT_OUTPUT_BIT = 1 << 10,
		// Compute shading
		COMPUTE_SHADER_BIT = 1 << 11,
		// Transfer/copy operations
		TRANSFER_BIT = 1 << 12,
		// After previous commands have completed
		BOTTOM_OF_PIPE_BIT = 1 << 13,
		// Indicates host (CPU) is a source/sink of the dependency
		HOST_BIT = 1 << 14,
		// All stages of the graphics pipeline
		ALL_GRAPHICS_BIT = 1 << 15,
		// All stages supported on the queue
		ALL_COMMANDS_BIT = 1 << 16,
	};

	[Flags] 
	public enum CommandPoolCreateFlagBits : byte
	{
		// Command buffers have a short lifetime
		TRANSIENT_BIT = 1 << 0,
		// Command buffers may release their memory individually
		RESET_COMMAND_BUFFER_BIT = 1 << 1,
	};

	[Flags] 
	public enum CommandPoolResetFlagBits : byte
	{
		// Release resources owned by the pool
		RELEASE_RESOURCES_BIT = 1 << 0,
	};

	[Flags] 
	public enum CommandBufferResetFlagBits : byte
	{
		// Release resources owned by the buffer
		RELEASE_RESOURCES_BIT = 1 << 0,
	};

	[Flags] 
	public enum SampleCountFlagBits : byte
	{
		// Sample count 1 supported
		COUNT_1_BIT = 1 << 0,
		// Sample count 2 supported
		COUNT_2_BIT = 1 << 1,
		// Sample count 4 supported
		COUNT_4_BIT = 1 << 2,
		// Sample count 8 supported
		COUNT_8_BIT = 1 << 3,
		// Sample count 16 supported
		COUNT_16_BIT = 1 << 4,
		// Sample count 32 supported
		COUNT_32_BIT = 1 << 5,
		// Sample count 64 supported
		COUNT_64_BIT = 1 << 6,
	};

	[Flags] 
	public enum AttachmentDescriptionFlagBits : byte
	{
		// The attachment may alias physical memory of another attachment in the same render pass
		ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT = 1 << 0,
	};

	[Flags] 
	public enum StencilFaceFlagBits : byte
	{
		// Front face
		FRONT_BIT = 1 << 0,
		// Back face
		BACK_BIT = 1 << 1,
		// Front and back faces
		FRONT_AND_BACK = 0x3,
	};

	[Flags] 
	public enum DescriptorPoolCreateFlagBits : byte
	{
		// Descriptor sets may be freed individually
		DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT = 1 << 0,
	};

	[Flags] 
	public enum DependencyFlagBits : byte
	{
		// Dependency is per pixel region 
		VK_DEPENDENCY_BY_REGION_BIT = 1 << 0,
	};


	public enum PresentModeKHR : byte
	{
		IMMEDIATE_KHR = 0,
		MAILBOX_KHR = 1,
		FIFO_KHR = 2,
		FIFO_RELAXED_KHR = 3,
	};


	public enum ColorSpaceKHR : byte
	{
		SRGB_NONLINEAR_KHR = 0,
	};

	[Flags] 
	public enum DisplayPlaneAlphaFlagBitsKHR : byte
	{
		OPAQUE_BIT_KHR = 1 << 0,
		GLOBAL_BIT_KHR = 1 << 1,
		PER_PIXEL_BIT_KHR = 1 << 2,
		PER_PIXEL_PREMULTIPLIED_BIT_KHR = 1 << 3,
	};

	[Flags] 
	public enum CompositeAlphaFlagBitsKHR : byte
	{
		OPAQUE_BIT_KHR = 1 << 0,
		PRE_MULTIPLIED_BIT_KHR = 1 << 1,
		POST_MULTIPLIED_BIT_KHR = 1 << 2,
		INHERIT_BIT_KHR = 1 << 3,
	};

	[Flags] 
	public enum SurfaceTransformFlagBitsKHR : ushort
	{
		IDENTITY_BIT_KHR = 1 << 0,
		ROTATE_90_BIT_KHR = 1 << 1,
		ROTATE_180_BIT_KHR = 1 << 2,
		ROTATE_270_BIT_KHR = 1 << 3,
		HORIZONTAL_MIRROR_BIT_KHR = 1 << 4,
		HORIZONTAL_MIRROR_ROTATE_90_BIT_KHR = 1 << 5,
		HORIZONTAL_MIRROR_ROTATE_180_BIT_KHR = 1 << 6,
		HORIZONTAL_MIRROR_ROTATE_270_BIT_KHR = 1 << 7,
		INHERIT_BIT_KHR = 1 << 8,
	};

	[Flags] 
	public enum DebugReportFlagBitsEXT : byte
	{
		INFORMATION_BIT_EXT = 1 << 0,
		WARNING_BIT_EXT = 1 << 1,
		PERFORMANCE_WARNING_BIT_EXT = 1 << 2,
		ERROR_BIT_EXT = 1 << 3,
		DEBUG_BIT_EXT = 1 << 4,
	};


	public enum DebugReportObjectTypeEXT : byte
	{
		UNKNOWN_EXT = 0,
		INSTANCE_EXT = 1,
		PHYSICAL_DEVICE_EXT = 2,
		DEVICE_EXT = 3,
		QUEUE_EXT = 4,
		SEMAPHORE_EXT = 5,
		COMMAND_BUFFER_EXT = 6,
		FENCE_EXT = 7,
		DEVICE_MEMORY_EXT = 8,
		BUFFER_EXT = 9,
		IMAGE_EXT = 10,
		EVENT_EXT = 11,
		QUERY_POOL_EXT = 12,
		BUFFER_VIEW_EXT = 13,
		IMAGE_VIEW_EXT = 14,
		SHADER_MODULE_EXT = 15,
		PIPELINE_CACHE_EXT = 16,
		PIPELINE_LAYOUT_EXT = 17,
		RENDER_PASS_EXT = 18,
		PIPELINE_EXT = 19,
		DESCRIPTOR_SET_LAYOUT_EXT = 20,
		SAMPLER_EXT = 21,
		DESCRIPTOR_POOL_EXT = 22,
		DESCRIPTOR_SET_EXT = 23,
		FRAMEBUFFER_EXT = 24,
		COMMAND_POOL_EXT = 25,
		SURFACE_KHR_EXT = 26,
		SWAPCHAIN_KHR_EXT = 27,
		DEBUG_REPORT_EXT = 28,
	};


	public enum DebugReportErrorEXT : byte
	{
		// Used for INFO & other non-error messages
		NONE_EXT = 0,
		// Callbacks were not destroyed prior to calling DestroyInstance
		CALLBACK_REF_EXT = 1,
	};

}