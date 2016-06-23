using System;

namespace Magnesium.OpenGL
{
	public interface IShaderProgram : IDisposable
	{
		void Use();
		void Unuse();
		byte DescriptorSet {get; set;}
		int VBO { get; set; }
	}
}

