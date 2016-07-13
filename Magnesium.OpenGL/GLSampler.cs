using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class GLSampler : IMgSampler
	{
		private static All GetAddressMode(MgSamplerAddressMode mode)
		{
			switch (mode)
			{
			case MgSamplerAddressMode.CLAMP_TO_BORDER:
				return All.ClampToBorder;
			case MgSamplerAddressMode.CLAMP_TO_EDGE:
				return All.ClampToEdge;
			case MgSamplerAddressMode.MIRRORED_REPEAT:
				return All.MirroredRepeat;
				// EXT ARB_texture_mirror_clamp_to_edge
			case MgSamplerAddressMode.MIRROR_CLAMP_TO_EDGE:
				return All.MirrorClampToEdge;
			case MgSamplerAddressMode.REPEAT:
				return All.Repeat;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetMinFilterValue(MgFilter filter, MgSamplerMipmapMode mode)
		{
			switch (filter)
			{
			case MgFilter.LINEAR:
				return (mode == MgSamplerMipmapMode.LINEAR) ? All.LinearMipmapLinear : All.Linear;
			case MgFilter.NEAREST:
				return (mode == MgSamplerMipmapMode.LINEAR) ? All.NearestMipmapLinear : All.Nearest;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetMagFilterValue(MgFilter filter)
		{
			switch (filter)
			{
			case MgFilter.LINEAR:
				return All.Linear;
			case MgFilter.NEAREST:
				return All.Nearest;
			default:
				throw new NotSupportedException();
			}
		}

		private static All GetCompareOp (MgCompareOp compareOp)
		{
			switch (compareOp)
			{
			case MgCompareOp.ALWAYS:
				return All.Always;
			case MgCompareOp.EQUAL:
				return All.Equal;
			case MgCompareOp.LESS:
				return All.Less;
			case MgCompareOp.LESS_OR_EQUAL:
				return All.Lequal;
			case MgCompareOp.GREATER:
				return All.Greater;
			case MgCompareOp.GREATER_OR_EQUAL:
				return All.Gequal;
			case MgCompareOp.NOT_EQUAL:
				return All.Notequal;
			case MgCompareOp.NEVER:
				return All.Never;
			default:
				throw new NotSupportedException();
			}
		}

		public int SamplerId { get; private set; }
		public GLSampler (int samplerId, MgSamplerCreateInfo pCreateInfo)
		{
			SamplerId = samplerId;
			Populate (pCreateInfo);
		}

		private void Populate (MgSamplerCreateInfo pCreateInfo)
		{
			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (BEFORE) : " + error);
			}

			// ARB_SAMPLER_OBJECTS
			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapS, (int) GetAddressMode(pCreateInfo.AddressModeU));

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureWrapS) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapT, (int) GetAddressMode(pCreateInfo.AddressModeV));

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureWrapT) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureWrapR, (int) GetAddressMode(pCreateInfo.AddressModeW));

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureWrapR) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMinLod, pCreateInfo.MinLod);

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureMinLod) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMaxLod, pCreateInfo.MaxLod);

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureMaxLod) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMinFilter, (int) GetMinFilterValue(pCreateInfo.MinFilter, pCreateInfo.MipmapMode));

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureMinFilter) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureMagFilter, (int) GetMagFilterValue(pCreateInfo.MagFilter));

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureMagFilter) : " + error);
			}

			GL.SamplerParameter (SamplerId, SamplerParameterName.TextureCompareFunc, (int) GetCompareOp(pCreateInfo.CompareOp) );

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureCompareFunc) : " + error);
			}

			// EXT_texture_filter_anisotropic
			//GL.SamplerParameter (samplerId, SamplerParameterName.TextureMaxAnisotropyExt, pCreateInfo.MaxAnisotropy);

			switch (pCreateInfo.BorderColor)
			{
			case MgBorderColor.FLOAT_OPAQUE_BLACK:
				var FLOAT_OPAQUE_BLACK = new float[] {
					0f,
					0f,
					0f,
					1f
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, FLOAT_OPAQUE_BLACK);

				break;
			case MgBorderColor.FLOAT_OPAQUE_WHITE:
				var FLOAT_OPAQUE_WHITE = new float[] {
					1f,
					1f,
					1f,
					1f
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, FLOAT_OPAQUE_WHITE);
				break;
			case MgBorderColor.FLOAT_TRANSPARENT_BLACK:
				var FLOAT_TRANSPARENT_BLACK = new float[] {
					0f,
					0f,
					0f,
					0f
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, FLOAT_TRANSPARENT_BLACK);
				break;
			case MgBorderColor.INT_OPAQUE_BLACK:
				var INT_OPAQUE_BLACK = new int[] {
					0,
					0,
					0,
					255
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, INT_OPAQUE_BLACK);
				break;
			case MgBorderColor.INT_OPAQUE_WHITE:
				var INT_OPAQUE_WHITE = new int[] {
					255,
					255,
					255,
					255
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, INT_OPAQUE_WHITE);
				break;
			case MgBorderColor.INT_TRANSPARENT_BLACK:
				var INT_TRANSPARENT_BLACK = new int[] {
					0,
					0,
					0,
					0
				};
				GL.SamplerParameter (SamplerId, SamplerParameterName.TextureBorderColor, INT_TRANSPARENT_BLACK);
				break;
			}

			{
				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "SamplerParameter (TextureBorderColor) : " + error);
			}
		}

		#region IMgSampler implementation
		private bool mIsDisposed = false;
		public void DestroySampler (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			int samplerId = SamplerId;
			GL.DeleteSamplers (1, ref samplerId);

			mIsDisposed = true;
		}

		#endregion
	}
}

