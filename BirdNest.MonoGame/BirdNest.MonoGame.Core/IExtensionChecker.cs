
namespace BirdNest.MonoGame.Core
{
	public interface IExtensionChecker
	{
		void Initialize();
		bool HasExtension(string extension);
	}
}

