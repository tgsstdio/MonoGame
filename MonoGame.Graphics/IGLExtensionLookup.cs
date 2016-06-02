using System;

namespace MonoGame.Graphics
{
	public interface IGLExtensionLookup
	{
		void Initialize();

		bool HasExtension (string extension);
	}
}

