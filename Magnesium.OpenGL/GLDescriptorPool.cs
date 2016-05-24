using System.Collections.Concurrent;

namespace Magnesium.OpenGL
{
	public class GLDescriptorPool : IMgDescriptorPool
	{
		public GLDescriptorPool (int poolSize)
		{
			Sets = new ConcurrentBag<GLDescriptorSet> ();

			for (int i = 0; i < poolSize; ++i)
			{
				var descriptorSet = new GLDescriptorSet (i);
				Sets.Add (descriptorSet);
			}
		}

		public ConcurrentBag<GLDescriptorSet> Sets { get; private set; }

		#region IMgDescriptorPool implementation

		public Result ResetDescriptorPool (IMgDevice device, uint flags)
		{
			throw new System.NotImplementedException ();
		}

		private bool mIsDisposed = false;
		public void DestroyDescriptorPool (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		#endregion
	}
}

