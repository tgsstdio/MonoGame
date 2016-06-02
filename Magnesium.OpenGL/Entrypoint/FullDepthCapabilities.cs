using System;
using OpenTK.Graphics.ES30;

namespace Magnesium.OpenGL
{
	public class FullDepthCapabilities : IDepthCapabilities
	{
		#region IDepthCapabilities implementation

		public void Initialize ()
		{
			EnableDepthBuffer ();
			SetDepthBufferFunc (MgCompareOp.LESS);
			SetDepthMask(true);
		}

		private bool mIsDepthBufferEnabled;
		public bool IsDepthBufferEnabled {
			get {
				return mIsDepthBufferEnabled;
			}
		}

		public void EnableDepthBuffer ()
		{
			GL.Enable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = true;
		}

		public void DisableDepthBuffer ()
		{
			GL.Disable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = false;
		}

		private static DepthFunction GetDepthFunction(MgCompareOp compare)
		{
			switch (compare)
			{
			default:
			case MgCompareOp.ALWAYS:
				return DepthFunction.Always;
			case MgCompareOp.EQUAL:
				return DepthFunction.Equal;
			case MgCompareOp.GREATER:
				return DepthFunction.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return DepthFunction.Gequal;
			case MgCompareOp.LESS:
				return DepthFunction.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return DepthFunction.Lequal;
			case MgCompareOp.NEVER:
				return DepthFunction.Never;
			case MgCompareOp.NOT_EQUAL:
				return DepthFunction.Notequal;
			}
		}

		public void SetDepthBufferFunc(MgCompareOp func)
		{
			GL.DepthFunc (GetDepthFunction (func));
		}

		public void SetDepthMask (bool isMaskOn)
		{
			GL.DepthMask(isMaskOn);
		}

		public void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange)
		{
			
		}
		#endregion
	}
}

