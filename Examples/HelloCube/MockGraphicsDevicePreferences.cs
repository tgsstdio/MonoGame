using Microsoft.Xna.Framework;

namespace HelloCube
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
