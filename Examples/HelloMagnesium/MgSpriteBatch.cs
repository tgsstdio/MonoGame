﻿using System;
using OpenTK.Graphics.OpenGL;
using Magnesium;
using System.Diagnostics;
using MonoGame.Content;
using MonoGame.Graphics;

namespace HelloMagnesium
{
	public class MgSpriteBatch : IDisposable
	{
		readonly IShaderContentStreamer mContent;
		readonly IMgThreadPartition mPartition;

		public MgSpriteBatch (IMgThreadPartition partition, IShaderContentStreamer content)
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
			// Create effect / pass / sub pass / pipeline tree
			using (var vs = mContent.Load (new AssetIdentifier {AssetId = 0x80000002}))
			using (var fs = mContent.Load (new AssetIdentifier {AssetId = 0x80000003}))
			{
				IMgShaderModule vertSM;
				var vertCreateInfo = new MgShaderModuleCreateInfo {
					Code = vs,
					CodeSize = new UIntPtr ((ulong)vs.Length),
				};
				var err =mPartition.Device.CreateShaderModule (vertCreateInfo, null, out vertSM);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                IMgShaderModule fragSM;
				var fragCreateInfo = new MgShaderModuleCreateInfo {
					Code = fs,
					CodeSize = new UIntPtr ((ulong)fs.Length),
				};
				err = mPartition.Device.CreateShaderModule (fragCreateInfo, null, out fragSM);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
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
				err = mPartition.Device.CreateDescriptorSetLayout (dslCreateInfo, null, out setLayout);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
                DescriptorSetLayout = setLayout;
				var pLayoutCreateInfo = new MgPipelineLayoutCreateInfo {
					SetLayouts = new[] {
						setLayout
					},
				};
				IMgPipelineLayout layout;
				err = mPartition.Device.CreatePipelineLayout (pLayoutCreateInfo, null, out layout);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
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
                                    // WORKS NOW, 
									BlendEnable = false,
									ColorWriteMask = MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
									SrcColorBlendFactor = MgBlendFactor.SRC_COLOR,
									SrcAlphaBlendFactor = MgBlendFactor.SRC_ALPHA,
                                    AlphaBlendOp = MgBlendOp.ADD,
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
								Name = "vmain",
								Stage = MgShaderStageFlagBits.VERTEX_BIT,
							},
							new MgPipelineShaderStageCreateInfo {
								Module = fragSM,
								Name = "fmain",
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
				err = mPartition.Device.CreateGraphicsPipelines (null, pipelineParameters, null, out graphicsPipelines);
                Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
				GraphicsPipelines = graphicsPipelines;
				vertSM.DestroyShaderModule (mPartition.Device, null);
				fragSM.DestroyShaderModule (mPartition.Device, null);
			}
		}

        #region IDisposable implementation
        private bool mIsDisposed = false;
		public void Dispose ()
		{
            if (mIsDisposed)
                return;

            if (GraphicsPipelines != null)
            {
                foreach (var pipeline in GraphicsPipelines)
                {
                    pipeline.DestroyPipeline(mPartition.Device, null);
                }
            }

            if (DescriptorSetLayout != null)
            {
                DescriptorSetLayout.DestroyDescriptorSetLayout(mPartition.Device, null);
            }

            if (PipelineLayout != null)
            {
                PipelineLayout.DestroyPipelineLayout(mPartition.Device, null);
            }

            mIsDisposed = true;
        }
		#endregion
	}
}

