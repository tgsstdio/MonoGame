using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	using GLStencilFunction = OpenTK.Graphics.OpenGL.StencilFunction;

	public class FullStencilCapabilities : IStencilCapabilities
	{
		#region IDepthStencilCapabilities implementation

		public void Initialise ()
		{
			DisableStencilBuffer ();
			SetStencilWriteMask (~0);
			SetStencilFunction (CompareFunction.Always, ~0, int.MaxValue);
			SetStencilOperation (StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep);
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

		public void SetFrontFaceCullStencilFunction (CompareFunction func, int referenceStencil, int stencilMask)
		{
			var cullFaceModeFront = StencilFace.Front;
			GL.StencilFuncSeparate (
				cullFaceModeFront,
				GetStencilFunc (func),
				referenceStencil,
				stencilMask);
		}

		public void SetBackFaceCullStencilFunction(CompareFunction func, int referenceStencil, int stencilMask)
		{
			var cullFaceModeBack = StencilFace.Back;					
			GL.StencilFuncSeparate (
				cullFaceModeBack,
				GetStencilFunc (func),
				referenceStencil,
				stencilMask);
		}

		private static GLStencilFunction GetStencilFunc(CompareFunction function)
		{
			switch (function)
			{
			case CompareFunction.Always: 
				return GLStencilFunction.Always;
			case CompareFunction.Equal:
				return GLStencilFunction.Equal;
			case CompareFunction.Greater:
				return GLStencilFunction.Greater;
			case CompareFunction.GreaterEqual:
				return GLStencilFunction.Gequal;
			case CompareFunction.Less:
				return GLStencilFunction.Less;
			case CompareFunction.LessEqual:
				return GLStencilFunction.Lequal;
			case CompareFunction.Never:
				return GLStencilFunction.Never;
			case CompareFunction.NotEqual:
				return GLStencilFunction.Notequal;
			default:
				return GLStencilFunction.Always;
			}
		}

		public void SetFrontFaceStencilOperation(
			StencilOperation stencilFail,
			StencilOperation stencilDepthBufferFail,
			StencilOperation stencilPass)
		{
			var stencilFaceFront = StencilFace.Front;					
			GL.StencilOpSeparate(stencilFaceFront, GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));
		}

		public void SetBackFaceStencilOperation(
			StencilOperation counterClockwiseStencilFail,
			StencilOperation counterClockwiseStencilDepthBufferFail,
			StencilOperation counterClockwiseStencilPass)
		{
			var stencilFaceBack = StencilFace.Back;					
			GL.StencilOpSeparate(stencilFaceBack, GetStencilOp(counterClockwiseStencilFail),
				GetStencilOp(counterClockwiseStencilDepthBufferFail),
				GetStencilOp(counterClockwiseStencilPass));			
		}

		public void SetStencilFunction(
			CompareFunction stencilFunction,
			int referenceStencil,
			int stencilMask)
		{
			GL.StencilFunc(
				GetStencilFunc (stencilFunction),
				referenceStencil,
				stencilMask);
		}

		public void SetStencilOperation(
			StencilOperation stencilFail,
			StencilOperation stencilDepthBufferFail,
			StencilOperation stencilPass)
		{
			GL.StencilOp (GetStencilOp(stencilFail),
				GetStencilOp(stencilDepthBufferFail),
				GetStencilOp(stencilPass));
		}

		private static StencilOp GetStencilOp(StencilOperation operation)
		{
			switch (operation)
			{
			case StencilOperation.Keep:
				return StencilOp.Keep;
			case StencilOperation.Decrement:
				return StencilOp.DecrWrap;
			case StencilOperation.DecrementSaturation:
				return StencilOp.Decr;
			case StencilOperation.IncrementSaturation:
				return StencilOp.Incr;
			case StencilOperation.Increment:
				return StencilOp.IncrWrap;
			case StencilOperation.Invert:
				return StencilOp.Invert;
			case StencilOperation.Replace:
				return StencilOp.Replace;
			case StencilOperation.Zero:
				return StencilOp.Zero;
			default:
				return StencilOp.Keep;
			}
		}

		#endregion

	}
}

