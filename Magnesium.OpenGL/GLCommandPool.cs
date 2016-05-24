namespace Magnesium.OpenGL
{
	// RENDERER HERE
	public class GLCommandPool : IMgCommandPool
	{
		#region IMgCommandPool implementation
		private bool mIsDisposed = false;
		public void DestroyCommandPool (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		public Result ResetCommandPool (IMgDevice device, MgCommandPoolResetFlagBits flags)
		{
			if (mIsDisposed)
				return Result.SUCCESS;

			return Result.SUCCESS;
		}

		#endregion


	}
}

