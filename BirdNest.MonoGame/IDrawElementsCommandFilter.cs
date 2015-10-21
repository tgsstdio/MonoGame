using System;

namespace BirdNest.MonoGame
{
	public interface IDrawElementsCommandFilter : IDisposable
	{
		DrawElementsIndirectCommand[] ToArray();
	}
}

