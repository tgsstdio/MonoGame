using System.Collections.Generic;
using System;


namespace Magnesium.OpenGL
{
	public class GLRenderPass : IGLRenderPass
	{
		static GLClearAttachmentType GetAttachmentType (MgFormat format)
		{
			switch (format)
			{
			case MgFormat.D16_UNORM:
			case MgFormat.D16_UNORM_S8_UINT:
			case MgFormat.D24_UNORM_S8_UINT:
			case MgFormat.D32_SFLOAT:
			case MgFormat.D32_SFLOAT_S8_UINT:				
				return GLClearAttachmentType.DEPTH_STENCIL;

			case MgFormat.R8_SINT:
			case MgFormat.R8G8_SINT:
			case MgFormat.R8G8B8_SINT:
			case MgFormat.R8G8B8A8_SINT:
			case MgFormat.R16_SINT:
			case MgFormat.R16G16_SINT:
			case MgFormat.R16G16B16_SINT:
			case MgFormat.R16G16B16A16_SINT:
			case MgFormat.R32_SINT:
			case MgFormat.R32G32_SINT:
			case MgFormat.R32G32B32_SINT:
			case MgFormat.R32G32B32A32_SINT:
			case MgFormat.R64_SINT:
			case MgFormat.R64G64_SINT:
			case MgFormat.R64G64B64_SINT:
			case MgFormat.R64G64B64A64_SINT:
				return GLClearAttachmentType.COLOR_INT;

			case MgFormat.R8_UINT:
			case MgFormat.R8G8_UINT:
			case MgFormat.R8G8B8_UINT:
			case MgFormat.R8G8B8A8_UINT:
			case MgFormat.R16_UINT:
			case MgFormat.R16G16_UINT:
			case MgFormat.R16G16B16_UINT:
			case MgFormat.R16G16B16A16_UINT:
			case MgFormat.R32_UINT:
			case MgFormat.R64_UINT:
				return GLClearAttachmentType.COLOR_UINT;

			case MgFormat.R32_SFLOAT:
			case MgFormat.R32G32_SFLOAT:
			case MgFormat.R32G32B32_SFLOAT:
			case MgFormat.R32G32B32A32_SFLOAT:
				return GLClearAttachmentType.COLOR_FLOAT;
			default:
				throw new NotSupportedException();
			}
		}

		public GLClearAttachmentType[] AttachmentFormats { get; private set; }
		public GLRenderPass (MgFormat[] attachmentFormats)
		{
			var attachmentTypes = new List<GLClearAttachmentType> ();
			for (uint i = 0; i < attachmentFormats.Length; ++i)
			{
				attachmentTypes.Add(GetAttachmentType (attachmentFormats[i]));
			}

			AttachmentFormats = attachmentTypes.ToArray();
		}

		#region IMgRenderPass implementation
		public void DestroyRenderPass (IMgDevice device, MgAllocationCallbacks allocator)
		{

		}
		#endregion
	}
}

