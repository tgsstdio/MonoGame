namespace Magnesium.OpenGL
{
	public class GLImageArraySubresource
	{
		public GLImageArraySubresource (uint index, MgSubresourceLayout [] layers)
		{
			Index = index;
			Levels = layers;
		}

		public uint Index { get; private set; }
		public MgSubresourceLayout [] Levels {get; private set;}
	}
}

