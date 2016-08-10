// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MonoGame.Core
{
	public interface IGraphicsCapabilities
	{
		void Initialize();

		/// <summary>
		/// Whether the device fully supports non power-of-two textures, including
		/// mip maps and wrap modes other than CLAMP_TO_EDGE
		/// </summary>
		bool SupportsNonPowerOfTwo { get; }

		/// <summary>
		/// Whether the device supports anisotropic texture filtering
		/// </summary>
		bool SupportsTextureFilterAnisotropic { get; }

		bool SupportsDepth24 { get;  }

		bool SupportsPackedDepthStencil { get; }

		bool SupportsDepthNonLinear { get; }

		/// <summary>
		/// Gets the support for DXT1
		/// </summary>
		bool SupportsDxt1 { get; }

		/// <summary>
		/// Gets the support for S3TC (DXT1, DXT3, DXT5)
		/// </summary>
		bool SupportsS3tc { get;  }

		/// <summary>
		/// Gets the support for PVRTC
		/// </summary>
		bool SupportsPvrtc { get;  }

		/// <summary>
		/// Gets the support for ETC1
		/// </summary>
		bool SupportsEtc1 { get; }

		/// <summary>
		/// Gets the support for ATITC
		/// </summary>
		bool SupportsAtitc { get; }

		bool SupportsTextureMaxLevel { get; }

		/// <summary>
		/// True, if sRGB is supported. On Direct3D platforms, this is always <code>true</code>.
		/// On OpenGL platforms, it is <code>true</code> if both framebuffer sRGB
		/// and texture sRGB are supported.
		/// </summary>
		bool SupportsSRgb { get; }

		bool SupportsTextureArrays { get; }

		bool SupportsDepthClamp { get; }

		bool SupportsVertexTextures { get; }	

//		bool SupportsFramebufferObjectARB { get; }
//
//		/// <summary>
//		/// True, if GL_EXT_framebuffer_object is supported; false otherwise.
//		/// </summary>
//		bool SupportsFramebufferObjectEXT { get; }
	}
}