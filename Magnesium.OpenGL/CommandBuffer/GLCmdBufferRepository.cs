using System;

namespace Magnesium.OpenGL
{
	public class GLCmdBufferRepository
	{
		public GLCmdBufferRepository ()
		{
			GraphicsPipelines = new GLCmdBufferStore<GLGraphicsPipeline>();
			Viewports = new GLCmdBufferStore<GLCmdViewportParameter>();
			DescriptorSets = new GLCmdBufferStore<GLCmdDescriptorSetParameter>();
			Scissors = new GLCmdBufferStore<GLCmdScissorParameter>();
			IndexBuffers = new GLCmdBufferStore<GLCmdIndexBufferParameter>();
			VertexBuffers = new GLCmdBufferStore<GLCmdVertexBufferParameter>();
		}

		public GLCmdBufferStore<GLCmdVertexBufferParameter> VertexBuffers { get; private set;	}
		public GLCmdBufferStore<GLCmdIndexBufferParameter> IndexBuffers { get; private set; }
		public GLCmdBufferStore<GLGraphicsPipeline> GraphicsPipelines { get; private set; }
		public GLCmdBufferStore<GLCmdViewportParameter> Viewports { get; private set; }
		public GLCmdBufferStore<GLCmdDescriptorSetParameter> DescriptorSets { get; private set; }
		public GLCmdBufferStore<GLCmdScissorParameter> Scissors {get; private set;}

		public void MapRepositoryFields(ref GLCmdDrawCommand command)
		{
			command.DescriptorSets = DescriptorSets.LastIndex ();
			command.IndexBuffer = IndexBuffers.LastIndex ();
			command.VertexBuffer = VertexBuffers.LastIndex ();
			command.Scissors = Scissors.LastIndex ();
			command.Viewports = Viewports.LastIndex ();

			GLGraphicsPipeline pipeline;
			if (GraphicsPipelines.LastValue (out pipeline))
			{
				// add defaults
			}
			command.Pipeline = GraphicsPipelines.LastIndex ();
		}

		public void Clear ()
		{
			GraphicsPipelines.Clear ();
			Viewports.Clear ();
			DescriptorSets.Clear ();
			Scissors.Clear ();
			IndexBuffers.Clear ();
			VertexBuffers.Clear ();

			// clear nullable fields
			Reference = null;
			ReferenceFace = null;
			WriteFace = null;
			WriteMask = null;
			CompareMask = null;
			CompareFace = null;
			MinDepthBounds = null;
			MaxDepthBounds = null;
			DepthBiasConstantFactor = null;
			DepthBiasClamp = null;
			LineWidth = null;
		}

		#region Nullable fields 

		public uint? Reference {
			get;
			set;
		}

		public MgStencilFaceFlagBits? ReferenceFace {
			get;
			set;
		}

		public MgStencilFaceFlagBits? WriteFace {
			get;
			set;
		}

		public uint? WriteMask {
			get;
			set;
		}

		public uint? CompareMask {
			get;
			set;
		}

		public MgStencilFaceFlagBits? CompareFace {
			get;
			set;
		}

		public float? MinDepthBounds {
			get;
			set;
		}

		public float? MaxDepthBounds {
			get;
			set;
		}

		public float? DepthBiasConstantFactor {
			get;
			set;
		}

		public float? DepthBiasClamp {
			get;
			set;
		}

		public float DepthBiasSlopeFactor {
			get;
			set;
		}

		public float? LineWidth {
			get;
			set;
		}

		public float[] BlendConstants { get; set; }

		#endregion
	}
}

