using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class FullScissorsCapabilities : IScissorsCapabilities
	{
		#region IScissorsCapabilities implementation

		public void ApplyScissors (GLCmdScissorParameter scissors)
		{
			GL.ScissorArray (scissors.Parameters.First, scissors.Parameters.Count, scissors.Parameters.Values);
		}


		public void ApplyViewports (GLCmdViewportParameter viewports)
		{			
			GL.ViewportArray (viewports.Viewport.First, viewports.Viewport.Count, viewports.Viewport.Values);
			GL.DepthRangeArray (viewports.DepthRange.First, viewports.DepthRange.Count, viewports.DepthRange.Values);
		}

		#endregion
	}
}

