using Microsoft.Xna.Framework;

namespace HelloMagnesium
{
	public class MockGraphicsDevicePreferences : IGraphicsDevicePreferences 
	{
		#region IGraphicsDevicePreferences implementation
		public bool PreferMultiSampling {
			get;
			set;
		}
		#endregion
	}

}
