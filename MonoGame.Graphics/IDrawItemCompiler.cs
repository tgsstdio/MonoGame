namespace MonoGame.Graphics
{
	public interface IDrawItemCompiler
	{
		DrawItemCompilerOutput[] Compile(StateGroup[] stack);
	}

}

