using System;

namespace StrawHat
{
	[Flags]
	public enum RasterizerStateBitFlags : byte
	{
		Off = 0,
		UseCounterClockwiseWindings = 1,
		CullingEnabled = 2,
		CullBackFaces = 4,
		CullFrontFaces = 8,
		ScissorTestEnabled = 16,
		PolygonOffsetFillEnabled = 32,
		DepthClipEnabled =  64,
	}
}

