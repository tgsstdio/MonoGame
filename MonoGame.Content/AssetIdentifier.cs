using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoGame.Content
{
	[StructLayout(LayoutKind.Explicit)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct AssetIdentifier
	{
		[FieldOffset(0)]
		public UInt32 AssetId;
		[FieldOffset(0)]
		public UInt64 AssetId64;

        public AssetIdentifier Add(UInt32 key)
        {
            return new AssetIdentifier { AssetId = AssetId + key };
        }

        public AssetIdentifier Add64(UInt64 key)
        {
            return new AssetIdentifier { AssetId64 = AssetId64 + key };
        }

        private string DebuggerDisplay
        {
            get
            {
                return ToString();
            }
        }


        public override string ToString()
        {
            return string.Format("0x{0:X8} [64bit = 0x{1:X16}]", AssetId, AssetId64);
        }
    }
}

