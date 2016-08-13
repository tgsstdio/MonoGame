using System;

namespace Magnesium.OpenGL
{
	public class DefaultGLDeviceEntrypoint : IGLDeviceEntrypoint
	{
		public DefaultGLDeviceEntrypoint 
		(
			ICmdVBOEntrypoint vbo,
			IGLSamplerEntrypoint sampler,
			IGLImageEntrypoint image,
			IGLImageViewEntrypoint imageView,
			IGLImageDescriptorEntrypoint imageDescriptor,
			IGLShaderModuleEntrypoint shaderModule,
			IGLDescriptorPoolEntrypoint descriptorPool,
			IGLBufferEntrypoint buffers,
			IGLDeviceMemoryEntrypoint deviceMemory,
			IGLSemaphoreEntrypoint semaphore,
			IGLGraphicsPipelineEntrypoint graphicsPipeline,
			IGLImageFormatEntrypoint imageFormat
		)
		{
			VBO = vbo;
			Sampler = sampler;
			Image = image;
			ImageView = imageView;
			ImageDescriptor = imageDescriptor;
			ShaderModule = shaderModule;
			DescriptorPool = descriptorPool;
			Buffers = buffers;
			DeviceMemory = deviceMemory;
			Semaphore = semaphore;
			GraphicsPipeline = graphicsPipeline;
			ImageFormat = imageFormat;
		}

		#region IGLDeviceCapabilities implementation

		public IGLImageFormatEntrypoint ImageFormat {
			get;
			private set;
		}

		public ICmdVBOEntrypoint VBO {
			get ;
			private set;
		}

		public IGLSamplerEntrypoint Sampler {
			get;
			private set;
		}

		public IGLImageEntrypoint Image {
			get;
			private set;
		}

		public IGLImageViewEntrypoint ImageView {
			get;
			private set;
		}

		public IGLImageDescriptorEntrypoint ImageDescriptor {
			get;
			private set;
		}

		public IGLShaderModuleEntrypoint ShaderModule {
			get;
			private set;
		}

		public IGLDescriptorPoolEntrypoint DescriptorPool {
			get;
			private set;
		}

		public IGLBufferEntrypoint Buffers
		{
			get;
			private set;
		}

		public IGLDeviceMemoryEntrypoint DeviceMemory {
			get;
			private set;
		}

		public IGLSemaphoreEntrypoint Semaphore {
			get;
			private set;
		}

		public IGLGraphicsPipelineEntrypoint GraphicsPipeline {
			get;
			private set;
		}
		#endregion
	}
}

