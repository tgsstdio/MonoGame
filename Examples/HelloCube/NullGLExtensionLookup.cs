using MonoGame.Graphics;

namespace HelloCube
{
	public class NullGLExtensionLookup : IGLExtensionLookup
	{
		#region IGLExtensionLookup implementation
		public void Initialise ()
		{
			//throw new NotImplementedException ();
		}
		public bool HasExtension (string extension)
		{
			return false;
		}
		#endregion
		
	}

}
