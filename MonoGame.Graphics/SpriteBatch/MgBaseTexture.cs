// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Magnesium;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

namespace MonoGame.Graphics
{
	public abstract class MgBaseTexture : IMgTexture
	{
		private Int32 _sortingKey;

		public IMgImage Image { get; private set; }
		public IMgImageView View {get; private set; }
		public IMgSampler Sampler {get; private set; }
		public IMgDeviceMemory DeviceMemory { get; private set; }

		protected MgBaseTexture(Int32 key, IMgImage image, IMgImageView view, IMgSampler sampler, IMgDeviceMemory deviceMemory)
		{
			_sortingKey = key;
			Image = image;
			View = view;
			Sampler = sampler;
			DeviceMemory = deviceMemory;
		}

        /// <summary>
        /// Gets a unique identifier of this texture for sorting purposes.
        /// </summary>
        /// <remarks>
        /// <para>For example, this value is used by <see cref="SpriteBatch"/> when drawing with <see cref="SpriteSortMode.Texture"/>.</para>
        /// <para>The value is an implementation detail and may change between application launches or MonoGame versions.
        /// It is only guaranteed to stay consistent during application lifetime.</para>
        /// </remarks>
		public Int32 SortingKey
        {
            get { return _sortingKey; }
        }

		internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
        {
			int levels = 1;
			int size = Math.Max(Math.Max(width, height), depth);
            while (size > 1)
            {
				size = size / 2;
                levels++;
            }
            return levels;
        }

//		internal uint GetPitch(UInt32 width)
//        {
//			uint pitch;
//
//            switch (_format)
//            {
//                case SurfaceFormat.Dxt1:
//                case SurfaceFormat.Dxt1SRgb:
//                case SurfaceFormat.Dxt1a:
//                case SurfaceFormat.RgbPvrtc2Bpp:
//                case SurfaceFormat.RgbaPvrtc2Bpp:
//                case SurfaceFormat.RgbEtc1:
//                case SurfaceFormat.Dxt3:
//                case SurfaceFormat.Dxt3SRgb:
//                case SurfaceFormat.Dxt5:
//                case SurfaceFormat.Dxt5SRgb:
//                case SurfaceFormat.RgbPvrtc4Bpp:
//                case SurfaceFormat.RgbaPvrtc4Bpp:                    
//                    pitch = ((width + 3u) / 4u) * _format.GetSize();
//                    break;
//
//                default:
//                    pitch = width * _format.GetSize();
//                    break;
//            };
//
//            return pitch;
//        }

		#region IMgTexture implementation

		public void DestroyTexture (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			View.DestroyImageView(device, allocator);
			Image.DestroyImage(device, allocator);
			Sampler.DestroySampler(device, allocator); 
			DeviceMemory.FreeMemory(device, allocator);
		}

		#endregion
//
//        public void GraphicsDeviceResetting()
//        {
//			mPlatform.GraphicsDeviceResetting(mImage, mView, mSampler, mDeviceMemory);
//        }
    }
}

