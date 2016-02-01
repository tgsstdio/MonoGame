namespace MonoGame.Graphics
{
	public interface IMeshBuffer
	{
		int BufferId { get; }
		ISyncObject[] Fences {get;set;}
		float Factor {get;set;}

		void UpdateAll (int index);
	}

}

