using System;

namespace MonoGame.Content
{
	public class StandardBlockLocator : IBlockLocator
	{
		#region IBlockLocator implementation
		private const UInt32 FRONT_MASK = 0xffff0000;
		private const UInt32 BACK_MASK = 0x0000ffff;

		public BlockIdentifier GetSource (AssetIdentifier identifier)
		{
			// 16 bit reserved for blocks
			UInt32 Front = identifier.AssetId & FRONT_MASK;

			// 16 bits reserved for assets in block
			//const UInt32 Back = identifier.AssetId & 0x0000ffff;

			return new BlockIdentifier{ BlockId = ReverseBits (Front)};
		}

		public string GetLocalPath(AssetIdentifier identifier)
		{
			// 16 bits reserved for assets in block
			UInt32 Back = identifier.AssetId & BACK_MASK;

			return Back.ToString ();
		}

		#endregion

		private static UInt32 ReverseBits(UInt32 x)
		{
			// FROM HACKER'S DELIGHT 9th printing (2009)
			// ISBN-13 978-0-201-91465-8
			// Henry S. Warren Jr. pg 101
			x = (x & 0x55555555) << 1  | (x & 0xaaaaaaaa) >> 1;
			x = (x & 0x33333333) << 2  | (x & 0xcccccccc) >> 2;
			x = (x & 0x0f0f0f0f) << 4  | (x & 0xf0f0f0f0) >> 4;
			x = (x & 0x00ff00ff) << 8  | (x & 0xff00ff00) >> 8;
			x = (x & 0x0000ffff) << 16 | (x & 0xffff0000) >> 16;
			return x;
		}
	}
}

