// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using MonoGame.Graphics;
using Magnesium;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Graphics
{
	public abstract partial class Texture
	{
		private ITexturePlatform mPlatform;
		private Int32 _sortingKey;

		internal MgImage mImage;
		internal MgImageView mView;

		protected Texture(ITexturePlatform platform)
		{
			mPlatform = platform;
			_sortingKey = mPlatform.GenerateSortingKey ();
		}

		internal SurfaceFormat _format;
		internal int _levelCount;

        /// <summary>
        /// Gets a unique identifier of this texture for sorting purposes.
        /// </summary>
        /// <remarks>
        /// <para>For example, this value is used by <see cref="SpriteBatch"/> when drawing with <see cref="SpriteSortMode.Texture"/>.</para>
        /// <para>The value is an implementation detail and may change between application launches or MonoGame versions.
        /// It is only guaranteed to stay consistent during application lifetime.</para>
        /// </remarks>
		internal Int32 SortingKey
        {
            get { return _sortingKey; }
        }

		public SurfaceFormat Format
		{
			get { return _format; }
		}
		
		public int LevelCount
		{
			get { return _levelCount; }
		}

		internal static uint CalculateMipLevels(UInt32 width, UInt32 height = 0, UInt32 depth = 0)
        {
			uint levels = 1u;
			UInt32 size = Math.Max(Math.Max(width, height), depth);
            while (size > 1u)
            {
				size = size / 2u;
                levels++;
            }
            return levels;
        }

		internal uint GetPitch(UInt32 width)
        {
			uint pitch;

            switch (_format)
            {
                case SurfaceFormat.Dxt1:
                case SurfaceFormat.Dxt1SRgb:
                case SurfaceFormat.Dxt1a:
                case SurfaceFormat.RgbPvrtc2Bpp:
                case SurfaceFormat.RgbaPvrtc2Bpp:
                case SurfaceFormat.RgbEtc1:
                case SurfaceFormat.Dxt3:
                case SurfaceFormat.Dxt3SRgb:
                case SurfaceFormat.Dxt5:
                case SurfaceFormat.Dxt5SRgb:
                case SurfaceFormat.RgbPvrtc4Bpp:
                case SurfaceFormat.RgbaPvrtc4Bpp:                    
                    pitch = ((width + 3u) / 4u) * _format.GetSize();
                    break;

                default:
                    pitch = width * _format.GetSize();
                    break;
            };

            return pitch;
        }

        internal protected void GraphicsDeviceResetting()
        {
			mPlatform.GraphicsDeviceResetting(mImage, mView);
        }
    }
}

