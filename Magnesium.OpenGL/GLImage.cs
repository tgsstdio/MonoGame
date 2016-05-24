namespace Magnesium.OpenGL
{
	public class GLImage : IMgImage
	{
		public GLImage (int width, int height)
		{
			Width = width;
			Height = height;
		}

		public int Height {
			get;
			set;
		}

		public int Width {
			get;
			set;
		}

		#region IMgImage implementation

		public void DestroyImage (IMgDevice device, MgAllocationCallbacks allocator)
		{

		}

		#endregion
	}
}

