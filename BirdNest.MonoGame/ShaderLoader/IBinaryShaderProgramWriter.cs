using System;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public interface IBinaryShaderProgramWriter
	{
		void Initialise(int bufSize);
		void WriteBinary(ShaderProgram program);
	}
}

