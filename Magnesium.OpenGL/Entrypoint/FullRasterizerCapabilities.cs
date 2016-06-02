using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class FullRasterizerCapabilities : IRasterizerCapabilities
	{
		#region IRasterizerCapabilities implementation

		public void DisablePolygonOffset ()
		{
			GL.Disable(EnableCap.PolygonOffsetFill);
		}

		public void EnablePolygonOffset (float slopeScaleDepthBias, float depthBias)
		{
			GL.Enable(EnableCap.PolygonOffsetFill);
			GL.PolygonOffset(slopeScaleDepthBias, depthBias);
		}

		public void SetUsingCounterClockwiseWindings (bool flag)
		{
			if (flag)
			{
				GL.FrontFace (FrontFaceDirection.Ccw);
			} 
			else
			{
				GL.FrontFace (FrontFaceDirection.Cw);
			}
		}

		public void EnableScissorTest ()
		{
			GL.Enable(EnableCap.ScissorTest);
			mScissorTestEnabled = true;
		}

		public void DisableScissorTest ()
		{
			GL.Disable(EnableCap.ScissorTest);
			mScissorTestEnabled = false;
		}

		public void SetCullingMode (bool front, bool back)
		{
			if (front && back)
			{
				GL.CullFace (CullFaceMode.FrontAndBack);
			}
			else if (front)
			{
				GL.CullFace (CullFaceMode.Front);
			}
			else if (back)
			{
				GL.CullFace (CullFaceMode.Back);
			}
			else
			{
				// not sure about this
				DisableCulling ();
			}
		}

		public void EnableCulling ()
		{
			GL.Enable(EnableCap.CullFace);
			mCullingEnabled = true;
		}

		public void DisableCulling ()
		{
			GL.Disable(EnableCap.CullFace);
			mCullingEnabled = false;
		}

		public void Initialize ()
		{
			EnableScissorTest ();
			DisableCulling ();
			SetCullingMode (false, true);
			SetUsingCounterClockwiseWindings (true);
			DisablePolygonOffset ();
		}

		private bool mScissorTestEnabled;
		public bool ScissorTestEnabled {
			get {
				return mScissorTestEnabled;
			}
		}

		private bool mCullingEnabled;
		public bool CullingEnabled {
			get {
				return mCullingEnabled;
			}
		}

		#endregion


	}
}

