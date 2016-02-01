namespace NewFences
{
	public class MeshBufferUpdater : IBufferUpdater
	{
		private readonly IFenceIndexVariable mIndexer;
		public MeshBufferUpdater(IFenceIndexVariable indexer)
		{
			mIndexer = indexer;
		}

		public MeshBuffer[] Buffers { get; set; }
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

