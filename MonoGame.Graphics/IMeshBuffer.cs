namespace MonoGame.Graphics
{
	public interface IMeshBuffer
	{
		int BufferId { get; }
		float Factor {get;set;}

		void UpdateAll (int index);
		IBufferSyncObject Fence { get; set; }
	}

}

