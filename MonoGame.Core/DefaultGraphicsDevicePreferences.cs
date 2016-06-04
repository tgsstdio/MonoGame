using Microsoft.Xna.Framework;

namespace MonoGame.Core
{
	public class DefaultGraphicsDevicePreferences : IGraphicsDevicePreferences 
	{
		public DefaultGraphicsDevicePreferences ()
		{
			PreferMultiSampling = true;
		}

		#region IGraphicsDevicePreferences implementation
		public bool PreferMultiSampling {
			get;
			set;
		}
		#endregion
	}

}
