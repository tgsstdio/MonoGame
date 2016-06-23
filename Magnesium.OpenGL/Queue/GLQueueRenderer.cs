using System;
using Magnesium;

namespace Magnesium.OpenGL
{
	public class GLQueueRenderer : IGLQueueRenderer
	{
		private readonly IBlendCapabilities mBlend;
		private readonly IStencilCapabilities mStencil;
		private readonly IRasterizerCapabilities mRaster;
		private readonly IDepthCapabilities mDepth;
		private readonly IShaderProgramCache mCache;

		public GLQueueRenderer (IBlendCapabilities blend, IStencilCapabilities stencil, IRasterizerCapabilities raster, IDepthCapabilities depth, IShaderProgramCache cache)
		{
			mCache = cache;
			mBlend = blend;
			mStencil = stencil;
			mRaster = raster;
			mDepth =  depth;
		}

		public GLQueueDrawItem mPreviousItem;

		private void ApplyBlendChanges (GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var pastBlend = previous.BlendValues;
			var nextBlend = next.BlendValues;

			bool blendEnabled = !(nextBlend.ColorSourceBlend == MgBlendFactor.ONE && 
				nextBlend.ColorDestinationBlend == MgBlendFactor.ZERO &&
				nextBlend.AlphaSourceBlend == MgBlendFactor.ONE &&
				nextBlend.AlphaDestinationBlend == MgBlendFactor.ZERO);

			if (blendEnabled != mBlend.IsEnabled)
			{
				mBlend.EnableBlending (blendEnabled);
			}

			if (nextBlend.ColorSourceBlend != pastBlend.ColorSourceBlend ||
				nextBlend.ColorDestinationBlend != pastBlend.ColorDestinationBlend ||
				nextBlend.AlphaSourceBlend != pastBlend.AlphaSourceBlend ||
				nextBlend.AlphaDestinationBlend != pastBlend.AlphaDestinationBlend)
			{
				mBlend.ApplyBlendSeparateFunction (
					nextBlend.ColorSourceBlend,
					nextBlend.ColorDestinationBlend,
					nextBlend.AlphaSourceBlend,
					nextBlend.AlphaDestinationBlend);
			}

			var writeMask = QueueDrawItemBitFlags.RedColorWriteChannel
				| QueueDrawItemBitFlags.GreenColorWriteChannel
				| QueueDrawItemBitFlags.BlueColorWriteChannel
				| QueueDrawItemBitFlags.AlphaColorWriteChannel; 

			var pastColourMask = writeMask & previous.Flags;
			var nextColourMask = writeMask & next.Flags;

			if (pastColourMask != nextColourMask)
			{
				mBlend.SetColorMask (nextColourMask);
			}
		}

