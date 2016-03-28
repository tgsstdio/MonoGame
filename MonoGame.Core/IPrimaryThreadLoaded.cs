namespace Microsoft.Xna.Framework
{
	/// <summary>
	/// Interface used to add an object to be loaded on the primary thread
	/// </summary>
	public interface IPrimaryThreadLoaded
	{
		bool Load();
	}
}

