namespace MonoGame.Graphics
{
	public interface IBufferSyncObject : IGPUSyncObject
	{
		void Lock (int index);
		int LastPass { get; }
		void Bind(int passId);
	}

}
