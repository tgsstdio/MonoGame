using Microsoft.Xna.Framework.Graphics;
using Magnesium;

namespace MonoGame.Graphics
{
	public interface IMgDeviceQuery
	{
		int GetStencilBit (DepthFormat format);
		int GetStencilBit (MgFormat format);

		int GetSwapInterval (PresentInterval interval);

		int GetDepthBit (DepthFormat format);
		int GetDepthBit (MgFormat format);

		MgFormat GetFormat (SurfaceFormat format);
		MgFormat GetDepthStencilFormat (DepthFormat format);
	}

}

