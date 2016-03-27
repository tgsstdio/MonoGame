namespace Microsoft.Xna.Framework
{
	public interface IGraphicsDeviceQuery
	{
		int PreferredBackBufferHeight { get; set; }
		int PreferredBackBufferWidth { get; set; }	
		//bool IsFullScreen { get; set; }
	}
}

