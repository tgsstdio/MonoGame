namespace MonoGame.Content
{
	public interface IExtensionChecker
	{
		void Initialize();
		bool HasExtension(string extension);
	}
}

