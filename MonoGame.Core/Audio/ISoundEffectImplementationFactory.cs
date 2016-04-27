using System;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame.Core
{
	public interface ISoundEffectImplementationFactory
	{
		ISoundEffectImplementation Create();
	}
}

