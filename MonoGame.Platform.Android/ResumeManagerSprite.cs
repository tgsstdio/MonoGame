using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.Android
{
	public class ResumeManagerSprite : IResumeManagerSprite
	{
		IContentManager mManager;
		readonly IPresentationParameters mPresentation;

		SpriteBatch mSpriteBatch;
		string mResumeTextureName;
		Texture2D resumeTexture;

		float mRotation;
		float mScale;
		float mRotateSpeed;

		public ResumeManagerSprite (
			IContentManager manager,
			IPresentationParameters presentation,

			SpriteBatch spriteBatch,
			string resumeTextureName,
			float rotation,
			float scale,
			float rotateSpeed)
		{
			//  this.content = new ContentManager(services, "Content");
			this.mManager = manager;
			this.mPresentation = presentation;
			this.mSpriteBatch = spriteBatch;
			this.mResumeTextureName = resumeTextureName;
			this.mRotation = rotation;
			this.mScale = scale;
			this.mRotateSpeed = rotateSpeed;
		}

		#region IResumeManagerSprite implementation

		public void LoadContent ()
		{
			mManager.Unload();
			resumeTexture = mManager.Load<Texture2D>(mResumeTextureName);
		}

		public void Draw ()
		{
			mRotation += mRotateSpeed;

			int sw = mPresentation.BackBufferWidth;
			int sh = mPresentation.BackBufferHeight;

            int tw = resumeTexture.Width;
            int th = resumeTexture.Height;

            // Draw the resume texture in the middle of the screen and make it spin
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(resumeTexture,
                            new Vector2(sw / 2, sh / 2), 
                            null, Color.White, mRotation,
                            new Vector2(tw / 2, th / 2),
                            mScale, SpriteEffects.None, 0.0f);

            mSpriteBatch.End();
		}

		#endregion
	}
}

