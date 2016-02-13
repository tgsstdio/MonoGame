using MonoGame.Platform.DesktopGL;

namespace HelloCube
{
	public class NullOpenTKWindowResetter : IOpenTKWindowResetter
	{
		#region IOpenTKGamePlatform implementation
		public void ResetWindowBounds ()
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}
