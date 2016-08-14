using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdImageEntrypoint : IGLCmdImageEntrypoint
	{
		public void PerformOperation(CmdImageInstructionSet instructionSet)
		{
			throw new NotImplementedException();
		}
	}
}