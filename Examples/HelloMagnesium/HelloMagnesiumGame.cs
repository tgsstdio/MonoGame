using Microsoft.Xna.Framework;
using Magnesium;
using MonoGame.Core;
using MonoGame.Content;

namespace HelloMagnesium
{
	public class HelloMagnesiumGame : Game
	{
		private IMgThreadPartition mPartition;
		private readonly ITexture2DLoader mTex2D;
		public HelloMagnesiumGame(
			IGraphicsDeviceManager manager,
			IMgThreadPartition partition,
			ITexture2DLoader tex2DLoader
		)
		{
			mTex2D = tex2DLoader;
		}

		private ITexture2D mBackground;
		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		public override void LoadContent()
		{
			mBackground = mTex2D.Load (new AssetIdentifier {AssetId = 0x80000001});
		}
	}
}
