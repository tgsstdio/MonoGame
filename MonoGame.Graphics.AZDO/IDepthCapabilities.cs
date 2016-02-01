using System;

namespace MonoGame.Graphics.AZDO
{
	public interface IDepthCapabilities
	{
		void Initialise();

		bool IsDepthBufferEnabled { get; }		
		void EnableDepthBuffer();
		void DisableDepthBuffer();

		void SetDepthBufferFunc(CompareFunction func);
		void SetDepthMask (bool isMaskOn);

		void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange);
	}
}

