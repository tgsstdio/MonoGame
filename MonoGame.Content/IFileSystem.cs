using System.IO;
using System;

namespace MonoGame.Content
{
	public interface IFileSystem : IDisposable
	{
		//string Path {get;}
		//void Initialize(string path);
		bool Register(BlockIdentifier id);
		bool IsRegistered(BlockIdentifier id);
		bool Exists (BlockIdentifier blockId, string path);
		Stream OpenStream(BlockIdentifier id, string path);
	}
}

