using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class FullRasterizerCapabilities : IRasterizerCapabilities
	{
		#region IRasterizerCapabilities implementation

		public void SetLineWidth (float width)
		{
			if (width > 0f)
			{
				GL.Enable (EnableCap.LineSmooth);
				GL.LineWidth (width);
			} 
			else
			{
				GL.Disable (EnableCap.LineSmooth);
			}

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetLineWidth : " + error);
				}
			}
		}

		public void DisablePolygonOffset ()
		{
			GL.Disable(EnableCap.PolygonOffsetFill);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DisablePolygonOffset : " + error);
				}
			}
		}

		public void EnablePolygonOffset (float slopeScaleDepthBias, float depthBias)
		{
			GL.Enable(EnableCap.PolygonOffsetFill);
			GL.PolygonOffset(slopeScaleDepthBias, depthBias);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnablePolygonOffset : " + error);
				}
			}
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

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetUsingCounterClockwiseWindings : " + error);
				}
			}
		}

		public void EnableScissorTest ()
		{
			GL.Enable(EnableCap.ScissorTest);
			mScissorTestEnabled = true;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableScissorTest : " + error);
				}
			}
		}

		public void DisableScissorTest ()
		{
			GL.Disable(EnableCap.ScissorTest);

			mScissorTestEnabled = false;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DisableScissorTest : " + error);
				}
			}
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

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetCullingMode : " + error);
				}
			}
		}

		public void EnableCulling ()
		{
			GL.Enable(EnableCap.CullFace);
			mCullingEnabled = true;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableCulling : " + error);
				}
			}
		}

		public void DisableCulling ()
		{
			GL.Disable(EnableCap.CullFace);
			mCullingEnabled = false;

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DisableCulling : " + error);
				}
			}
		}

		public GLQueueRendererRasterizerState Initialize ()
		{
			var initialValue = new GLQueueRendererRasterizerState {
				Flags = 
					GLGraphicsPipelineFlagBits.ScissorTestEnabled 
					| GLGraphicsPipelineFlagBits.CullBackFaces 
					| GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings,
					// ! QueueDrawItemBitFlags.CullingEnabled,
				DepthBias = new GLCmdDepthBiasParameter
				{
					DepthBiasClamp = 0,
					DepthBiasConstantFactor = 0,
					DepthBiasSlopeFactor = 0,
				},
				LineWidth = 1f,
			};

			EnableScissorTest ();
			DisableCulling ();

			SetCullingMode (
				(initialValue.Flags & GLGraphicsPipelineFlagBits.CullFrontFaces)
				== GLGraphicsPipelineFlagBits.CullFrontFaces,
				(initialValue.Flags & GLGraphicsPipelineFlagBits.CullBackFaces)
				== GLGraphicsPipelineFlagBits.CullBackFaces);
			SetUsingCounterClockwiseWindings (
				(initialValue.Flags & GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings)
					== GLGraphicsPipelineFlagBits.UseCounterClockwiseWindings
			);

			DisablePolygonOffset ();

			SetLineWidth (initialValue.LineWidth);

			return initialValue;
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

