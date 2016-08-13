using Magnesium.OpenGL;


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
