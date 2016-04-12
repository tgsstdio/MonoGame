using System;

namespace Magnesium
{
	// UNIONS
	public struct ClearColorValue
	{
		float[] float32; // m4;
		Int32[] int32; // m4;
		UInt32[] uint32; // m4;
	}

	public struct ClearValue
	{
		public ClearColorValue mColor;
		public ClearDepthStencilValue mDepthStencil;
	}
}

