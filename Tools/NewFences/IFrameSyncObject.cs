using MonoGame.Graphics;

namespace NewFences
{
	public interface IFrameSyncObject : IGPUSyncObject
	{
		void Lock (int index);
	}
}
