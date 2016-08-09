using System;
using Magnesium;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLQueueRenderer : IGLQueueRenderer
	{
		private readonly IBlendCapabilities mBlend;
		private readonly IStencilCapabilities mStencil;
		private readonly IRasterizerCapabilities mRaster;
		private readonly IDepthCapabilities mDepth;
		private readonly IShaderProgramCache mCache;
		private readonly IScissorsCapabilities mScissor;

		ICmdDrawCapabilities mRender;

		public GLQueueRenderer (
			IBlendCapabilities blend,
			IStencilCapabilities stencil,
			IRasterizerCapabilities raster,
			IDepthCapabilities depth, 
			IShaderProgramCache cache,
			IScissorsCapabilities scissor,
			ICmdDrawCapabilities render
		)
		{
			mCache = cache;
			mBlend = blend;
			mStencil = stencil;
			mRaster = raster;
			mDepth =  depth;
			mScissor = scissor;
			mRender = render;
		}

		private void ApplyBlendChanges (GLQueueRendererColorBlendState previous, GLQueueRendererColorBlendState current)
		{
			if (previous.LogicOpEnable != current.LogicOpEnable || previous.LogicOp != current.LogicOp)
			{
				mBlend.EnableLogicOp (current.LogicOpEnable);
				mBlend.LogicOp (current.LogicOp);
			}


			uint leftSize = (uint)previous.Attachments.Length;
			uint rightSize = (uint)current.Attachments.Length;

			uint fullLoop = Math.Max (leftSize, rightSize);

			for (uint i = 0; i < fullLoop; ++i)
			{
				bool hasPastValue = (i < leftSize);
				bool hasNextValue = (i < rightSize);

				if (hasPastValue && hasNextValue)
				{
					var past = previous.Attachments [i];
					var next = current.Attachments [i];

//					bool blendEnabled = !(next.Value.SrcColorBlendFactor == MgBlendFactor.ONE &&
//					                    next.Value.DstColorBlendFactor == MgBlendFactor.ZERO &&
//					                    next.Value.SrcAlphaBlendFactor == MgBlendFactor.ONE &&
//					                    next.Value.DstAlphaBlendFactor == MgBlendFactor.ZERO);

					if (past.BlendEnable != next.BlendEnable)
					{
						mBlend.EnableBlending (i, next.BlendEnable);
					}

					if (next.SrcColorBlendFactor != past.SrcColorBlendFactor ||
					    next.DstColorBlendFactor != past.DstColorBlendFactor ||
					    next.SrcAlphaBlendFactor != past.SrcAlphaBlendFactor ||
					    next.DstAlphaBlendFactor != past.DstAlphaBlendFactor)
					{
						mBlend.ApplyBlendSeparateFunction (
							i,
							next.SrcColorBlendFactor,
							next.DstColorBlendFactor,
							next.SrcAlphaBlendFactor,
							next.DstAlphaBlendFactor);
					}

					if (past.ColorWriteMask != next.ColorWriteMask)
					{
						mBlend.SetColorMask (i, next.ColorWriteMask);
					}
				}
				else if (!hasPastValue && hasNextValue)
				{
					var next = current.Attachments [i];
//					bool blendEnabled = !(next.Value.SrcColorBlendFactor == MgBlendFactor.ONE &&
//						next.Value.DstColorBlendFactor == MgBlendFactor.ZERO &&
//						next.Value.SrcAlphaBlendFactor == MgBlendFactor.ONE &&
//						next.Value.DstAlphaBlendFactor == MgBlendFactor.ZERO);

					mBlend.EnableBlending (i, next.BlendEnable);

					mBlend.ApplyBlendSeparateFunction (
						i,
						next.SrcColorBlendFactor,
						next.DstColorBlendFactor,
						next.SrcAlphaBlendFactor,
						next.DstAlphaBlendFactor);

					mBlend.SetColorMask (i, next.ColorWriteMask);
				}
			}
		}

		private void ApplyStencilChanges (
			GLQueueRendererStencilState past,
			GLQueueRendererStencilState next)
		{
			var pastStencil = past.Enums;
			var nextStencil = next.Enums;

			if (past.Front.WriteMask != next.Front.WriteMask)
			{
				mStencil.SetStencilWriteMask (next.Front.WriteMask);
			}

			var newStencilEnabled = (next.Flags & QueueDrawItemBitFlags.StencilEnabled);
			if (mStencil.IsStencilBufferEnabled != (newStencilEnabled != 0))
			{
				if (mStencil.IsStencilBufferEnabled)
				{
					mStencil.DisableStencilBuffer ();
				}
				else
				{
					mStencil.EnableStencilBuffer ();
				}
			}

			// TODO : Stencil operations
			// set function
			bool pastTwoSided = (past.Flags & QueueDrawItemBitFlags.TwoSidedStencilMode) > 0;
			bool nextTwoSided = (past.Flags & QueueDrawItemBitFlags.TwoSidedStencilMode) > 0;

			if (nextTwoSided)
			{
				if (nextTwoSided != pastTwoSided ||
					nextStencil.FrontStencilFunction != pastStencil.FrontStencilFunction ||
					past.Front.Reference != next.Front.Reference ||
					past.Front.CompareMask != next.Front.CompareMask)
				{
					mStencil.SetFrontFaceCullStencilFunction (
						nextStencil.FrontStencilFunction,
						next.Front.Reference,
						next.Front.CompareMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.BackStencilFunction != pastStencil.BackStencilFunction ||
					next.Back.Reference != past.Back.Reference ||
					next.Back.CompareMask != past.Back.CompareMask)
				{
					mStencil.SetBackFaceCullStencilFunction (
						nextStencil.BackStencilFunction,
						next.Back.Reference,
						next.Back.CompareMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.FrontStencilFail != pastStencil.FrontStencilFail ||
					nextStencil.FrontDepthBufferFail != pastStencil.FrontDepthBufferFail ||
					nextStencil.FrontStencilPass != pastStencil.FrontStencilPass)
				{
					mStencil.SetFrontFaceStencilOperation(			
						nextStencil.FrontStencilFail,
						nextStencil.FrontDepthBufferFail,
						nextStencil.FrontStencilPass);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.BackStencilFail != pastStencil.BackStencilFail ||
					nextStencil.BackDepthBufferFail != pastStencil.BackDepthBufferFail ||
					nextStencil.BackStencilPass != pastStencil.BackStencilPass)
				{
					mStencil.SetBackFaceStencilOperation(			
						nextStencil.BackStencilFail,
						nextStencil.BackDepthBufferFail,
						nextStencil.BackStencilPass);
				}
			}
			else
			{
				if (nextTwoSided != pastTwoSided ||
					nextStencil.FrontStencilFunction != pastStencil.FrontStencilFunction ||
					next.Front.Reference != past.Front.Reference ||
					next.Front.CompareMask != past.Front.CompareMask)
				{
					mStencil.SetStencilFunction (
						nextStencil.FrontStencilFunction,
						next.Front.Reference,
						next.Front.CompareMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.FrontStencilFail != pastStencil.FrontStencilFail ||
					nextStencil.FrontDepthBufferFail != pastStencil.FrontDepthBufferFail ||
					nextStencil.FrontStencilPass != pastStencil.FrontStencilPass)
				{
					mStencil.SetStencilOperation (
						nextStencil.FrontStencilFail,
						nextStencil.FrontDepthBufferFail,
						nextStencil.FrontStencilPass);
				}
			}
		}

		private static bool ChangesFoundInDepth(GLCmdBufferPipelineItem previous, GLCmdBufferPipelineItem next)
		{
			var mask = QueueDrawItemBitFlags.DepthBufferEnabled
			           | QueueDrawItemBitFlags.DepthBufferWriteEnabled;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags) || (!previous.DepthState.Equals (next.DepthState));
		}

		private static bool ChangesFoundInStencil (
			GLQueueRendererStencilState previous,
			GLQueueRendererStencilState current
		)
		{
			var mask = QueueDrawItemBitFlags.StencilEnabled
			           | QueueDrawItemBitFlags.TwoSidedStencilMode;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & current.Flags;

			return (pastFlags != nextFlags)
				|| (!previous.Enums.Equals(current.Enums))
				|| (!previous.Front.Equals(current.Front))
				|| (!previous.Back.Equals(current.Back));
		}

		private static bool ChangesFoundInBlend(GLQueueRendererColorBlendState previous, GLQueueRendererColorBlendState next)
		{
			if (previous == null && next != null)
			{
				return true;
			}

			if (previous != null && next == null)
			{
				return false;
			}

			if (previous.Attachments.Length != next.Attachments.Length)
				return true;

			if (previous.LogicOpEnable != next.LogicOpEnable)
				return true;

			if (previous.LogicOp != next.LogicOp)
				return true;

			for (uint i = 0; i < next.Attachments.Length; ++i)
			{
				if (!previous.Attachments [i].Equals (next.Attachments [i]))
				{
					return true;
				}
			}

			return false;
		}

		public void SetDefault()
		{			
			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SetDefault (BEFORE) : " + error);
			}

			const int NO_OF_COLOR_ATTACHMENTS = 4;
			PastColorBlend = mBlend.Initialize (NO_OF_COLOR_ATTACHMENTS);

			var initialStencilValue = mStencil.Initialize ();
			PastStencil = initialStencilValue;

			var initialDepthValue = mDepth.Initialize ();
			PreviousPipeline = new GLCmdBufferPipelineItem {
				DepthState = initialDepthValue,
				StencilState = initialStencilValue.Enums,
			};

			PastRasterization = mRaster.Initialize ();

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SetDefault (AFTER) : " + error);
			}
		}

		private static bool ChangesFoundInRasterization(
			GLQueueRendererRasterizerState previous,
			GLQueueRendererRasterizerState next)
		{
			var mask = QueueDrawItemBitFlags.CullBackFaces
				| QueueDrawItemBitFlags.CullFrontFaces
				| QueueDrawItemBitFlags.CullingEnabled
				| QueueDrawItemBitFlags.ScissorTestEnabled
				| QueueDrawItemBitFlags.UseCounterClockwiseWindings;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags) || !(previous.Equals (next));
		}

		private void ApplyRasterizationChanges(GLQueueRendererRasterizerState previous, GLQueueRendererRasterizerState next)
		{
			var mask = QueueDrawItemBitFlags.CullingEnabled;
			if ((previous.Flags & mask) != (next.Flags & mask))
			{
				if (mRaster.CullingEnabled)
				{
					mRaster.DisableCulling ();
				}
				else
				{
					mRaster.EnableCulling ();
				}
			}

			// culling facing face
			mask = QueueDrawItemBitFlags.CullFrontFaces | QueueDrawItemBitFlags.CullBackFaces;
			if ((previous.Flags & mask) != (next.Flags & mask))
			{
				mRaster.SetCullingMode (
					(next.Flags & QueueDrawItemBitFlags.CullFrontFaces) > 0 
					, (next.Flags & QueueDrawItemBitFlags.CullBackFaces) > 0 );
			}

			mask = QueueDrawItemBitFlags.ScissorTestEnabled;
			if ((previous.Flags & mask) != (next.Flags & mask))
			{
				if (mRaster.ScissorTestEnabled)
				{
					mRaster.DisableScissorTest ();
				}
				else
				{
					mRaster.EnableScissorTest ();
				}
			}

			mask = QueueDrawItemBitFlags.UseCounterClockwiseWindings;
			var nextMaskValue = (next.Flags & mask);
			if ((previous.Flags & mask) != nextMaskValue)
			{
				mRaster.SetUsingCounterClockwiseWindings(nextMaskValue > 0);
			}

			var prevState = previous.DepthBias;
			var nextState = next.DepthBias;
			if (Math.Abs (prevState.DepthBiasConstantFactor - nextState.DepthBiasConstantFactor) >= float.Epsilon
				|| Math.Abs (prevState.DepthBiasSlopeFactor - nextState.DepthBiasSlopeFactor) >= float.Epsilon)
			{
				if (	nextState.DepthBiasConstantFactor > 0.0f
					|| 	nextState.DepthBiasConstantFactor < 0.0f
					|| nextState.DepthBiasSlopeFactor < 0.0f
					|| nextState.DepthBiasSlopeFactor > 0.0f
					)
				{   
					mRaster.EnablePolygonOffset (nextState.DepthBiasSlopeFactor, nextState.DepthBiasConstantFactor);
				} else
				{
					mRaster.DisablePolygonOffset ();		
				}
			}
		}

		private void ApplyDepthChanges (GLCmdBufferPipelineItem previous, GLCmdBufferPipelineItem next)
		{
			var enabled = (next.Flags & QueueDrawItemBitFlags.DepthBufferEnabled) != 0;

			if (mDepth.IsDepthBufferEnabled != enabled)
			{
				if (mDepth.IsDepthBufferEnabled)
				{
					mDepth.DisableDepthBuffer ();
				}
				else
				{
					mDepth.EnableDepthBuffer ();
				}
			}

			var oldDepthWrite = (previous.Flags & QueueDrawItemBitFlags.DepthBufferWriteEnabled);
			var newDepthWrite = (next.Flags & QueueDrawItemBitFlags.DepthBufferWriteEnabled);

			var pastDepth = previous.DepthState;
			var nextDepth = next.DepthState;

			if ((oldDepthWrite & newDepthWrite) != oldDepthWrite)
			{
				mDepth.SetDepthMask (newDepthWrite != 0);
			}

			if (pastDepth.DepthBufferFunction != nextDepth.DepthBufferFunction)
			{
				mDepth.SetDepthBufferFunc (nextDepth.DepthBufferFunction);
			}
		}

		public GLCmdBufferPipelineItem PreviousPipeline { get ; private set; }
		//public GLCmdBufferDrawItem mPreviousItem;
		public GLQueueRendererRasterizerState PastRasterization { get ; private set; }
		public GLQueueRendererStencilState PastStencil { get; private set; }

		public GLQueueRendererColorBlendState PastColorBlend {
			get;
			private set;
		}

		static GLQueueRendererStencilState ExtractStencilValues (CmdBufferInstructionSet instructionSet, GLCmdBufferDrawItem drawItem, GLCmdBufferPipelineItem currentPipeline)
		{
			var currentStencil = new GLQueueRendererStencilState ();
			currentStencil.Flags = currentPipeline.Flags;
			currentStencil.Front = new GLQueueStencilMasks {
				CompareMask = instructionSet.FrontCompareMasks [drawItem.FrontStencilCompareMask],
				Reference = instructionSet.FrontReferences [drawItem.FrontStencilReference],
				WriteMask = instructionSet.FrontWriteMasks [drawItem.FrontStencilWriteMask],
			};
			currentStencil.Back = new GLQueueStencilMasks {
				CompareMask = instructionSet.BackCompareMasks [drawItem.BackStencilCompareMask],
				Reference = instructionSet.BackReferences [drawItem.BackStencilReference],
				WriteMask = instructionSet.BackWriteMasks [drawItem.BackStencilWriteMask],
			};
			return currentStencil;
		}

		static GLQueueRendererRasterizerState ExtractRasterizationValues (CmdBufferInstructionSet instructionSet, GLCmdBufferDrawItem drawItem, GLCmdBufferPipelineItem currentPipeline)
		{
			return new GLQueueRendererRasterizerState {
				Flags = currentPipeline.Flags,
				DepthBias = instructionSet.DepthBias [drawItem.DepthBias],
				LineWidth = instructionSet.LineWidths [drawItem.LineWidth],
			};
		}

		static bool ChangesFoundInScissors (GLCmdScissorParameter pastScissors, GLCmdScissorParameter currentScissors)
		{
			if (pastScissors == null && currentScissors != null)
				return true;

			if (pastScissors != null && currentScissors == null)
				return false;

			return !pastScissors.Equals (currentScissors);
		}

		public GLCmdScissorParameter PastScissors {
			get;
			private set;
		}

		public GLCmdViewportParameter PastViewport {
			get;
			private set;
		}

		bool ChangesFoundInViewports (GLCmdViewportParameter pastViewport, GLCmdViewportParameter currentViewport)
		{
			if (pastViewport == null && currentViewport != null)
				return true;

			if (pastViewport != null && currentViewport == null)
				return false;

			return !pastViewport.Equals (currentViewport);
		}

		static void ApplyClearBuffers (GLCmdClearValuesParameter clearState, ClearBufferMask combinedMask)
		{
			if (clearState.Attachments.Length > 0)
			{
				GL.Clear (combinedMask);
			}
		}

		static ClearBufferMask InitialiseClearBuffers (GLCmdClearValuesParameter clearState)
		{
			ClearBufferMask combinedMask = 0;

			foreach (var state in clearState.Attachments)
			{
				if (state.Attachment.LoadOp == MgAttachmentLoadOp.CLEAR)
				{
					if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_INT)
					{
						var initialValue = state.Value.Color.Int32;

						var clearValue = new MgVec4f {
							X = Math.Max (Math.Min ((float)initialValue.X, state.Attachment.Divisor), -state.Attachment.Divisor) / state.Attachment.Divisor,
							Y = Math.Max (Math.Min ((float)initialValue.Y, state.Attachment.Divisor), -state.Attachment.Divisor) / state.Attachment.Divisor,
							Z = Math.Max (Math.Min ((float)initialValue.Z, state.Attachment.Divisor), -state.Attachment.Divisor) / state.Attachment.Divisor,
							W = Math.Max (Math.Min ((float)initialValue.W, state.Attachment.Divisor), -state.Attachment.Divisor) / state.Attachment.Divisor,
						};

						GL.ClearColor (clearValue.X, clearValue.Y, clearValue.Z, clearValue.W);
						combinedMask |= ClearBufferMask.ColorBufferBit;
					} else if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_FLOAT)
					{
						var clearValue = state.Value.Color.Float32;
						GL.ClearColor (clearValue.X, clearValue.Y, clearValue.Z, clearValue.W);
						combinedMask |= ClearBufferMask.ColorBufferBit;
					} else if (state.Attachment.AttachmentType == GLClearAttachmentType.COLOR_UINT)
					{
						var initialValue = state.Value.Color.Uint32;

						var clearValue = new MgVec4f {
							X = Math.Min ((float)initialValue.X, state.Attachment.Divisor) / state.Attachment.Divisor,
							Y = Math.Min ((float)initialValue.Y, state.Attachment.Divisor) / state.Attachment.Divisor,
							Z = Math.Min ((float)initialValue.Z, state.Attachment.Divisor) / state.Attachment.Divisor,
							W = Math.Min ((float)initialValue.W, state.Attachment.Divisor) / state.Attachment.Divisor,
						};

						GL.ClearColor (clearValue.X, clearValue.Y, clearValue.Z, clearValue.W);
						combinedMask |= ClearBufferMask.ColorBufferBit;
					} 
					else if (state.Attachment.AttachmentType == GLClearAttachmentType.DEPTH_STENCIL)
					{
						var clearValue = state.Value.DepthStencil;
						GL.ClearDepth (clearValue.Depth);
						combinedMask |= ClearBufferMask.DepthBufferBit;
					}

				}

				if (state.Attachment.StencilLoadOp == MgAttachmentLoadOp.CLEAR)
				{
					if (state.Attachment.AttachmentType == GLClearAttachmentType.DEPTH_STENCIL)
					{
						var clearValue = state.Value.DepthStencil;
						GL.ClearDepth (clearValue.Depth);

						// TODO : CHECK IF THIS RIGHT
						GL.ClearStencil ((int)clearValue.Stencil);
						combinedMask |= ClearBufferMask.StencilBufferBit;
					}
				}
			}

			return combinedMask;
		}

		byte ApplyRenderpass (CmdBufferInstructionSet instructionSet, GLCmdBufferPipelineItem currentPipeline, GLCmdBufferDrawItem drawItem)
		{
			var clearValues = currentPipeline.ClearValues;
			var clearState = instructionSet.ClearValues [clearValues];
			var bitmask = InitialiseClearBuffers (clearState);

			// scissor 
			var currentScissors = instructionSet.Scissors [drawItem.Scissor];
			if (ChangesFoundInScissors (PastScissors, currentScissors))
			{
				mScissor.ApplyScissors (currentScissors);
			}
			PastScissors = currentScissors;

			// viewport
			var currentViewport = instructionSet.Viewports [drawItem.Viewport];
			if (ChangesFoundInViewports (PastViewport, currentViewport))
			{
				mScissor.ApplyViewports (currentViewport);
			}
			PastViewport = currentViewport;

			ApplyClearBuffers (clearState, bitmask);
			return clearValues;
		}

		public void Render(CmdBufferInstructionSet[] items)
		{
			var pastPipeline = PreviousPipeline;
			var pastStencil = PastStencil;
			var clearValues = 0;

			var isFirst = true;

			foreach (var instructionSet in items)
			{
				foreach (var drawItem in instructionSet.DrawItems)
				{
					// TODO : bind render target
					var currentPipeline = instructionSet.Pipelines[drawItem.Pipeline];

					// clear values
					if (isFirst)
					{
						clearValues = ApplyRenderpass (instructionSet, currentPipeline, drawItem);

						isFirst = false;
					}
					else
					{
						if (clearValues != currentPipeline.ClearValues)
						{
							clearValues = ApplyRenderpass (instructionSet, currentPipeline, drawItem);
						}
					}

					CheckProgram (instructionSet, drawItem);

					var currentBlendState = instructionSet.ColorBlends[currentPipeline.ColorBlendEnums];
					if (ChangesFoundInBlend (PastColorBlend, currentBlendState))
					{
						ApplyBlendChanges (PastColorBlend, currentBlendState);
					}
					PastColorBlend = currentBlendState;

					if (ChangesFoundInDepth (PreviousPipeline, currentPipeline))
					{
						ApplyDepthChanges (PreviousPipeline, currentPipeline);
					}

					var currentStencil = ExtractStencilValues (instructionSet, drawItem, currentPipeline);
					if (ChangesFoundInStencil (pastStencil, currentStencil))
					{
						ApplyStencilChanges (pastStencil, currentStencil);
					}
					PastStencil = currentStencil;

					var currentRasterization = ExtractRasterizationValues (instructionSet, drawItem, currentPipeline);
					if (ChangesFoundInRasterization (PastRasterization, currentRasterization))
					{
						ApplyRasterizationChanges (PastRasterization, currentRasterization);
					}
					PastRasterization = currentRasterization;

					// Draw here 
					if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndexedIndirect) == GLCommandBufferFlagBits.CmdDrawIndexedIndirect)
					{
						var indexType = (drawItem.Command & GLCommandBufferFlagBits.Index16BitMode) == GLCommandBufferFlagBits.Index16BitMode
							? MgIndexType.UINT16 : MgIndexType.UINT32;

						if (drawItem.Offset >= (ulong) int.MaxValue)
						{
							throw new InvalidOperationException ();
						}

						var indirect = IntPtr.Add (drawItem.Buffer, (int) drawItem.Offset);

						mRender.DrawIndexedIndirect (drawItem.Topology, indexType, indirect, drawItem.Count, drawItem.Stride);
					}
					else if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndexed) == GLCommandBufferFlagBits.CmdDrawIndexed)
					{
						var indexType = (drawItem.Command & GLCommandBufferFlagBits.Index16BitMode) == GLCommandBufferFlagBits.Index16BitMode
							? MgIndexType.UINT16 : MgIndexType.UINT32;

						mRender.DrawIndexed (drawItem.Topology, indexType, drawItem.First, drawItem.Count, drawItem.InstanceCount, drawItem.VertexOffset);
					}
					else if ((drawItem.Command & GLCommandBufferFlagBits.CmdDrawIndirect) == GLCommandBufferFlagBits.CmdDrawIndirect)
					{
						if (drawItem.Offset >= (ulong) int.MaxValue)
						{
							throw new InvalidOperationException ();
						}

						var indirect = IntPtr.Add (drawItem.Buffer, (int) drawItem.Offset);

						mRender.DrawArraysIndirect (drawItem.Topology, indirect, drawItem.Count, drawItem.Stride);
					}
					else
					{
						mRender.DrawArrays(drawItem.Topology, drawItem.First, drawItem.Count, drawItem.InstanceCount, drawItem.FirstInstance);
					}

//					pastState = instructionSet;
					pastPipeline = currentPipeline;
				}
			}
			PreviousPipeline = pastPipeline;
		}

		public void CheckProgram(CmdBufferInstructionSet instructionSet, GLCmdBufferDrawItem drawItem)
		{
			// bind program
			if (mCache.ProgramID != drawItem.ProgramID)
			{
				mCache.ProgramID = drawItem.ProgramID;
				mCache.VBO = drawItem.VBO;
				mCache.DescriptorSet = instructionSet.DescriptorSets [drawItem.DescriptorSet];
				mCache.BindDescriptorSet ();
			}
			else
			{
				mCache.VBO = drawItem.VBO;

				if (mCache.DescriptorSetIndex != drawItem.DescriptorSet)
				{
					// TODO : FIX ME
					mCache.DescriptorSet = instructionSet.DescriptorSets [drawItem.DescriptorSet];

					if (mCache.DescriptorSet != null)
					{
						mCache.BindDescriptorSet ();
					}
				}
			}

			// bind constant buffers
//			if (currentProgram.GetBufferMask () != nextState.BufferMask)
//			{
//				currentProgram.BindMask (mBuffers);
//			}
			// bind uniforms



		}
	}
}

