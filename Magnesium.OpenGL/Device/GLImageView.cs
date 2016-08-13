namespace Magnesium.OpenGL
{
	public class GLImageView : IMgImageView
	{
		public int TextureId { get; private set; }

		readonly IGLImageViewEntrypoint mModule;

		public GLImageView (int textureId, IGLImageViewEntrypoint module)
		{
			TextureId = textureId;
			mModule = module;
		}

		#region IMgImageView implementation
		private bool mIsDisposed = false;
		public void DestroyImageView (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mModule.DeleteImageView (TextureId);

//			int textureId = TextureId;
//			GL.DeleteTextures (1, ref textureId);

			mIsDisposed = true;
		}

		#endregion
	}
}

