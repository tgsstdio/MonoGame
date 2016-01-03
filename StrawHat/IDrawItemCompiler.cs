using System;

namespace MonoGame.Graphics
{
	public interface IDrawItemCompiler
	{
		DrawItem Compile(StateGroup[] stack, DrawCommand command);
	}

}

