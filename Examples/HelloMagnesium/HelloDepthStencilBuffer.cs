using System;
using Magnesium;

namespace HelloMagnesium
{
	public class HelloDepthStencilBuffer : IMgDepthStencilBuffer
	{
		private class NullImageView : IMgImageView
		{
			#region IMgImageView implementation

			public void DestroyImageView (IMgDevice device, MgAllocationCallbacks allocator)
			{
				throw new NotImplementedException ();
			}

			#endregion
		}

		#region IMgDepthStencilBuffer implementation

		NullImageView mView;
		public HelloDepthStencilBuffer ()
		{
			mView = new NullImageView ();
		}

		public IMgImageView View {
			get {
				return mView;
			}
		}

		public void Setup ()
		{
			
		}

		public void Create (MgDepthStencilBufferCreateInfo createInfo)
		{
			
		}

		public void Dispose ()
		{
			
		}
		#endregion
	}
}

