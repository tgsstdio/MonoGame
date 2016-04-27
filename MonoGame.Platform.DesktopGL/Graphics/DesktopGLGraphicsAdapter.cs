// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.DesktopGL.Graphics
{
    public class DesktopGLGraphicsAdapter : IGraphicsAdapter
    {      
        private DisplayModeCollection _supportedDisplayModes;   
        
		public DesktopGLGraphicsAdapter ()
		{
			InitialiseSupportedModes ();
		}

		private void InitialiseSupportedModes()
		{
			var modes = new List<DisplayMode>(new[] { CurrentDisplayMode, });

			var displays = new List<OpenTK.DisplayDevice>();

			OpenTK.DisplayIndex[] displayIndices = {
				OpenTK.DisplayIndex.First,
				OpenTK.DisplayIndex.Second,
				OpenTK.DisplayIndex.Third,
				OpenTK.DisplayIndex.Fourth,
				OpenTK.DisplayIndex.Fifth,
				OpenTK.DisplayIndex.Sixth,
			};

			foreach(var displayIndex in displayIndices) 
			{
				var currentDisplay = OpenTK.DisplayDevice.GetDisplay(displayIndex);
				if(currentDisplay!= null) displays.Add(currentDisplay);
			}

			if (displays.Count > 0)
			{
				modes.Clear();
				foreach (OpenTK.DisplayDevice display in displays)
				{
					foreach (OpenTK.DisplayResolution resolution in display.AvailableResolutions)
					{                                
						SurfaceFormat format = SurfaceFormat.Color;
						switch (resolution.BitsPerPixel)
						{
						case 32: format = SurfaceFormat.Color; break;
						case 16: format = SurfaceFormat.Bgr565; break;
						case 8: format = SurfaceFormat.Bgr565; break;
						default:
							break;
						}
						// Just report the 32 bit surfaces for now
						// Need to decide what to do about other surface formats
						if (format == SurfaceFormat.Color)
						{
							modes.Add(new DisplayMode(resolution.Width, resolution.Height, (int)resolution.RefreshRate, format));
						}
					}

				}
			}
			_supportedDisplayModes = new DisplayModeCollection(modes);

		}

        public DisplayMode CurrentDisplayMode
        {
            get
            {
                return new DisplayMode( OpenTK.DisplayDevice.Default.Width, OpenTK.DisplayDevice.Default.Height, (int)OpenTK.DisplayDevice.Default.RefreshRate, SurfaceFormat.Color);
            }
        }
    
        public DisplayModeCollection SupportedDisplayModes
        {
            get
            {
                return _supportedDisplayModes;
            }
        }

        /// <summary>
        /// Gets a <see cref="System.Boolean"/> indicating whether
        /// <see cref="GraphicsAdapter.CurrentDisplayMode"/> has a
        /// Width:Height ratio corresponding to a widescreen <see cref="DisplayMode"/>.
        /// Common widescreen modes include 16:9, 16:10 and 2:1.
        /// </summary>
        public bool IsWideScreen
        {
            get
            {
                // Common non-widescreen modes: 4:3, 5:4, 1:1
                // Common widescreen modes: 16:9, 16:10, 2:1
                // XNA does not appear to account for rotated displays on the desktop
                const float limit = 4.0f / 3.0f;
                var aspect = CurrentDisplayMode.AspectRatio;
                return aspect > limit;
            }
        }
    }
}
