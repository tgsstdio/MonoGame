using System.IO;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.FileSystem
{
	public interface IFileSystem
	{
		string Path {get;}
		void Initialise(string path);
		bool Register(BlockIdentifier id);
		bool IsRegistered(BlockIdentifier id);
		Stream OpenStream(BlockIdentifier id, string path);
	}
}

