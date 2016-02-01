namespace MonoGame.Graphics
{
	public interface IPassSyncObject : IGPUSyncObject
	{
		void Lock (int index);
	}
}
