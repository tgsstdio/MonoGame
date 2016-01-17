using System;
using BirdNest.MonoGame.Graphics;
using MonoGame.Content;

namespace BirdNest.MonoGame
{
	public interface IShaderRegistry : IDisposable
	{
		void Add(AssetInfo key, ShaderProgram program);
		void Remove(ShaderProgram program);
		bool TryGetValue(AssetIdentifier identifier, out ShaderProgram result);
	}
}

