using MonoGame.Graphics;

namespace NewFences
{
	public class MeshBufferUpdater : IMeshBufferUpdater
	{
		private readonly IFenceIndexVariable mIndexer;
		public MeshBufferUpdater(IFenceIndexVariable indexer)
		{
			mIndexer = indexer;
		}

		public IMeshBuffer[] Buffers { get; set; }
		#region IMeshBufferUpdater implementation
		public void Update()
		{
			if (Buffers != null)
			{
				int fence = mIndexer.Index;

				foreach (var buff in Buffers)
				{
					buff.Fence.WaitForGPU (fence);
					buff.UpdateAll (fence);
				}
			}
		}
		#endregion
	}
}

