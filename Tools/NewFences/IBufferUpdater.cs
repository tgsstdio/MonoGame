namespace NewFences
{
	public interface IBufferUpdater
	{
		MeshBuffer[] Buffers { get; set; }
		void Update();
	}
}

