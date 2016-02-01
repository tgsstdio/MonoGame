using System;
using MonoGame.Content;

namespace MonoGame.Shaders
{
	public interface IShaderRegistry : IDisposable
	{
		void Add(AssetInfo key, ShaderProgram program);
		void Remove(ShaderProgram program);
		bool TryGetValue(AssetIdentifier identifier, out ShaderProgram result);
	}
}

