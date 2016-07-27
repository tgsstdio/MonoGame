﻿using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
	public class Transformer : ICmdBufferInstructionSetComposer
	{
		#region ICmdBufferInstructionSetComposer implementation

		public CmdBufferInstructionSet Compose (GLCmdBufferRepository repository, IEnumerable<GLCmdRenderPassCommand> passes)
		{	
			if (repository == null || passes == null)
			{
				// EARLY EXIT
				return BuildInstructionSet ();
			}

			bool isFirst = true;
			foreach (var pass in passes)
			{
				foreach (var command in pass.DrawCommands)
				{
					InitialiseDrawItem(repository, pass, command);
				}
			}
			return BuildInstructionSet ();
		}

		CmdBufferInstructionSet BuildInstructionSet ()
		{
			var output = new CmdBufferInstructionSet {
				DrawItems = DrawItems.ToArray(),
				Pipelines = Pipelines.ToArray (),
				VBOs = VBOs.ToArray (),
				ClearValues = ClearValues.ToArray (),
				ColorBlends = ColorBlends.ToArray (),
				Viewports = Viewports.Items.ToArray (),
				BackCompareMasks = BackCompareMasks.Items.ToArray (),
				FrontCompareMasks = FrontCompareMasks.Items.ToArray (),
				BackReferences = BackReferences.Items.ToArray (),
				FrontReferences = FrontReferences.Items.ToArray (),
				BackWriteMasks = BackWriteMasks.Items.ToArray (),
				FrontWriteMasks = FrontWriteMasks.Items.ToArray (),
				LineWidths = LineWidths.Items.ToArray (),
				//Scissors = Scissors.Items.ToArray (),
				BlendConstants = BlendConstants.Items.ToArray (),
				//DescriptorSets = DescriptorSets.Items.ToArray (),
				DepthBias = DepthBias.ToArray (),
				DepthBounds = DepthBounds.Items.ToArray (),
			};

			return output;
		}

		#endregion

		ICmdVBOCapabilities mVBO;

		public Transformer (ICmdVBOCapabilities vbo)
		{
			mVBO = vbo;
			VBOs = new List<GLCmdVertexBufferObject>();
			DepthBias = new List<GLCmdDepthBiasParameter> ();
			DrawItems = new List<GLCmdBufferDrawItem> ();
			Pipelines = new List<GLCmdBufferPipelineItem> ();
			ClearValues = new List<GLCmdClearValuesParameter> ();
			ColorBlends = new List<GLQueueRendererColorBlendState> ();

			BlendConstants = new TransformerStore<MgColor4f> (
				GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS,
				(d) => d.BlendConstants,
				(r, i) => r.BlendConstants.At(i),
				(gp) => gp.BlendConstants
			);

			DepthBounds = new TransformerStore<GLCmdDepthBoundsParameter> (
				GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS,
				(d) => d.DepthBounds,
				(r, i) => r.DepthBounds.At(i),
				(gp) => new GLCmdDepthBoundsParameter{
					MinDepthBounds = gp.MinDepthBounds,
					MaxDepthBounds = gp.MaxDepthBounds}
			);

			LineWidths = new TransformerStore<float> (
				GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH,
				(d) => d.LineWidth,
				(r, i) => r.LineWidths.At(i),
				(gp) => gp.LineWidth
			);

			BackCompareMasks = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK,
				(d) => d.BackCompareMask,
				(r, i) => r.BackCompareMasks.At (i),
				(gp) => gp.Back.CompareMask);

			FrontCompareMasks = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK,
				(d) => d.FrontCompareMask,
				(r, i) => r.FrontCompareMasks.At (i),
				(gp) => gp.Front.CompareMask);

			FrontWriteMasks = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK,
				(d) => d.FrontWriteMask,
				(r, i) => r.FrontWriteMasks.At (i),
				(gp) => gp.Front.WriteMask);

			BackWriteMasks = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK,
				(d) => d.BackWriteMask,
				(r, i) => r.BackWriteMasks.At (i),
				(gp) => gp.Back.WriteMask);

			FrontReferences = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE,
				(d) => d.FrontReference,
				(r, i) => r.FrontReferences.At (i),
				(gp) => gp.Front.Reference);

			BackReferences = new TransformerStore<int> (
				GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE,
				(d) => d.BackReference,
				(r, i) => r.BackReferences.At (i),
				(gp) => gp.Back.Reference);

			Viewports = new TransformerStore<GLCmdViewportParameter> (
				GLGraphicsPipelineDynamicStateFlagBits.VIEWPORT,
				(d) => d.Viewports,
				(r, i) => r.Viewports.At(i),
				(gp) => gp.Viewports);

			Scissors = new TransformerStore<GLCmdScissorParameter> (
				GLGraphicsPipelineDynamicStateFlagBits.SCISSOR,
				(d) => d.Scissors,
				(r, i) => r.Scissors.At(i),
				(gp) => gp.Scissors);
		}

		public TransformerStore<GLCmdScissorParameter> Scissors {
			get;
			private set;
		}

		public TransformerStore<GLCmdViewportParameter> Viewports {
			get;
			private set;
		}

		public TransformerStore<int> FrontReferences {
			get;
			private set;
		}

		public TransformerStore<int> BackReferences {
			get;
			private set;
		}

		public TransformerStore<int> FrontWriteMasks {
			get;
			private set;
		}

		public TransformerStore<int> BackWriteMasks {
			get;
			private set;
		}

		public TransformerStore<int> FrontCompareMasks {
			get;
			private set;
		}

		public TransformerStore<int> BackCompareMasks {
			get;
			private set;
		}

		public TransformerStore<float> LineWidths {
			get;
			private set;
		}

		public TransformerStore<GLCmdDepthBoundsParameter> DepthBounds {
			get;
			private set;
		}

		public TransformerStore<MgColor4f> BlendConstants {
			get;
			private set;
		}

		public List<GLCmdBufferDrawItem> DrawItems {
			get;
			private set;
		}

		public List<GLCmdDepthBiasParameter> DepthBias {
			get;
			private set;
		}

		public List<GLCmdVertexBufferObject> VBOs { get; private set; }
		public void Initialise(IGLCmdBufferRepository userSettings)
		{
			InsertNullVBO ();

			if (userSettings.Viewports.Count == 0)
			{
				Viewports.Items.Add (new GLCmdViewportParameter (0, new MgViewport[]{ }));
			}

			if (userSettings.Scissors.Count == 0)
			{
				Scissors.Items.Add (new GLCmdScissorParameter (0, new MgRect2D[]{ }));
			}
		}

		void InsertNullVBO ()
		{
			VBOs.Add (new GLCmdVertexBufferObject (0, 0, null, null));
		}

		GLCmdVertexBufferObject GenerateVBO (IGLCmdBufferRepository repository, GLCmdDrawCommand command, IGLGraphicsPipeline pipeline)
		{
			var vertexData = repository.VertexBuffers.At(command.VertexBuffer.Value);
			var noOfBindings = pipeline.VertexInput.Bindings.Length;
			var bufferIds = new int[noOfBindings];
			var offsets = new long[noOfBindings];

			for (uint i = 0; i < vertexData.pBuffers.Length; ++i)
			{
				uint index = i + vertexData.firstBinding;
				var buffer = vertexData.pBuffers [index] as GLIndirectBuffer;
				// SILENT error
				if (buffer.BufferType == GLMemoryBufferType.VERTEX)
				{
					bufferIds [i] = buffer.BufferId;
					offsets [i] = (vertexData.pOffsets != null) ? (long)vertexData.pOffsets [i] : 0L;
				}
				else
				{
					bufferIds [i] = 0;
					offsets [i] = 0;
				}
			}

			int vbo = mVBO.GenerateVBO ();
			foreach (var attribute in pipeline.VertexInput.Attributes)
			{	
				var bufferId = bufferIds [attribute.Binding];
				var binding = pipeline.VertexInput.Bindings [attribute.Binding];
				mVBO.AssociateBufferToLocation (vbo, attribute.Location, bufferId, offsets[attribute.Binding], binding.Stride);
				// GL.VertexArrayVertexBuffer (vertexArray, location, bufferId, new IntPtr (offset), (int)binding.Stride);

				if (attribute.Function == GLVertexAttribFunction.FLOAT)
				{
					// direct state access
					mVBO.BindFloatVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);

					//GL.VertexArrayAttribFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.IsNormalized, attribute.Offset);
				}
				else if (attribute.Function == GLVertexAttribFunction.INT)
				{
					mVBO.BindIntVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);

					//GL.VertexArrayAttribIFormat (vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
				}
				else if (attribute.Function == GLVertexAttribFunction.DOUBLE)
				{
					mVBO.BindDoubleVertexAttribute(vbo, attribute.Location, attribute.Size, attribute.PointerType, attribute.Offset);
					//GL.VertexArrayAttribLFormat (vbo, attribute.Location, attribute.Size, (All)attribute.PointerType, attribute.Offset);
				}
				mVBO.SetupVertexAttributeDivisor(vbo, attribute.Location, attribute.Divisor);
				//GL.VertexArrayBindingDivisor (vbo, attribute.Location, attribute.Divisor);
			}

			if (command.IndexBuffer.HasValue)
			{
				var indexData = repository.IndexBuffers.At(command.IndexBuffer.Value);
				var indexBuffer = indexData.buffer as GLIndirectBuffer;
				if (indexBuffer != null && indexBuffer.BufferType == GLMemoryBufferType.INDEX)
				{
					mVBO.BindIndexBuffer  (vbo, indexBuffer.BufferId);
					//GL.VertexArrayElementBuffer (vbo, indexBuffer.BufferId);
				}
			}

			return new GLCmdVertexBufferObject(vbo, command.VertexBuffer.Value, command.IndexBuffer, mVBO);
		}

		GLCmdVertexBufferObject GetLastVBO ()
		{
			return VBOs [VBOs.Count - 1];
		}

		// NEED TO MERGE 
		public int ExtractVertexBuffer(IGLCmdBufferRepository userSettings, GLCmdDrawCommand command)
		{				
			if (command.Pipeline.HasValue && command.VertexBuffer.HasValue)
			{				
				var currentVertexArray = GetLastVBO ();
				var pipeline = userSettings.GraphicsPipelines.At (command.Pipeline.Value);

				// create new vbo
				if (currentVertexArray == null)
				{
					currentVertexArray = GenerateVBO (userSettings, command, pipeline);
					VBOs.Add (currentVertexArray);
				} else
				{
					bool useSameBuffers = (command.IndexBuffer.HasValue)
						? currentVertexArray.Matches (
							command.VertexBuffer.Value,
							command.IndexBuffer.Value)
						: currentVertexArray.Matches (
							command.VertexBuffer.Value);

					if (!useSameBuffers)
					{
						currentVertexArray = GenerateVBO (userSettings, command, pipeline);
						VBOs.Add (currentVertexArray);
					}
				}

				return currentVertexArray.VBO;
			} 
			else
			{
				return 0;
			}
		}

		static bool CanOverrideDepthBias (GLCmdDrawCommand command, IGLGraphicsPipeline pipeline)
		{
			return command.DepthBias.HasValue && (pipeline.DynamicsStates & GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS) == GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS;
		}

		byte ExtractDepthBias (IGLCmdBufferRepository repo, IGLGraphicsPipeline pipeline, GLCmdDrawCommand command)
		{
			if (pipeline == null)
			{	
				throw new ArgumentNullException ("pipeline");
			}

			var currentValue = CanOverrideDepthBias (command, pipeline)
				? repo.DepthBias.At (command.DepthBias.Value)
				: new GLCmdDepthBiasParameter {
					DepthBiasClamp = pipeline.DepthBiasClamp,
					DepthBiasConstantFactor = pipeline.DepthBiasConstantFactor,
					DepthBiasSlopeFactor = pipeline.DepthBiasSlopeFactor,
				};

			// NONE EXISTS

			var count = DepthBias.Count;

			if (count == 0)
			{
				// USE DEFAULT
				DepthBias.Add (currentValue);
				return 0;
			}
			else
			{	
				var topIndex = count - 1;
				var lastValue = DepthBias [topIndex];

				if (currentValue.Equals (lastValue))
				{
					return (byte)topIndex;
				}
				else
				{
					DepthBias.Add (currentValue);
					return (byte)count;
				}
			}			
		}

		static IntPtr ExtractIndirectBuffer (IMgBuffer buffer)
		{
			var indirectBuffer = buffer as IGLIndirectBuffer;
			return (indirectBuffer != null && indirectBuffer.BufferType == GLMemoryBufferType.INDIRECT) ? indirectBuffer.Source : IntPtr.Zero;
		}

		public static GLCommandBufferFlagBits ExtractPolygonMode (IGLGraphicsPipeline pipeline)
		{
			return (pipeline.PolygonMode == MgPolygonMode.LINE) ? GLCommandBufferFlagBits.AsLinesMode : ((pipeline.PolygonMode == MgPolygonMode.POINT) ? GLCommandBufferFlagBits.AsPointsMode : 0);
		}

		public GLCmdBufferDrawItem GenerateDrawItem (GLCmdRenderPassCommand pass, IGLGraphicsPipeline pipeline, GLCmdDrawCommand command)
		{
			var item = new GLCmdBufferDrawItem {
				Command = ExtractPolygonMode (pipeline), 
				ProgramID = pipeline.ProgramID,
				Topology = pipeline.Topology,
			};

			item.Pipeline = ExtractGraphicsPipeline (pass, pipeline);

			if (command.DrawIndexedIndirect != null)
			{
				item.Command |= GLCommandBufferFlagBits.CmdDrawIndexedIndirect;
				var drawCommand = command.DrawIndexedIndirect;
				item.Buffer = ExtractIndirectBuffer (drawCommand.buffer);
				item.Count = drawCommand.drawCount;
				item.Offset = drawCommand.offset;
				item.Stride = drawCommand.stride;
			}
			else if (command.DrawIndirect != null)
			{
				item.Command |= GLCommandBufferFlagBits.CmdDrawIndirect;
				var drawCommand = command.DrawIndirect;
				item.Buffer = ExtractIndirectBuffer (drawCommand.buffer);
				item.Count = drawCommand.drawCount;
				item.Offset = drawCommand.offset;
				item.Stride = drawCommand.stride;
			}
			else if (command.DrawIndexed != null)
			{
				item.Command |= GLCommandBufferFlagBits.CmdDrawIndexed;
				var drawCommand = command.DrawIndexed;
				item.First = drawCommand.firstIndex;
				item.FirstInstance = drawCommand.firstInstance;
				item.Count = drawCommand.indexCount;
				item.VertexOffset = drawCommand.vertexOffset;
				item.InstanceCount = drawCommand.instanceCount;
			}
			else if (command.Draw != null)
			{
				var drawCommand = command.Draw;
				item.FirstInstance = drawCommand.firstInstance;
				item.First = drawCommand.firstVertex;
				item.InstanceCount = drawCommand.instanceCount;
				item.Count = drawCommand.vertexCount;
			}
			else
			{
				throw new InvalidOperationException ();
			}
			return item;
		}

		public List<GLCmdClearValuesParameter> ClearValues { get; private set; }
		byte GenerateClearValues (GLCmdRenderPassCommand pass)
		{
			if (pass == null)
			{
				throw new ArgumentNullException ("pass");
			}

			if (pass.Origin == null)
			{
				throw new ArgumentNullException ("pass.Origin");
			}

			var glPass = pass.Origin as IGLRenderPass;

			if (glPass == null)
			{
				throw new InvalidCastException ();
			}

			var noOfAttachments = glPass.AttachmentFormats == null ? 0 : glPass.AttachmentFormats.Length;
			var noOfClearValues = pass.ClearValues == null ? 0 : pass.ClearValues.Length;

			var finalLength = Math.Min (noOfAttachments, noOfClearValues);

			var finalValues = new List<GLClearValueArrayItem> ();
			for (var i = 0; i < finalLength; ++i)
			{
				finalValues.Add (new GLClearValueArrayItem {
					Attachment = glPass.AttachmentFormats [i],
					Value = pass.ClearValues [i]
				});
			}

			var currentValue = new GLCmdClearValuesParameter {
				Attachments = finalValues.ToArray(),
			};

			var count = ClearValues.Count;
			if (count == 0)
			{
				// USE DEFAULT
				ClearValues.Add (currentValue);
				return 0;
			}
			else
			{	
				var topIndex = count - 1;
				var lastValue = ClearValues [topIndex];

				if (currentValue.Equals (lastValue))
				{
					return (byte)topIndex;
				}
				else
				{
					ClearValues.Add (currentValue);
					return (byte)count;
				}
			}	
		}

		public List<GLQueueRendererColorBlendState> ColorBlends {get; private set; }
		byte GenerateColorBlendEnums (IGLGraphicsPipeline pipeline)
		{
			if (pipeline == null)
			{
				throw new ArgumentNullException("pipeline");
			}

			var currentValue = pipeline.ColorBlendEnums;

			var count = ColorBlends.Count;
			if (count == 0)
			{
				// USE DEFAULT
				ColorBlends.Add (currentValue);
				return 0;
			}
			else
			{	
				var topIndex = count - 1;
				var lastValue = ColorBlends [topIndex];

				if (currentValue.Equals (lastValue))
				{
					return (byte)topIndex;
				}
				else
				{
					ColorBlends.Add (currentValue);
					return (byte)count;
				}
			}	
		}

		public List<GLCmdBufferPipelineItem> Pipelines {get; private set; }
		byte ExtractGraphicsPipeline (GLCmdRenderPassCommand pass, IGLGraphicsPipeline pipeline)
		{
			QueueDrawItemBitFlags flags = pipeline.Flags;

			var count = Pipelines.Count;
			var currentValue = new GLCmdBufferPipelineItem {
				//Pipeline = (byte) count,
				Flags = flags,
				DepthState = pipeline.DepthState,
				StencilState = pipeline.StencilState,
				ClearValues = GenerateClearValues (pass),
				ColorBlendEnums = GenerateColorBlendEnums (pipeline),
			};

			if (count == 0)
			{
				// USE DEFAULT
				Pipelines.Add (currentValue);
				return 0;
			}
			else
			{	
				var topIndex = count - 1;
				var lastValue = Pipelines [topIndex];

				if (currentValue.Equals (lastValue))
				{
					return (byte)topIndex;
				}
				else
				{
					Pipelines.Add (currentValue);
					return (byte)count;
				}
			}	
		}

		public bool InitialiseDrawItem(IGLCmdBufferRepository repo, GLCmdRenderPassCommand pass, GLCmdDrawCommand drawCommand)
		{
			if (drawCommand.Pipeline.HasValue)
			{
				var pipeline = repo.GraphicsPipelines.At (drawCommand.Pipeline.Value);

				var item = GenerateDrawItem (pass, pipeline, drawCommand);

				item.DepthBias = ExtractDepthBias (repo, pipeline, drawCommand);

				item.BlendConstants = BlendConstants.Extract (repo, pipeline, drawCommand);

				item.DepthBounds = DepthBounds.Extract (repo, pipeline, drawCommand);

				item.LineWidth = LineWidths.Extract (repo, pipeline, drawCommand);

				item.BackStencilCompareMask = BackCompareMasks.Extract (repo, pipeline, drawCommand);

				item.FrontStencilCompareMask = FrontCompareMasks.Extract (repo, pipeline, drawCommand);

				item.FrontStencilWriteMask = FrontWriteMasks.Extract (repo, pipeline, drawCommand);

				item.BackStencilWriteMask = BackWriteMasks.Extract (repo, pipeline, drawCommand);

				item.FrontStencilReference = FrontReferences.Extract (repo, pipeline, drawCommand);

				item.BackStencilReference = BackReferences.Extract (repo, pipeline, drawCommand);

				item.Viewport = Viewports.Extract (repo, pipeline, drawCommand);

				item.Scissor = Scissors.Extract (repo, pipeline, drawCommand);

				DrawItems.Add (item);
				return true;
			}
			else
			{
				return false;
			}
		}

	}

}

