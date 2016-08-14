using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLImageFormatEntrypoint : IGLImageFormatEntrypoint
	{
		public GLInternalImageFormat GetGLFormat(MgFormat format, bool supportsSRgb)
		{
			throw new NotImplementedException();
		}
	}
}