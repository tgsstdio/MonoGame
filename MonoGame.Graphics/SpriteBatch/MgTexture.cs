using System;
using Magnesium;

namespace MonoGame.Graphics
{
    public class MgTexture : IMgTexture
    {
        public MgTexture(IMgImage image, IMgImageView view, IMgSampler sampler, IMgDeviceMemory deviceMemory)
        {
            mImage = image;
            mImageView = view;
            mSampler = sampler;
            mDeviceMemory = deviceMemory;
        }

        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }
        public uint MipmapLevels { get; set; }
        public uint ArrayLayers { get; set; }        

        private IMgDeviceMemory mDeviceMemory;
        public IMgDeviceMemory DeviceMemory
        {
            get
            {
                return mDeviceMemory;
            }
        }

        private IMgImage mImage;
        public IMgImage Image
        {
            get
            {
                return mImage;
            }
        }

        private IMgSampler mSampler;
        public IMgSampler Sampler
        {
            get
            {
                return mSampler;
            }
        }

        private IMgImageView mImageView;
        public IMgImageView View
        {
            get
            {
                return mImageView;
            }
        }

        public void DestroyTexture(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            mImageView.DestroyImageView(device, allocator);
            mImage.DestroyImage(device, allocator);
            mSampler.DestroySampler(device, allocator);
            mDeviceMemory.FreeMemory(device, allocator);
        }
    }
}
