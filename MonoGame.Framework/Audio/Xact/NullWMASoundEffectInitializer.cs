using System;

namespace Microsoft.Xna.Framework.Audio
{
	public class NullWMASoundEffectInitializer : IWMASoundEffectInitializer
	{
		#region IWMASoundEffectInitializer implementation

		// FOR DIRECTX AND WINRT
		public SoundEffect LoadEffect (byte[] audiodata, bool isWma, bool isM4a)
		{			
			throw new NotImplementedException ();
		}

		#endregion
	}
}

