using Microsoft.Xna.Framework.Graphics;
using Magnesium;
using OpenTK.Graphics;

namespace HelloMagnesium
{
	public interface IMgDeviceQuery
	{
		int GetStencilBit (DepthFormat format);
		int GetStencilBit (MgFormat format);

		int GetSwapInterval (PresentInterval interval);

		int GetDepthBit (DepthFormat format);
		int GetDepthBit (MgFormat format);

		ColorFormat GetColorFormat(MgFormat format);
		MgFormat GetFormat (SurfaceFormat format);
		MgFormat GetDepthStencilFormat (DepthFormat format);
	}

}

