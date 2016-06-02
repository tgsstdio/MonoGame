﻿using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IBlendCapabilities
	{	
		bool IsEnabled { get; }

		void Initialize();

		void EnableBlending (bool value);

		void SetColorMask (QueueDrawItemBitFlags colorMask);

		void ApplyBlendSeparateFunction (
			MgBlendFactor colorSource,
			MgBlendFactor colorDest,
			MgBlendFactor alphaSource,
			MgBlendFactor alphaDest);
	}
}

