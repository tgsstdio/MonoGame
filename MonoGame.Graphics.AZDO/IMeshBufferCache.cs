using System.Collections.Generic;

namespace MonoGame.Graphics.AZDO
{
	public interface IMeshBufferCache
	{
		IList<IMeshBuffer> Buffers { get; }
	}
}

