using System;

namespace Magnesium.OpenGL
{
	public class GLCmdRenderPassCommand
	{
		public MgSubpassContents Contents;
		public MgClearValue[] ClearValues;
		public MgRenderPass Origin;
		public GLCmdDrawCommand[] DrawCommands;
		public GLCmdComputeCommand[] ComputeCommands;
	}
}

