// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame.Platform.AndroidGL.Example
{

	public class NullGraphicsDeviceService : IGraphicsDeviceService
	{
		#region IGraphicsDeviceService implementation
		public event System.EventHandler<System.EventArgs> DeviceCreated;
		public event System.EventHandler<System.EventArgs> DeviceDisposing;
		public event System.EventHandler<System.EventArgs> DeviceReset;
		public event System.EventHandler<System.EventArgs> DeviceResetting;
		public void OnDeviceDisposing (object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException ();
		}
		public void OnDeviceResetting (object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException ();
		}
		public void OnDeviceReset (object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException ();
		}
		public void OnDeviceCreated (object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException ();
		}
		public Microsoft.Xna.Framework.IGraphicsDevice GraphicsDevice {
			get {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}	

}
