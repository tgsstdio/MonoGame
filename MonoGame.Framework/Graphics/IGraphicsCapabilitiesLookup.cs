// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

#if OPENGL
#if MONOMAC
using MonoMac.OpenGL;

#elif GLES
using OpenTK.Graphics.ES20;

#else
using OpenTK.Graphics.OpenGL;
#endif
#endif

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IGraphicsCapabilitiesLookup
	{
		void Initialise();

		bool SupportsNonPowerOfTwo ();

		bool SupportsTextureMaxLevel ();

		bool SupportsTextureFilterAnisotropic ();

		bool SupportsDepth24 ();

		bool SupportsPackedDepthStencil ();

		bool SupportsDepthNonLinear ();

		bool SupportsS3tc ();

		bool SupportsDxt1 ();

		bool SupportsPvrtc ();

		bool SupportsEtc1 ();

		bool SupportsAtitc ();

		bool SupportsFramebufferObjectARB ();

		bool SupportsFramebufferObjectEXT ();

		int GetMaxTextureAnisotropy ();

		bool SupportsSRgb ();

		bool SupportsTextureArrays ();

		bool SupportsDepthClamp ();

		bool SupportsVertexTextures ();
	}
}