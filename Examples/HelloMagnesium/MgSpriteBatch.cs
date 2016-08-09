using System;
using OpenTK.Graphics.OpenGL;
using Magnesium;
using System.Diagnostics;
using MonoGame.Content;

namespace HelloMagnesium
{
	public class MgSpriteBatch : IDisposable
	{
		readonly IContentStreamer mContent;
		readonly IMgThreadPartition mPartition;

		public MgSpriteBatch (IMgThreadPartition partition, IContentStreamer content)
		{
			mPartition = partition;
			mContent = content;
		}

		public IMgPipeline[] GraphicsPipelines {
			get;
			private set;
		}

		public IMgDescriptorSetLayout DescriptorSetLayout {
			get;
			private set;
		}

		public IMgPipelineLayout PipelineLayout {
			get;
			private set;
		}

		public void GenerateEffectPipeline (IMgGraphicsDevice graphicsDevice)
		{
			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "GenerateEffectPipeline (PREVIOUS) : " + error);
			}

			// Create effect / pass / sub pass / pipeline tree
			using (var vs = mContent.LoadContent (new AssetIdentifier {AssetId = 0x80000002}, new[] {".vs"}))
			using (var fs = mContent.LoadContent (new AssetIdentifier {AssetId = 0x80000003}, new[] {".fs"}))
			{
				IMgShaderModule vertSM;
				var vertCreateInfo = new MgShaderModuleCreateInfo {
					Code = vs,
					CodeSize = new UIntPtr ((ulong)vs.Length),
				};
				mPartition.Device.CreateShaderModule (vertCreateInfo, null, out vertSM);
				IMgShaderModule fragSM;
				var fragCreateInfo = new MgShaderModuleCreateInfo {
					Code = fs,
					CodeSize = new UIntPtr ((ulong)fs.Length),
				};
				mPartition.Device.CreateShaderModule (fragCreateInfo, null, out fragSM);
				MgDescriptorSetLayoutCreateInfo dslCreateInfo = new MgDescriptorSetLayoutCreateInfo {
					Bindings = new[] {
						//						new MgDescriptorSetLayoutBinding
						//						{
						//							Binding = 0,
						//							DescriptorType = MgDescriptorType.STORAGE_BUFFER,
						//							StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
						//							DescriptorCount = 1,
						//						},
						new MgDescriptorSetLayoutBinding {
							Binding = 0,
							DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
							StageFlags = MgShaderStageFlagBits.FRAGMENT_BIT,
							DescriptorCount = 1,
						},
					},
				};
				IMgDescriptorSetLayout setLayout;
				mPartition.Device.CreateDescriptorSetLayout (dslCreateInfo, null, out setLayout);
				DescriptorSetLayout = setLayout;
				var pLayoutCreateInfo = new MgPipelineLayoutCreateInfo {
					SetLayouts = new[] {
						setLayout
					},
				};
				IMgPipelineLayout layout;
				mPartition.Device.CreatePipelineLayout (pLayoutCreateInfo, null, out layout);
				PipelineLayout = layout;
				var pipelineParameters = new[] {
					new MgGraphicsPipelineCreateInfo {
						RenderPass = graphicsDevice.Renderpass,
						ColorBlendState = new MgPipelineColorBlendStateCreateInfo
						{
							Attachments = new MgPipelineColorBlendAttachmentState[]
							{
								new MgPipelineColorBlendAttachmentState
								{
									BlendEnable = true,
									ColorWriteMask = MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
									SrcColorBlendFactor = MgBlendFactor.SRC_COLOR,
									SrcAlphaBlendFactor = MgBlendFactor.SRC_ALPHA,
									ColorBlendOp = MgBlendOp.ADD,
									DstColorBlendFactor = MgBlendFactor.ZERO,
									DstAlphaBlendFactor = MgBlendFactor.ZERO,
								}
							},
						},
						DepthStencilState = new MgPipelineDepthStencilStateCreateInfo {
							Front = new MgStencilOpState {
								WriteMask = ~0U,
								Reference = ~0U,
								CompareMask = int.MaxValue,
								CompareOp = MgCompareOp.ALWAYS,
								PassOp = MgStencilOp.KEEP,
								FailOp = MgStencilOp.KEEP,
								DepthFailOp = MgStencilOp.KEEP,
							},
							Back = new MgStencilOpState {								
								WriteMask = ~0U,
								Reference = ~0U,
								CompareMask = int.MaxValue,
								CompareOp = MgCompareOp.ALWAYS,
								PassOp = MgStencilOp.KEEP,
								FailOp = MgStencilOp.KEEP,
								DepthFailOp = MgStencilOp.KEEP,
							},
							StencilTestEnable = false,
							DepthTestEnable = true,
							DepthWriteEnable = true,
							DepthCompareOp = MgCompareOp.LESS,
							MinDepthBounds = 0f,
							MaxDepthBounds = 1000f,
						},
						Stages = new[] {
							new MgPipelineShaderStageCreateInfo {
								Module = vertSM,
								Name = "main",
								Stage = MgShaderStageFlagBits.VERTEX_BIT,
							},
							new MgPipelineShaderStageCreateInfo {
								Module = fragSM,
								Name = "main",
								Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
							},
						},
						InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo {
							Topology = MgPrimitiveTopology.TRIANGLE_LIST,
						},
						Layout = layout,
						MultisampleState = new MgPipelineMultisampleStateCreateInfo {
							RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
						},
						RasterizationState = new MgPipelineRasterizationStateCreateInfo {
							PolygonMode = MgPolygonMode.FILL,
							CullMode = MgCullModeFlagBits.NONE,
							FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
							Flags = 0,
							DepthClampEnable = true,
							LineWidth = 1f,
						},
						VertexInputState = new MgPipelineVertexInputStateCreateInfo {
							VertexBindingDescriptions = new[] {
								new MgVertexInputBindingDescription {
									Binding = 0,
									Stride = sizeof(float) * 4,
									InputRate = MgVertexInputRate.VERTEX
								},
							},
							VertexAttributeDescriptions = new[] {
								new MgVertexInputAttributeDescription {
									Binding = 0,
									Location = 0,
									Format = MgFormat.R32G32_SFLOAT,
									Offset = 0,
								},
								new MgVertexInputAttributeDescription {
									Binding = 0,
									Location = 1,
									Format = MgFormat.R32G32_SFLOAT,
									Offset = sizeof(float) * 2,
								},
							},
						},
						ViewportState = new MgPipelineViewportStateCreateInfo {
							Scissors = new[] {
								graphicsDevice.Scissor
							},
							Viewports = new[] {
								graphicsDevice.CurrentViewport
							},
						},
					},
				};
				IMgPipeline[] graphicsPipelines;
				mPartition.Device.CreateGraphicsPipelines (null, pipelineParameters, null, out graphicsPipelines);
				GraphicsPipelines = graphicsPipelines;
				vertSM.DestroyShaderModule (mPartition.Device, null);
				fragSM.DestroyShaderModule (mPartition.Device, null);
			}

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "GenerateEffectPipeline (END) : " + error);
			}
		}

		#region IDisposable implementation
		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

