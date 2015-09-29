using System;
using BirdNest.MonoGame.Graphics;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame
{
	public interface IShaderRegistry : IDisposable
	{
		void Add(AssetInfo key, ShaderProgramData programID);
		void Remove(ShaderProgramData programID);
		bool TryGetValue(AssetIdentifier identifier, out ShaderProgramData result);
	}
}

