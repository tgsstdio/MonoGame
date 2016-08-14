using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLSamplerEntrypoint : IGLSamplerEntrypoint
	{
		public int CreateSampler()
		{
			throw new NotImplementedException();
		}

		public void DeleteSampler(int samplerId)
		{
			throw new NotImplementedException();
		}

		public void SetTextureBorderColorF(int samplerId, float[] color)
		{
			throw new NotImplementedException();
		}

		public void SetTextureBorderColorI(int samplerId, int[] color)
		{
			throw new NotImplementedException();
		}

		public void SetTextureCompareFunc(int samplerId, MgCompareOp compareOp)
		{
			throw new NotImplementedException();
		}

		public void SetTextureMagFilter(int samplerId, MgFilter magFilter)
		{
			throw new NotImplementedException();
		}

		public void SetTextureMaxLod(int samplerId, float maxLod)
		{
			throw new NotImplementedException();
		}

		public void SetTextureMinFilter(int samplerId, MgFilter minFilter, MgSamplerMipmapMode mipmapMode)
		{
			throw new NotImplementedException();
		}

		public void SetTextureMinLod(int samplerId, float minLod)
		{
			throw new NotImplementedException();
		}

		public void SetTextureWrapR(int samplerId, MgSamplerAddressMode addressModeW)
		{
			throw new NotImplementedException();
		}

		public void SetTextureWrapS(int samplerId, MgSamplerAddressMode addressModeU)
		{
			throw new NotImplementedException();
		}

		public void SetTextureWrapT(int samplerId, MgSamplerAddressMode addressModeV)
		{
			throw new NotImplementedException();
		}
	}
}