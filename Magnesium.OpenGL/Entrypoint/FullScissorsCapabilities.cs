using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class FullScissorsCapabilities : IScissorsCapabilities
	{
		#region IScissorsCapabilities implementation

		public void ApplyScissors (GLCmdScissorParameter scissors)
		{
			GL.ScissorArray (scissors.Parameters.First, scissors.Parameters.Count, scissors.Parameters.Values);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("ApplyScissors : " + error);
				}
			}
		}


		public void ApplyViewports (GLCmdViewportParameter viewports)
		{			
			GL.ViewportArray (viewports.Viewport.First, viewports.Viewport.Count, viewports.Viewport.Values);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("ViewportArray : " + error);
				}
			}

			GL.DepthRangeArray (viewports.DepthRange.First, viewports.DepthRange.Count, viewports.DepthRange.Values);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DepthRangeArray : " + error);
				}
			}
		}

		#endregion
	}
}

