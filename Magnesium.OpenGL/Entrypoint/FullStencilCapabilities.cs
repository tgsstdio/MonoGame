using OpenTK.Graphics.OpenGL;
using Magnesium;

namespace Magnesium.OpenGL
{
	using GLStencilFunction = OpenTK.Graphics.OpenGL.StencilFunction;

	public class FullStencilCapabilities : IStencilCapabilities
	{
		#region IDepthStencilCapabilities implementation

		public void Initialize ()
		{
			DisableStencilBuffer ();
			SetStencilWriteMask (~0);
			SetStencilFunction (MgCompareOp.ALWAYS, ~0, int.MaxValue);
			SetStencilOperation (MgStencilOp.KEEP, MgStencilOp.KEEP, MgStencilOp.KEEP);
		}

		public void EnableStencilBuffer()
		{
			GL.Disable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = true;
		}

		public void DisableStencilBuffer()
		{
			GL.Enable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = false;
		}

		private bool mIsStencilBufferEnabled;
		public bool IsStencilBufferEnabled {
			get {
				return mIsStencilBufferEnabled;
			}
		}

		public void SetStencilWriteMask(int mask)
		{
			GL.StencilMask(mask);
		}

		public void SetFrontFaceCullStencilFunction (MgCompareOp func, int referenceStencil, int stencilMask)
		{
			var cullFaceModeFront = StencilFace.Front;
			GL.StencilFuncSeparate (
				cullFaceModeFront,
				GetStencilFunc (func),
				referenceStencil,
				stencilMask);
		}

		public void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, int stencilMask)
		{
			var cullFaceModeBack = StencilFace.Back;					
			GL.StencilFuncSeparate (
				cullFaceModeBack,
				GetStencilFunc (func),
				referenceStencil,
				stencilMask);
		}

		private static GLStencilFunction GetStencilFunc(MgCompareOp function)
		{
			switch (function)
			{
			case MgCompareOp.ALWAYS: 
				return GLStencilFunction.Always;
			case MgCompareOp.EQUAL:
				return GLStencilFunction.Equal;
			case MgCompareOp.GREATER:
				return GLStencilFunction.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return GLStencilFunction.Gequal;
			case MgCompareOp.LESS:
				return GLStencilFunction.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return GLStencilFunction.Lequal;
			case MgCompareOp.NEVER:
				return GLStencilFunction.Never;
			case MgCompareOp.NOT_EQUAL:
				return GLStencilFunction.Notequal;
			default:
				return GLStencilFunction.Always;
			}
		}

		public void SetFrontFaceStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass)
		{
			var stencilFaceFront = StencilFace.Front;					
			GL.StencilOpSeparate(stencilFaceFront, GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));
		}

		public void SetBackFaceStencilOperation(
			MgStencilOp counterClockwiseStencilFail,
			MgStencilOp counterClockwiseStencilDepthBufferFail,
			MgStencilOp counterClockwiseStencilPass)
		{
			var stencilFaceBack = StencilFace.Back;					
			GL.StencilOpSeparate(stencilFaceBack, GetStencilOp(counterClockwiseStencilFail),
				GetStencilOp(counterClockwiseStencilDepthBufferFail),
				GetStencilOp(counterClockwiseStencilPass));			
		}

		public void SetStencilFunction(
			MgCompareOp stencilFunction,
			int referenceStencil,
			int stencilMask)
		{
			GL.StencilFunc(
				GetStencilFunc (stencilFunction),
				referenceStencil,
				stencilMask);
		}

		public void SetStencilOperation(
			MgStencilOp stencilFail,
			MgStencilOp stencilDepthBufferFail,
			MgStencilOp stencilPass)
		{
			GL.StencilOp (GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));
		}

		private static StencilOp GetStencilOp(MgStencilOp operation)
		{
			switch (operation)
			{
			case MgStencilOp.KEEP:
				return StencilOp.Keep;
			case MgStencilOp.DECREMENT_AND_WRAP:
				return StencilOp.DecrWrap;
			case MgStencilOp.DECREMENT_AND_CLAMP:
				return StencilOp.Decr;
			case MgStencilOp.INCREMENT_AND_CLAMP:
				return StencilOp.Incr;
			case MgStencilOp.INCREMENT_AND_WRAP:
				return StencilOp.IncrWrap;
			case MgStencilOp.INVERT:
				return StencilOp.Invert;
			case MgStencilOp.REPLACE:
				return StencilOp.Replace;
			case MgStencilOp.ZERO:
				return StencilOp.Zero;
			default:
				return StencilOp.Keep;
			}
		}

		#endregion

	}
}

