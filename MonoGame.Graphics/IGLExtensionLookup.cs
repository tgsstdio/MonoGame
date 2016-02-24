using System;

namespace MonoGame.Graphics
{
	public interface IGLExtensionLookup
	{
		void Initialise();

		bool HasExtension (string extension);
	}
}

