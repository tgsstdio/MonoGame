using System;

namespace MonoGame.Content
{
	public class MaskedBlockLocator : IBlockLocator
	{
		#region IBlockLocator implementation
		private const UInt32 FRONT_MASK = 0xffff0000;
		private const UInt32 BACK_MASK = 0x0000ffff;

		public string GetBlockPath (AssetIdentifier identifier)
		{
			// 16 bit reserved for blocks
			UInt32 front = identifier.AssetId & FRONT_MASK;

			return front.ToString("x8");
		}

		public string GetLocalPath(AssetIdentifier identifier)
		{
			// 16 bits reserved for assets in block
			UInt32 back = identifier.AssetId;

			return back.ToString("x8");
		}

		#endregion
	}
}

