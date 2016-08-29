// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Graphics
{
	public class DefaultPresentationParameters : IPresentationParameters
    {
        #region Constants

        public const int DefaultPresentRate = 60;

        #endregion Constants

        #region Private Fields

        private DepthFormat depthStencilFormat;
        private SurfaceFormat backBufferFormat;
        private int backBufferHeight;
        private int backBufferWidth;
        private IntPtr deviceWindowHandle;
        private int multiSampleCount;
        private bool isFullScreen;


        #endregion Private Fields

        #region Constructors
		private readonly IBackBufferPreferences mPreferences;
		public DefaultPresentationParameters(IBackBufferPreferences preferences)
        {
			mPreferences = preferences;
			backBufferHeight = mPreferences.DefaultBackBufferHeight;
			backBufferWidth = mPreferences.DefaultBackBufferWidth;
            Clear();
        }

        #endregion Constructors

        #region Properties

        public SurfaceFormat BackBufferFormat
        {
            get { return backBufferFormat; }
            set { backBufferFormat = value; }
        }

        public int BackBufferHeight
        {
            get { return backBufferHeight; }
            set { backBufferHeight = value; }
        }

        public int BackBufferWidth
        {
            get { return backBufferWidth; }
            set { backBufferWidth = value; }
        }

        public Rectangle Bounds 
        {
            get { return new Rectangle(0, 0, backBufferWidth, backBufferHeight); }
        }

        public IntPtr DeviceWindowHandle
        {
            get { return deviceWindowHandle; }
            set { deviceWindowHandle = value; }
        }

		public DepthFormat DepthStencilFormat
        {
            get { return depthStencilFormat; }
            set { depthStencilFormat = value; }
        }

        public bool IsFullScreen
        {
			get
            {
				 return isFullScreen;
            }
            set
            {
                isFullScreen = value;
			}
        }
		
        public int MultiSampleCount
        {
            get { return multiSampleCount; }
            set { multiSampleCount = value; }
        }
		
        public PresentInterval PresentationInterval { get; set; }

		public DisplayOrientation DisplayOrientation 
		{ 
			get; 
			set; 
		}
		
		public RenderTargetUsage RenderTargetUsage { get; set; }

        #endregion Properties

        #region Methods

        public void Clear()
        {
            backBufferFormat = SurfaceFormat.Color;

			backBufferWidth = mPreferences.DefaultBackBufferWidth;
			backBufferHeight = mPreferences.DefaultBackBufferHeight;     
            deviceWindowHandle = IntPtr.Zero;

            depthStencilFormat = DepthFormat.None;
            multiSampleCount = 0;
            PresentationInterval = PresentInterval.Default;
            DisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation.Default;
        }

        public IPresentationParameters Clone()
        {
			DefaultPresentationParameters clone = new DefaultPresentationParameters (this.mPreferences);
			clone.backBufferFormat = this.backBufferFormat;
			clone.backBufferHeight = this.backBufferHeight;
			clone.backBufferWidth = this.backBufferWidth;
			clone.deviceWindowHandle = this.deviceWindowHandle;
			clone.IsFullScreen = this.IsFullScreen;
			clone.depthStencilFormat = this.depthStencilFormat;
			clone.multiSampleCount = this.multiSampleCount;
			clone.PresentationInterval = this.PresentationInterval;
			clone.DisplayOrientation = this.DisplayOrientation;				
            return clone;
        }

        #endregion Methods

    }
}
