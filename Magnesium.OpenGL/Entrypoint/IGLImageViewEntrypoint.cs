namespace Magnesium.OpenGL
{
	public interface IGLImageViewEntrypoint
	{
		int CreateImageView (GLImage originalImage, MgImageViewCreateInfo pCreateInfo);
		void DeleteImageView(int texture);
	}
}

