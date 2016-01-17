namespace MonoGame.Graphics
{
	public interface IMeshBuffer
	{
		int BufferId { get; }
		SyncObject Fence {get;set;}
		float Factor {get;set;}
	}

}

