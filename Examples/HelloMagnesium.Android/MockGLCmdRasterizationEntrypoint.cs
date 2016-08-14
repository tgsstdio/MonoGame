using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdRasterizationEntrypoint : IGLCmdRasterizationEntrypoint
	{
		public bool CullingEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool ScissorTestEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void DisableCulling()
		{
			throw new NotImplementedException();
		}

		public void DisablePolygonOffset()
		{
			throw new NotImplementedException();
		}

		public void DisableScissorTest()
		{
			throw new NotImplementedException();
		}

		public void EnableCulling()
		{
			throw new NotImplementedException();
		}

		public void EnablePolygonOffset(float slopeScaleDepthBias, float depthBias)
		{
			throw new NotImplementedException();
		}

		public void EnableScissorTest()
		{
			throw new NotImplementedException();
		}

		public GLQueueRendererRasterizerState Initialize()
		{
			throw new NotImplementedException();
		}

		public void SetCullingMode(bool front, bool back)
		{
			throw new NotImplementedException();
		}

		public void SetLineWidth(float width)
		{
			throw new NotImplementedException();
		}

		public void SetUsingCounterClockwiseWindings(bool b)
		{
			throw new NotImplementedException();
		}
	}
}