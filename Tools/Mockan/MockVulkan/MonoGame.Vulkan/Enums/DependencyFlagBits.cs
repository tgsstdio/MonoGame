using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum DependencyFlagBits : byte
	{
		// Dependency is per pixel region 
		VK_DEPENDENCY_BY_REGION_BIT = 1 << 0,
	}
}

