using Magnesium.OpenGL;
using Magnesium.OpenGL.DesktopGL;

namespace HelloMagnesium
{
	public class MockGLSemaphoreGenerator : IGLSemaphoreEntrypoint
	{
		#region IGLSemaphoreGenerator implementation
		public IGLSemaphore CreateSemaphore ()
		{
			return new GLQueueSemaphore ();
		}
		#endregion
	}
}
