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

		public IConstantBufferCollection mBuffers;

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

		private void ApplyStencilChanges (GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var pastStencil = previous.StencilValues;
			var nextStencil = next.StencilValues;

			if (pastStencil.StencilWriteMask != nextStencil.StencilWriteMask)
			{
				mStencil.SetStencilWriteMask (nextStencil.StencilWriteMask);
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
					nextStencil.StencilFunction != pastStencil.StencilFunction ||
					nextStencil.ReferenceStencil != pastStencil.ReferenceStencil ||
					nextStencil.StencilMask != pastStencil.StencilMask)
				{
					mStencil.SetFrontFaceCullStencilFunction (
						nextStencil.StencilFunction,
						nextStencil.ReferenceStencil,
						nextStencil.StencilMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.CounterClockwiseStencilFunction != pastStencil.CounterClockwiseStencilFunction ||
					nextStencil.ReferenceStencil != pastStencil.ReferenceStencil ||
					nextStencil.StencilMask != pastStencil.StencilMask)
				{
					mStencil.SetBackFaceCullStencilFunction (
						nextStencil.CounterClockwiseStencilFunction,
						nextStencil.ReferenceStencil,
						nextStencil.StencilMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.StencilFail != pastStencil.StencilFail ||
					nextStencil.StencilDepthBufferFail != pastStencil.StencilDepthBufferFail ||
					nextStencil.StencilPass != pastStencil.StencilPass)
				{
					mStencil.SetFrontFaceStencilOperation(			
						nextStencil.StencilFail,
						nextStencil.StencilDepthBufferFail,
						nextStencil.StencilPass);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.CounterClockwiseStencilFail != pastStencil.CounterClockwiseStencilFail ||
					nextStencil.CounterClockwiseStencilDepthBufferFail != pastStencil.CounterClockwiseStencilDepthBufferFail ||
					nextStencil.CounterClockwiseStencilPass != pastStencil.CounterClockwiseStencilPass)
				{
					mStencil.SetBackFaceStencilOperation(			
						nextStencil.CounterClockwiseStencilFail,
						nextStencil.CounterClockwiseStencilDepthBufferFail,
						nextStencil.CounterClockwiseStencilPass);
				}
			}
			else
			{
				if (nextTwoSided != pastTwoSided ||
					nextStencil.StencilFunction != pastStencil.StencilFunction ||
					nextStencil.ReferenceStencil != pastStencil.ReferenceStencil ||
					nextStencil.StencilMask != pastStencil.StencilMask)
				{
					mStencil.SetStencilFunction (
						nextStencil.StencilFunction,
						nextStencil.ReferenceStencil,
						nextStencil.StencilMask);
				}

				if (nextTwoSided != pastTwoSided ||
					nextStencil.StencilFail != pastStencil.StencilFail ||
					nextStencil.StencilDepthBufferFail != pastStencil.StencilDepthBufferFail ||
					nextStencil.StencilPass != pastStencil.StencilPass)
				{
					mStencil.SetStencilOperation (
						nextStencil.StencilFail,
						nextStencil.StencilDepthBufferFail,
						nextStencil.StencilPass);
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

		private static bool ChangesFoundInStencil (GLQueueDrawItem previous, GLQueueDrawItem next)
		{
			var mask = QueueDrawItemBitFlags.StencilEnabled
			           | QueueDrawItemBitFlags.TwoSidedStencilMode;

			var pastFlags = mask & previous.Flags;
			var nextFlags = mask & next.Flags;

			return (pastFlags != nextFlags) || (!(previous.StencilValues.Equals(next.StencilValues)));
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
			mBlend.Initialise ();
			mStencil.Initialise ();
			mRaster.Initialise ();
			mDepth.Initialise ();
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
			if (Math.Abs (previous.RasterizerValues.DepthBias - nextState.DepthBias) >= float.Epsilon
				|| Math.Abs (previous.RasterizerValues.SlopeScaleDepthBias - nextState.SlopeScaleDepthBias) >= float.Epsilon)
			{
				if (	nextState.DepthBias > 0.0f
					|| 	nextState.DepthBias < 0.0f
					|| nextState.SlopeScaleDepthBias < 0.0f
					|| nextState.SlopeScaleDepthBias > 0.0f
					)
				{   
					mRaster.EnablePolygonOffset (nextState.SlopeScaleDepthBias, nextState.DepthBias);
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

				if (ChangesFoundInStencil (pastState, nextState))
				{
					ApplyStencilChanges (pastState, nextState);
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
			if (currentProgram.GetBufferMask () != nextState.BufferMask)
			{
				currentProgram.BindMask (mBuffers);
			}
			// bind uniforms
			if (currentProgram.GetUniformIndex () != nextState.UniformsIndex)
			{
				currentProgram.SetUniformIndex (nextState.UniformsIndex);
			}

			if (currentProgram.GetBindingSet () != nextState.BindingSet)
			{
				currentProgram.BindSet (nextState.BindingSet);
			}
		}
	}
}