		private void ApplyStencilChanges (
			GLQueueDrawItem previous,
			GLQueueStencilMasks pastFrontMasks,
			GLQueueStencilMasks pastBackMasks,
			GLQueueDrawItem next,
			GLQueueStencilMasks nextFrontMasks,
			GLQueueStencilMasks nextBackMasks)
		{
			var pastStencil = previous.StencilValues;
			var nextStencil = next.StencilValues;

			if (pastFrontMasks.WriteMask != nextFrontMasks.WriteMask)
			{
				mStencil.SetStencilWriteMask (nextFrontMasks.WriteMask);
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
			bool pastTwoSided = (previous.Flags & QueueDrawItemBitFlags.TwoSidedStencilMode) > 0;
			bool nextTwoSided = (previous.Flags & QueueDrawItemBitFlags.TwoSidedStencilMode) > 0;

			if (nextTwoSided)
			{
				if (nextTwoSided != pastTwoSided ||
					nextStencil.FrontStencilFunction != pastStencil.FrontStencilFunction ||
					pastFrontMasks.Reference != nextFrontMasks.Reference ||
					pastFrontMasks.CompareMask != nextFrontMasks.CompareMask)
				{
					mStencil.SetFrontFaceCullStencilFunction (
						nextStencil.FrontStencilFunction,
						nextFrontMasks.Reference,
						nextFrontMasks.CompareMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.BackStencilFunction != pastStencil.BackStencilFunction ||
					nextBackMasks.Reference != pastBackMasks.Reference ||
					nextBackMasks.CompareMask != pastBackMasks.CompareMask)
				{
					mStencil.SetBackFaceCullStencilFunction (
						nextStencil.BackStencilFunction,
						nextBackMasks.Reference,
						nextBackMasks.CompareMask);
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
					nextFrontMasks.Reference != pastFrontMasks.Reference ||
					nextFrontMasks.CompareMask != pastFrontMasks.CompareMask)
				{
					mStencil.SetStencilFunction (
						nextStencil.FrontStencilFunction,
						nextFrontMasks.Reference,
						nextFrontMasks.CompareMask);
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

		private static bool ChangesFoundInDepth(GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var mask = QueueDrawItemBitFlags.DepthBufferEnabled
			           | QueueDrawItemBitFlags.DepthBufferWriteEnabled;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags);
		}

		private static bool ChangesFoundInStencil (
			GLQueueDrawItem previous,
			GLQueueStencilMasks previousFront,
			GLQueueStencilMasks previousBack,
			GLQueueDrawItem next,
			GLQueueStencilMasks nextFront,
			GLQueueStencilMasks nextBack		
		)
		{
			var mask = QueueDrawItemBitFlags.StencilEnabled
			           | QueueDrawItemBitFlags.TwoSidedStencilMode;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags)
				|| (!previous.StencilValues.Equals(next.StencilValues))
					|| (!previousFront.Equals(nextFront))
				|| (!previousBack.Equals(nextBack));
		}

		private static bool ChangesFoundInBlend(GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var mask = QueueDrawItemBitFlags.RedColorWriteChannel
				| QueueDrawItemBitFlags.GreenColorWriteChannel
				| QueueDrawItemBitFlags.BlueColorWriteChannel
				| QueueDrawItemBitFlags.AlphaColorWriteChannel;

					var pastFlags = mask & previous.Flags;
					var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags) || (!(previous.BlendValues.Equals (next.BlendValues)));
		}

		public void SetDefault()
		{			
			mBlend.Initialize ();
			mStencil.Initialize ();
			mRaster.Initialize ();
			mDepth.Initialize ();
		}

		private static bool ChangesFoundInRasterization(GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var mask = QueueDrawItemBitFlags.CullBackFaces
				| QueueDrawItemBitFlags.CullFrontFaces
				| QueueDrawItemBitFlags.CullingEnabled
				| QueueDrawItemBitFlags.ScissorTestEnabled
				| QueueDrawItemBitFlags.UseCounterClockwiseWindings;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags) || !(previous.RasterizerValues.Equals (next.RasterizerValues));
		}

		private void ApplyRasterizationChanges(GLQueueDrawItem previous, GLQueueDrawItem next)
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


			var nextState = next.RasterizerValues;
			if (Math.Abs (previous.RasterizerValues.DepthBiasConstantFactor - nextState.DepthBiasConstantFactor) >= float.Epsilon
				|| Math.Abs (previous.RasterizerValues.DepthBiasSlopeFactor - nextState.DepthBiasSlopeFactor) >= float.Epsilon)
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

		private void ApplyDepthChanges (GLQueueDrawItem previous, GLQueueDrawItem next)
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

			var pastDepth = previous.DepthValues;
			var nextDepth = next.DepthValues;

			if ((oldDepthWrite & newDepthWrite) != oldDepthWrite)
			{
				mDepth.SetDepthMask (newDepthWrite != 0);
			}

			if (pastDepth.DepthBufferFunction != nextDepth.DepthBufferFunction)
			{
				mDepth.SetDepthBufferFunc (nextDepth.DepthBufferFunction);
			}
		}

		public void Render(GLQueueDrawItem[] items)
		{
			var pastState = mPreviousItem;
			foreach (var nextState in items)
			{
				// TODO : bind render target

				CheckProgram (nextState);

				if (ChangesFoundInBlend (pastState, nextState))
				{
					ApplyBlendChanges (pastState, nextState);
				}

				if (ChangesFoundInDepth (pastState, nextState))
				{
					ApplyDepthChanges(pastState, nextState);
				}

				//TODO : revise
				var dummyFrontMask = new GLQueueStencilMasks();
				var dummyBackMask = new GLQueueStencilMasks();

				if (ChangesFoundInStencil (pastState, dummyFrontMask, dummyBackMask, nextState, dummyFrontMask, dummyBackMask))
				{
					ApplyStencilChanges (pastState, dummyFrontMask, dummyBackMask, nextState, dummyFrontMask, dummyBackMask);
				}

				if (ChangesFoundInRasterization(pastState, nextState))
				{
					ApplyRasterizationChanges (pastState, nextState);
				}

				pastState = nextState;
			}
		}

		public void CheckProgram(GLQueueDrawItem nextState)
		{
			// bind program
			if (mCache.ProgramIndex != nextState.ProgramIndex)
			{
				mCache.SetProgram (nextState.ProgramIndex);
			}

			var currentProgram = mCache.GetActiveProgram ();
			// bind constant buffers
//			if (currentProgram.GetBufferMask () != nextState.BufferMask)
//			{
//				currentProgram.BindMask (mBuffers);
//			}
			// bind uniforms
			currentProgram.DescriptorSet = nextState.DescriptorSet;

			currentProgram.VBO = nextState.VBO;
		}
	}
}

