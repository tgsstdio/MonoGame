using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public interface ICmdBufferInstructionSetComposer
	{
		CmdBufferInstructionSet Compose (GLCmdBufferRepository repository, IEnumerable<GLCmdRenderPassCommand> passes);
	}
}

