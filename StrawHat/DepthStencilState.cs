using System.Runtime.InteropServices;

namespace StrawHat
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DepthStencilState
	{
		public CompareFunction DepthCompareFunction { get; set; }
		public CompareFunction StencilCompareFunction { get; set; }
		public StencilOperation StencilTestPassed { get; set; }
		public StencilOperation StencilTestFailed {get;set;}
		public StencilOperation StencilAndDepthTestFailed { get; set; }
		public DepthStencilBitFlags Flags { get; set; }
		public int StencilMask {get;set;}
		public int StencilWriteMask {get;set;}
	}
}

