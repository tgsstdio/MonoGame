using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLImageView : IMgImageView
	{
		public int TextureId { get; private set; }
		public GLImageView (int textureId)
		{
			TextureId = textureId;
		}

		#region IMgImageView implementation
		private bool mIsDisposed = false;
		public void DestroyImageView (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			int textureId = TextureId;
			GL.DeleteTextures (1, ref textureId);

			mIsDisposed = true;
		}

		#endregion
	}
}

