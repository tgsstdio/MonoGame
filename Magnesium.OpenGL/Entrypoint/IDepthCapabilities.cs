﻿using System;

namespace Magnesium.OpenGL
{
	public interface IDepthCapabilities
	{
		void Initialize();

		bool IsDepthBufferEnabled { get; }		
		void EnableDepthBuffer();
		void DisableDepthBuffer();

		void SetDepthBufferFunc(MgCompareOp func);
		void SetDepthMask (bool isMaskOn);

		void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange);
	}
}

