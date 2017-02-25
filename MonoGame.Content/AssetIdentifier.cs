using System;
using System.Runtime.InteropServices;

namespace MonoGame.Content
{
	[StructLayout(LayoutKind.Explicit)]
	public struct AssetIdentifier
	{
		[FieldOffset(0)]
		public UInt32 AssetId;
		[FieldOffset(0)]
		public UInt64 AssetId64;
	}
}

