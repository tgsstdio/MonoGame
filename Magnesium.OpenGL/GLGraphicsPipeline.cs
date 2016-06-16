using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLGraphicsPipeline : IMgPipeline
	{
		public GLGraphicsPipeline (int programId,
			MgPipelineRasterizationStateCreateInfo rasterizationState, 
			MgPipelineInputAssemblyStateCreateInfo inputAssemblyState,
			MgPipelineDepthStencilStateCreateInfo depthStencilState
		)
		{
			ProgramId = programId;

			PopulatePipelineConstants (rasterizationState);

			PopulateCmdFallbacks (rasterizationState);

			PopulateInputAssembly (inputAssemblyState);

			PopulateDepthStencilState (depthStencilState);
		}

		public int ProgramId {get; private set;}

		#region Pipeline constants

		public QueueDrawItemBitFlags Flags { get; private set; }
		public GLQueueStencilState StencilState { get; private set; }
		public GLQueueDepthState DepthState {
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

				StencilState = new GLQueueStencilState {
					// SAME STENCIL MODE USED FOR FRONT AND BACK
					StencilMask = (int)depthStencilState.Front.CompareMask,
					StencilWriteMask = (int)depthStencilState.Front.WriteMask,
					ReferenceStencil = (int)depthStencilState.Front.Reference,

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

				StencilState = new GLQueueStencilState {
					// SAME STENCIL MODE USED FOR FRONT AND BACK
					StencilMask = int.MaxValue,
					StencilWriteMask = ~0,
					ReferenceStencil = ~0,

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
			}

			Flags |= flags;
		}

		public MgPrimitiveTopology Topology {
			get;
			private set;
		}

		void PopulateInputAssembly (MgPipelineInputAssemblyStateCreateInfo inputAssemblyState)
		{
			Topology = inputAssemblyState.Topology;
		}

		public MgPolygonMode PolygonMode {
			get;
			set;
		}

		public bool RasterizerDiscardEnable {
			get;
			set;
		}

		public bool DepthClampEnable {
			get;
			set;
		}


		public GLQueueRasterizerState RasterizationState { get; private set; }
		void PopulatePipelineConstants (MgPipelineRasterizationStateCreateInfo rasterization)
		{
			QueueDrawItemBitFlags flags = 0;

			flags |= ((rasterization.CullMode & MgCullModeFlagBits.FRONT_AND_BACK) > 0) ? QueueDrawItemBitFlags.CullingEnabled : 0;
			flags |= ((rasterization.CullMode & MgCullModeFlagBits.FRONT_BIT) > 0) ? QueueDrawItemBitFlags.CullFrontFaces : 0;
			flags |= ((rasterization.CullMode & MgCullModeFlagBits.BACK_BIT) > 0) ? QueueDrawItemBitFlags.CullBackFaces : 0;
			flags |= (rasterization.FrontFace == MgFrontFace.COUNTER_CLOCKWISE) ? QueueDrawItemBitFlags.UseCounterClockwiseWindings : 0; 


			this.PolygonMode = rasterization.PolygonMode;
			this.RasterizerDiscardEnable = rasterization.RasterizerDiscardEnable;

			flags |= (rasterization.DepthBiasEnable) ? QueueDrawItemBitFlags.DepthBiasEnabled : 0;

			RasterizationState = new GLQueueRasterizerState {
				DepthBias = rasterization.DepthBiasConstantFactor,
				SlopeScaleDepthBias = rasterization.DepthBiasSlopeFactor,
			};

			this.DepthClampEnable = rasterization.DepthClampEnable;

			Flags |= flags;
		}

		#endregion

		#region Command buffer rasterization fallbacks

		void PopulateCmdFallbacks (MgPipelineRasterizationStateCreateInfo rasterization)
		{
			// RASTERIZATION DEFAULTS
			DepthBiasConstantFactor = rasterization.DepthBiasConstantFactor;
			DepthBiasClamp = rasterization.DepthBiasClamp;
			DepthBiasSlopeFactor = rasterization.DepthBiasSlopeFactor;
			LineWidth = rasterization.LineWidth;
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
			set;
		}



		#region IMgPipeline implementation
		private bool mIsDisposed = false;
		public void DestroyPipeline (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			GL.DeleteProgram (ProgramId);

			mIsDisposed = true;
		}

		#endregion
	}
}

