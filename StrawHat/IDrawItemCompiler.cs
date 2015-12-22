using System;

namespace StrawHat
{
	public interface IDrawItemCompiler
	{
		DrawItem Compile(StateGroup[] stack, DrawCommand command);
	}

}

