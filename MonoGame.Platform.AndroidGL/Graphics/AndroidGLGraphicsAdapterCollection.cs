using Microsoft.Xna.Framework.Graphics;
using Android.Views;

namespace MonoGame.Platform.AndroidGL.Graphics
{
	public class AndroidGLGraphicsAdapterCollection : IGraphicsAdapterCollection
	{
		public IGraphicsAdapter[] Options { get; private set;}
		public AndroidGLGraphicsAdapterCollection (IAndroidGameActivity activity)
		{
			Options = new IGraphicsAdapter[]{ new AndroidGLGraphicsAdapter (activity) };
		}
	}
}

