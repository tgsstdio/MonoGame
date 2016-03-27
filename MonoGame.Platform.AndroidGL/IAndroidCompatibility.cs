using Microsoft.Xna.Framework;

namespace MonoGame.Platform.AndroidGL
{
	public interface IAndroidCompatibility
	{
		DisplayOrientation GetAbsoluteOrientation ();
		DisplayOrientation GetAbsoluteOrientation (int orientation);
	}
}
