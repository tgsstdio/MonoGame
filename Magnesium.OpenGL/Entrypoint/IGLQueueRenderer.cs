namespace Magnesium.OpenGL
{
	public interface IGLQueueRenderer
	{
		void SetDefault ();
//		void CheckProgram (GLQueueDrawItem nextState);
		void Render (CmdBufferInstructions[] items);
	}
}

