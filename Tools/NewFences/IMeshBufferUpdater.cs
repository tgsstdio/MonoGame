using MonoGame.Graphics;

namespace NewFences
{
	public interface IMeshBufferUpdater
	{
		IMeshBuffer[] Buffers { get; set; }
		void Update();
	}
}

