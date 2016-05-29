using System;

namespace Magnesium.OpenGL
{
	public class GLEntrypoint : IMgEntrypoint
	{
		private readonly IGLQueue mQueue;
		public GLEntrypoint(IGLQueue queue)
		{
			mQueue = queue;
		}

		#region IMgEntrypoint implementation

		public Result CreateInstance (MgInstanceCreateInfo createInfo, MgAllocationCallbacks allocator, out IMgInstance instance)
		{
			instance = new GLInstance (mQueue);
			return Result.SUCCESS;
		}

		public Result EnumerateInstanceLayerProperties (out MgLayerProperties[] properties)
		{
			throw new NotImplementedException ();
		}

		public Result EnumerateInstanceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

