using System.IO;
using System;

namespace MonoGame.Content
{
	public interface IFileSystem : IDisposable
	{
		//string Path {get;}
		//void Initialize(string path);
//		bool Register(string id);
//		bool IsRegistered(string id);
		bool Exists (string blockPath, string localPath);
		Stream OpenStream(string blockPath, string localPath);
	}
}

