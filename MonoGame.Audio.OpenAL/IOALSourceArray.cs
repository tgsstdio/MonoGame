using System.Collections.Generic;

namespace MonoGame.Audio.OpenAL
{
	public interface IOALSourceArray
	{
		bool IsEmpty();
		void Remove(int sourceId);
		void Add(int sourceId);
		int First();
	}
}

