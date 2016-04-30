using System;
using System.Threading;

namespace MonoGame.Core
{
	public class DefaultTextureSortingKeyGenerator : ITextureSortingKeyGenerator
	{
		private Int32 _lastSortingKey = 0;
		#region ITexturePlatform implementation

		public Int32 GenerateSortingKey ()
		{
			return Interlocked.Increment(ref _lastSortingKey);
		}

		#endregion
	}
}

