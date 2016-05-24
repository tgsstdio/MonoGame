using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLGraphicsPipeline : IMgPipeline
	{
		public int ProgramId {get; private set;}
		public GLGraphicsPipeline (int programId, MgPipelineRasterizationStateCreateInfo rasterization)
		{
			ProgramId = programId;

			PopulatePipelineConstants (rasterization);

			PopulateCmdFallbacks (rasterization);
		}

		#region Pipeline constants

		public MgCullModeFlagBits CullMode {
			get;
			private set;
		}

		public MgFrontFace FrontFace {
			get;
			set;
		}

		public MgPolygonMode PolygonMode {
			get;
			set;
		}

		public bool RasterizerDiscardEnable {
			get;
			set;
		}

		public bool DepthBiasEnable {
			get;
			set;
		}

		public bool DepthClampEnable {
			get;
			set;
		}

		void PopulatePipelineConstants (MgPipelineRasterizationStateCreateInfo rasterization)
		{
			this.CullMode = rasterization.CullMode;
			this.FrontFace = rasterization.FrontFace;
			this.PolygonMode = rasterization.PolygonMode;
			this.RasterizerDiscardEnable = rasterization.RasterizerDiscardEnable;
			this.DepthBiasEnable = rasterization.DepthBiasEnable;
			this.DepthClampEnable = rasterization.DepthClampEnable;
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

