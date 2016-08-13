
namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLImageFormatEntrypoint : IGLImageFormatEntrypoint
	{
		#region IGLImageFormatEntrypoint implementation
		public GLInternalImageFormat GetGLFormat (MgFormat format, bool supportsSRgb)
		{
			return new GLInternalImageFormat {

			};
		}
		#endregion
	}

}

