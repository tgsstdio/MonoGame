namespace Magnesium.OpenGL.UnitTests
{
	public interface IComposer
	{
		CmdBufferInstructions Compose (GLCmdBufferRepository repository, GLCmdRenderPassCommand[] passes);
	}
}

