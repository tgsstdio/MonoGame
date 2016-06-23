﻿using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLGraphicsPipeline : IMgPipeline, IGLGraphicsPipeline
	{
		public GLGraphicsPipeline (int programId,
			MgGraphicsPipelineCreateInfo info,
			GLProgramUniformBinder uniforms
		)
		{
			ProgramID = programId;
			UniformBinder = uniforms;

			PopulateVertexDefinition (info.VertexInputState);

			PopulatePipelineConstants (info.RasterizationState);

			PopulateCmdFallbacks (info.RasterizationState);

			PopulateInputAssembly (info.InputAssemblyState);

			PopulateDepthStencilState (info.DepthStencilState);

			PopulateDynamicStates (info.DynamicState);

			PopulateColorBlend (info.ColorBlendState);

			PopulateViewports (info.ViewportState);
		}

		public int ProgramID {get; private set;}

		public float MaxDepthBounds {
			get;
			private set;
		}

		public float MinDepthBounds {
			get;
			private set;
		}

		public GLCmdViewportParameter Viewports { get; private set; }
		public GLCmdScissorParameter Scissors { get; private set; }
		void PopulateViewports (MgPipelineViewportStateCreateInfo viewportState)
		{
			if (viewportState != null)
			{
				Viewports = new GLCmdViewportParameter (0, viewportState.Viewports);
				Scissors = new GLCmdScissorParameter (0, viewportState.Scissors);
			}
			else
			{
				// Should be 0,0 (width, height)
				Viewports = new GLCmdViewportParameter (0,
					new MgViewport [] { 
						//new MgViewport{ X = 0, Y = 0, Width = 0, Height = 0 }
					}
				);
				// 
				Scissors = new GLCmdScissorParameter (0,
					new MgRect2D [] { 
//						new MgRect2D
//						{ 
//							Offset = new MgOffset2D{ X = 0, Y = 0},
//							Extent = new MgExtent2D{Width = 0, Height = 0 },
//						}
					}
				);
			}
		}

		public MgColor4f BlendConstants { get; private set; }
		void PopulateColorBlend (MgPipelineColorBlendStateCreateInfo colorBlend)
		{
			if (colorBlend != null)
			{
				BlendConstants = colorBlend.BlendConstants;
			} 
			else
			{
				BlendConstants = new MgColor4f( 0f, 0f, 0f, 0f );	
			}
		}


		#region Pipeline constants

		static GLVertexAttributeInfo GetAttributeFormat (MgFormat format)
		{
			switch (format)
			{
			case MgFormat.R8_SINT:
				return new GLVertexAttributeInfo {
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};	
			case MgFormat.R8G8_SINT:
				return new GLVertexAttributeInfo {				
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R8G8B8_SINT:							
			case MgFormat.B8G8R8_SINT:	// TODO : swizzle	
				return new GLVertexAttributeInfo {				
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R8G8B8A8_SINT:
				return new GLVertexAttributeInfo {				
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};

			case MgFormat.R16_SINT:
				return new GLVertexAttributeInfo {					
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Short,
					Function = GLVertexAttribFunction.INT,						
				};
			case MgFormat.R16G16_SINT:
				return new GLVertexAttributeInfo {					
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Short,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R16G16B16_SINT:
				return new GLVertexAttributeInfo {				
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Short,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R16G16B16A16_SINT:
				return new GLVertexAttributeInfo {				
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Short,
					Function = GLVertexAttribFunction.INT,
				};

			case MgFormat.R32_SINT:
				return new GLVertexAttributeInfo {				
					 Size = 1,
					 IsNormalized = false,
					PointerType = GLVertexAttributeType.Int,
					Function = GLVertexAttribFunction.INT,
				 };	
			case MgFormat.R32G32_SINT:
				return new GLVertexAttributeInfo {					
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Int,
					Function = GLVertexAttribFunction.INT,
				 };	
			case MgFormat.R32G32B32_SINT:
				return new GLVertexAttributeInfo {					
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Int,
					Function = GLVertexAttribFunction.INT,
				 };	
			case MgFormat.R32G32B32A32_SINT:
				return new GLVertexAttributeInfo {					
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Int,
					Function = GLVertexAttribFunction.INT,
				 };

			case MgFormat.R8_UINT:
				return new GLVertexAttributeInfo {				
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedByte,
					Function = GLVertexAttribFunction.INT,
			 	};			
			case MgFormat.R8G8_UINT:
				return new GLVertexAttributeInfo {					
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedByte,
					Function = GLVertexAttribFunction.INT,
			 	};
			case MgFormat.R8G8B8_UINT:							
			case MgFormat.B8G8R8_UINT:	// TODO : swizzle	
				return new GLVertexAttributeInfo {					
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedByte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R8G8B8A8_UINT:
				return new GLVertexAttributeInfo {					
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedByte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R16_UINT:
				return new GLVertexAttributeInfo {	
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};				
			case MgFormat.R16G16_UINT:
				return new GLVertexAttributeInfo {	
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};	
			case MgFormat.R16G16B16_UINT:
				return new GLVertexAttributeInfo {
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
			 	};
			case MgFormat.R16G16B16A16_UINT:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R32_UINT:
				return new GLVertexAttributeInfo {	
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt,
					Function = GLVertexAttribFunction.INT,
				};	
			case MgFormat.R32G32_UINT:
				return new GLVertexAttributeInfo {	
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R32G32B32_UINT:
				return new GLVertexAttributeInfo {	
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt,
					Function = GLVertexAttribFunction.INT,
				};	
			case MgFormat.R32G32B32A32_UINT:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt,
					Function = GLVertexAttribFunction.INT,
				};

			case MgFormat.R32_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Float,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R32G32_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 2,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.Float,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R32G32B32_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Float,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R32G32B32A32_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 4,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.Float,
					Function = GLVertexAttribFunction.FLOAT,
				};
			case MgFormat.R16_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 1,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.HalfFloat,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R16G16_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 2,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.HalfFloat,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R16G16B16_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 3,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.HalfFloat,
					Function = GLVertexAttribFunction.FLOAT,
				};	
			case MgFormat.R16G16B16A16_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.HalfFloat,
					Function = GLVertexAttribFunction.FLOAT,
				};
			case MgFormat.R64_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 1,
				 	IsNormalized = false,
					PointerType = GLVertexAttributeType.Double,
					Function = GLVertexAttribFunction.DOUBLE,
				};
			case MgFormat.R64G64_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Double,
					Function = GLVertexAttribFunction.DOUBLE,
				};
			case MgFormat.R64G64B64_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Double,
					Function = GLVertexAttribFunction.DOUBLE,
				};
			case MgFormat.R64G64B64A64_SFLOAT:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Double,
					Function = GLVertexAttribFunction.DOUBLE,
				};

			// NORMALIZED

			case MgFormat.R8_SNORM:
				return new GLVertexAttributeInfo {	
					Size = 1,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};			
			case MgFormat.R8G8_SNORM:
				return new GLVertexAttributeInfo {	
					Size = 2,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R8G8B8_SNORM:							
			case MgFormat.B8G8R8_SNORM:	// TODO : swizzle	
				return new GLVertexAttributeInfo {	
					Size = 3,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R8G8B8A8_SNORM:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Byte,
					Function = GLVertexAttribFunction.INT,
				};

			case MgFormat.R16_UNORM:
				return new GLVertexAttributeInfo {
					Size = 1,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};				
			case MgFormat.R16G16_UNORM:
				return new GLVertexAttributeInfo {	
					Size = 2,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};	
			case MgFormat.R16G16B16_UNORM:
				return new GLVertexAttributeInfo {	
					Size = 3,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.R16G16B16A16_UNORM:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.UnsignedShort,
					Function = GLVertexAttribFunction.INT,
				};		

			// A2B10G10R10
			case MgFormat.A2B10G10R10_SINT_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Int2101010Rev,
					Function = GLVertexAttribFunction.INT,
				};
			case MgFormat.A2B10G10R10_SNORM_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.Int2101010Rev,
					Function = GLVertexAttribFunction.FLOAT,
				};

			case MgFormat.A2B10G10R10_SSCALED_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.Int2101010Rev,
					Function = GLVertexAttribFunction.FLOAT,
				};

			case MgFormat.A2B10G10R10_UINT_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt2101010Rev,
					Function = GLVertexAttribFunction.FLOAT,
				};

			case MgFormat.A2B10G10R10_UNORM_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = true,
					PointerType = GLVertexAttributeType.UnsignedInt2101010Rev,
					Function = GLVertexAttribFunction.FLOAT,
				};

			case MgFormat.A2B10G10R10_USCALED_PACK32:
				return new GLVertexAttributeInfo {	
					Size = 4,
					IsNormalized = false,
					PointerType = GLVertexAttributeType.UnsignedInt2101010Rev,
					Function = GLVertexAttribFunction.FLOAT,
				};
			
			default:
				throw new NotSupportedException ();
			}
		}

		public GLVertexBufferBinder VertexInput { get; private set; }

		void PopulateVertexDefinition (MgPipelineVertexInputStateCreateInfo vertexInput)
		{
			var perInstance = new SortedList<uint, GLVertexBufferBinding> ();

			foreach (var vbuf in vertexInput.VertexBindingDescriptions)
			{
				var def = new GLVertexBufferBinding (vbuf.Binding, vbuf.InputRate, vbuf.Stride);
				perInstance.Add (def.Binding, def);
			}

			var bindings = new GLVertexBufferBinding[perInstance.Values.Count];
			perInstance.Values.CopyTo (bindings, 0);

			var attributes = new List<GLVertexInputAttribute> ();
			foreach (var description in vertexInput.VertexAttributeDescriptions)
			{
				var binding = bindings[description.Binding];

				var elementInfo = GetAttributeFormat(description.Format);

				var divisor = (binding.InputRate == MgVertexInputRate.INSTANCE) ? 1 : 0;

				var attribute = new GLVertexInputAttribute (
					description.Binding,
					description.Location,
					description.Offset,
					binding.Stride,
					divisor,
					elementInfo
				);

				attributes.Add (attribute);
			}

			VertexInput = new GLVertexBufferBinder (bindings, attributes.ToArray ());
		}

		public GLGraphicsPipelineDynamicStateFlagBits DynamicsStates { get; private set; }
		void PopulateDynamicStates (MgPipelineDynamicStateCreateInfo dynamicStates)
		{
			GLGraphicsPipelineDynamicStateFlagBits flags = 0;

			if (dynamicStates != null)
			{
				foreach (var state in dynamicStates.DynamicStates)
				{
					switch (state)
					{
					case MgDynamicState.VIEWPORT:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT;
						break;
					case MgDynamicState.SCISSOR:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.SCISSOR;
						break;
					case MgDynamicState.BLEND_CONSTANTS:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS;
						break;
					case MgDynamicState.STENCIL_COMPARE_MASK:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK;
						break;
					case MgDynamicState.STENCIL_REFERENCE:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE;
						break;
					case MgDynamicState.STENCIL_WRITE_MASK:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK;
						break;
					case MgDynamicState.LINE_WIDTH:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH;
						break;
					case MgDynamicState.DEPTH_BIAS:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS;
						break;
					case MgDynamicState.DEPTH_BOUNDS:
						flags |= GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS;
						break;					
					}
				}
				DynamicsStates = flags;
			}
		}

		public QueueDrawItemBitFlags Flags { get; private set; }
		public GLQueueStencilState StencilState { get; private set; }
		public GLQueueDepthState DepthState {
			get;
			private set;
		}

		public GLQueueStencilMasks Front {
			get;
			private set;
		}

		public GLQueueStencilMasks Back {
			get;
			private set;
		}

		/// <summary>
		/// Because depthStencilState is optional 
		/// </summary>
		/// <param name="depthStencilState">Depth stencil state.</param>
		void PopulateDepthStencilState (MgPipelineDepthStencilStateCreateInfo depthStencilState)
		{
			QueueDrawItemBitFlags flags = 0;

			// VULKAN DOC : The scissor test is always performed.
			//  Applications can effectively disable the scissor test by specifying a
			//  scissor rectangle that encompasses the entire framebuffer.
			flags |= QueueDrawItemBitFlags.ScissorTestEnabled;

			if (depthStencilState != null)
			{
				flags |= (depthStencilState.DepthTestEnable) ? QueueDrawItemBitFlags.DepthBufferEnabled : 0;
				flags |= (depthStencilState.StencilTestEnable) ? QueueDrawItemBitFlags.StencilEnabled : 0;
				flags |= (depthStencilState.DepthWriteEnable) ? QueueDrawItemBitFlags.DepthBufferWriteEnabled : 0;

				flags |= QueueDrawItemBitFlags.TwoSidedStencilMode;

				// SAME STENCIL MODE USED FOR FRONT AND BACK
				Front = new GLQueueStencilMasks {
					CompareMask = (int)depthStencilState.Front.CompareMask,
					WriteMask = (int)depthStencilState.Front.WriteMask,
					Reference = (int)depthStencilState.Front.Reference,
				};

				Back = new GLQueueStencilMasks 
				{
					CompareMask = (int)depthStencilState.Back.CompareMask,
					WriteMask = (int)depthStencilState.Back.WriteMask,
					Reference = (int)depthStencilState.Back.Reference,
				};

				StencilState = new GLQueueStencilState {

					FrontStencilFunction = depthStencilState.Front.CompareOp,
					FrontStencilPass = depthStencilState.Front.PassOp,
					FrontStencilFail = depthStencilState.Front.FailOp,
					FrontDepthBufferFail = depthStencilState.Front.DepthFailOp,

					BackStencilPass = depthStencilState.Back.PassOp,
					BackStencilFail = depthStencilState.Back.FailOp,
					BackDepthBufferFail = depthStencilState.Back.DepthFailOp,
					BackStencilFunction = depthStencilState.Back.CompareOp,
				};

				DepthState = new GLQueueDepthState {
					DepthBufferFunction = depthStencilState.DepthCompareOp,
				};

				MaxDepthBounds = depthStencilState.MaxDepthBounds;
				MinDepthBounds = depthStencilState.MinDepthBounds;
			} 
			else
			{
				flags |= QueueDrawItemBitFlags.DepthBufferEnabled;
				flags |= QueueDrawItemBitFlags.DepthBufferWriteEnabled;

				// Based on OpenGL defaults
				//flags |= (depthStencilState.StencilTestEnable) ? QueueDrawItemBitFlags.StencilEnabled : 0;
				//flags |= QueueDrawItemBitFlags.TwoSidedStencilMode;

				//			DisableStencilBuffer ();
				//			SetStencilWriteMask (~0);
				//			SetStencilFunction (MgCompareOp.ALWAYS, ~0, int.MaxValue);
				//			SetStencilOperation (MgStencilOp.KEEP, MgStencilOp.KEEP, MgStencilOp.KEEP);
				//
				//			void SetStencilFunction(
				//				MgCompareOp stencilFunction,
				//				int referenceStencil,
				//				int stencilMask);

				//			void SetStencilOperation(
				//				MgStencilOp stencilFail,
				//				MgStencilOp stencilDepthBufferFail,
				//				MgStencilOp stencilPass);

				// SAME STENCIL MODE USED FOR FRONT AND BACK
				Front = new GLQueueStencilMasks
				{
					CompareMask = int.MaxValue,
					WriteMask = ~0,
					Reference = ~0,
				};

				Back = new GLQueueStencilMasks 
				{
					CompareMask = int.MaxValue,
					WriteMask = ~0,
					Reference = ~0,
				};

				StencilState = new GLQueueStencilState {

					FrontStencilFunction = MgCompareOp.ALWAYS,
					FrontStencilPass = MgStencilOp.KEEP,
					FrontStencilFail = MgStencilOp.KEEP,
					FrontDepthBufferFail = MgStencilOp.KEEP,

					BackStencilFunction = MgCompareOp.ALWAYS,
					BackStencilPass = MgStencilOp.KEEP,
					BackStencilFail = MgStencilOp.KEEP,
					BackDepthBufferFail = MgStencilOp.KEEP,
				};

				DepthState = new GLQueueDepthState {
					DepthBufferFunction = MgCompareOp.LESS,
				};

				MinDepthBounds = 0f;
				MaxDepthBounds = 1f;

			}

			Flags |= flags;
		}

		public MgPrimitiveTopology Topology {
			get;
			private set;
		}

		public bool PrimitiveRestartEnable { get; private set; }

		void PopulateInputAssembly (MgPipelineInputAssemblyStateCreateInfo inputAssemblyState)
		{
			Topology = inputAssemblyState.Topology;
			PrimitiveRestartEnable = inputAssemblyState.PrimitiveRestartEnable;
		}

		public MgPolygonMode PolygonMode {
			get;
			private set;
		}

		public bool RasterizerDiscardEnable {
			get;
			private set;
		}

		public bool DepthClampEnable {
			get;
			private set;
		}

		void PopulatePipelineConstants (MgPipelineRasterizationStateCreateInfo rasterization)
		{
			if (rasterization != null)
			{
				QueueDrawItemBitFlags flags = 0;

				flags |= ((rasterization.CullMode & MgCullModeFlagBits.FRONT_AND_BACK) > 0) ? QueueDrawItemBitFlags.CullingEnabled : 0;
				flags |= ((rasterization.CullMode & MgCullModeFlagBits.FRONT_BIT) > 0) ? QueueDrawItemBitFlags.CullFrontFaces : 0;
				flags |= ((rasterization.CullMode & MgCullModeFlagBits.BACK_BIT) > 0) ? QueueDrawItemBitFlags.CullBackFaces : 0;
				flags |= (rasterization.FrontFace == MgFrontFace.COUNTER_CLOCKWISE) ? QueueDrawItemBitFlags.UseCounterClockwiseWindings : 0;

				this.PolygonMode = rasterization.PolygonMode;
				this.RasterizerDiscardEnable = rasterization.RasterizerDiscardEnable;

				flags |= (rasterization.DepthBiasEnable) ? QueueDrawItemBitFlags.DepthBiasEnabled : 0;

				this.DepthClampEnable = rasterization.DepthClampEnable;

				Flags |= flags;
			}
			else
			{
				// https://www.opengl.org/sdk/docs/man/html/glPolygonOffset.xhtml
				DepthBiasConstantFactor = 0;
				DepthBiasSlopeFactor = 0;

				QueueDrawItemBitFlags flags = 0;
				flags |= QueueDrawItemBitFlags.CullingEnabled;
				// DISABLED flags |= QueueDrawItemBitFlags.CullFrontFaces;
				flags |= QueueDrawItemBitFlags.CullBackFaces;
				flags |= QueueDrawItemBitFlags.UseCounterClockwiseWindings;

				// https://www.opengl.org/sdk/docs/man/html/glPolygonMode.xhtml
				PolygonMode = MgPolygonMode.FILL;

				Flags |= flags;
			}
		}

		#endregion

		#region Command buffer rasterization fallbacks

		void PopulateCmdFallbacks (MgPipelineRasterizationStateCreateInfo rasterization)
		{
			if (rasterization != null)
			{
				// RASTERIZATION DEFAULTS
				DepthBiasConstantFactor = rasterization.DepthBiasConstantFactor;
				DepthBiasClamp = rasterization.DepthBiasClamp;
				DepthBiasSlopeFactor = rasterization.DepthBiasSlopeFactor;
				LineWidth = rasterization.LineWidth;
			}
			else
			{
				// https://www.opengl.org/sdk/docs/man/html/glPolygonOffset.xhtml
				DepthBiasConstantFactor = 0;
				DepthBiasSlopeFactor = 0;
				// https://www.opengl.org/registry/specs/EXT/polygon_offset_clamp.txt
				DepthBiasClamp = 0;
				LineWidth = 1f;
			}
		}

		public float DepthBiasSlopeFactor {
			get;
			private set;
		}

		public float LineWidth {
			get;
			private set;
		}

		public float DepthBiasClamp {
			get;
			private set;
		}

		public float DepthBiasConstantFactor {
			get;
			private set;
		}
		
		#endregion

		public GLProgramUniformBinder UniformBinder {
			get;
			private set;
		}

		#region IMgPipeline implementation
		private bool mIsDisposed = false;
		public void DestroyPipeline (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			GL.DeleteProgram (ProgramID);

			mIsDisposed = true;
		}

		#endregion
	}
}

