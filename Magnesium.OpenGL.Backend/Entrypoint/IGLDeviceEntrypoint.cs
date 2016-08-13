namespace Magnesium.OpenGL
{
	public interface IGLDeviceEntrypoint
	{
		ICmdVBOEntrypoint VBO { get; }
		IGLSamplerEntrypoint Sampler { get; }
		IGLImageEntrypoint Image {get; }
		IGLImageViewEntrypoint ImageView { get; }
		IGLImageDescriptorEntrypoint ImageDescriptor { get; }
		IGLShaderModuleEntrypoint ShaderModule { get; }
		IGLDescriptorPoolEntrypoint DescriptorPool { get; }
		IGLBufferEntrypoint Buffers { get;}
		IGLDeviceMemoryEntrypoint DeviceMemory { get; }
		IGLSemaphoreEntrypoint Semaphore {get; }
		IGLGraphicsPipelineEntrypoint GraphicsPipeline { get; }
		IGLImageFormatEntrypoint ImageFormat { get; }
	}
}

