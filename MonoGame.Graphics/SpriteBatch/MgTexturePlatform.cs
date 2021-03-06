// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Magnesium;

namespace MonoGame.Graphics
{
	/// <summary>
	/// For any singleton / global static variable
	/// </summary>
	public class MgTexturePlatform : IMgTexturePlatform
    {
		readonly IMgDevice mDevice;
		readonly IMgAllocationCallbacks mCallbacks;

		public MgTexturePlatform (IMgDevice device, IMgAllocationCallbacks callbacks)
		{
			mDevice = device;
			mCallbacks = callbacks;
		}

		#region ITexturePlatform implementation

		public void GraphicsDeviceResetting (IMgImage image, IMgImageView view, IMgSampler sampler, IMgDeviceMemory deviceMemory)
		{
			DestroyTexture (image, view, sampler, deviceMemory);
		}

		#endregion

		private void DestroyTexture(IMgImage image, IMgImageView view, IMgSampler sampler, IMgDeviceMemory deviceMemory)
        {			
			view.DestroyImageView(mDevice, mCallbacks);
			image.DestroyImage(mDevice, mCallbacks);
			sampler.DestroySampler(mDevice, mCallbacks); 
			deviceMemory.FreeMemory(mDevice, mCallbacks);
        }
    }
}

