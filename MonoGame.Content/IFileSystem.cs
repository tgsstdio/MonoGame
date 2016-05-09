using System.IO;
using System;

namespace MonoGame.Content
{
	public interface IFileSystem : IDisposable
	{
		//string Path {get;}
		//void Initialise(string path);
		bool Register(BlockIdentifier id);
		bool IsRegistered(BlockIdentifier id);
		Stream OpenStream(BlockIdentifier id, string path);
	}
}

