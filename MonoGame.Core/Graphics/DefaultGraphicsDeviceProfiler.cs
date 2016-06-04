using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class DefaultGraphicsDeviceProfiler : IGraphicsProfiler
	{
		#region IGraphicsProfiler implementation

		public GraphicsProfile GetHighestSupportedGraphicsProfile ()
		{
			return GraphicsProfile.HiDef;
		}

		#endregion
	}
}

