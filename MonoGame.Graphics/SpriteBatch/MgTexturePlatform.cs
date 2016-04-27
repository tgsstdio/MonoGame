// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Magnesium;
using System;
using System.Threading;

namespace MonoGame.Graphics
{
	/// <summary>
	/// For any singleton / global static variable
	/// </summary>
	public class MgTexturePlatform : IMgTexturePlatform
    {
		readonly IMgDevice mDevice;
		readonly MgAllocationCallbacks mCallbacks;
		private Int32 _lastSortingKey;

		public MgTexturePlatform (IMgDevice device, MgAllocationCallbacks callbacks)
		{
			mDevice = device;
			mCallbacks = callbacks;
		}

		#region ITexturePlatform implementation

		public void GraphicsDeviceResetting (MgImage image, MgImageView view, MgSampler sampler, MgDeviceMemory deviceMemory)
		{
			DeleteGLTexture (image, view, sampler, deviceMemory);
		}

		public Int32 GenerateSortingKey ()
		{
			return Interlocked.Increment(ref _lastSortingKey);
		}

		#endregion

		private void DeleteGLTexture(MgImage image, MgImageView view, MgSampler sampler, MgDeviceMemory deviceMemory)
        {			
			mDevice.DestroyImageView(view, mCallbacks);
			mDevice.DestroyImage(image, mCallbacks);
			mDevice.DestroySampler (sampler, mCallbacks); 
			mDevice.FreeMemory (deviceMemory, mCallbacks);
        }
    }
}

