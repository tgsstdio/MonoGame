using Magnesium.OpenGL;


namespace HelloMagnesium
{
	public class MockGLSemaphoreGenerator : IGLSemaphoreGenerator
	{
		#region IGLSemaphoreGenerator implementation
		public ISyncObject Generate ()
		{
			return new GLQueueSemaphore ();
		}
		#endregion
	}
}
