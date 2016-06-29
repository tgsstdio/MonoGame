using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class FullGLCmdImageCapabilities : IGLCmdImageCapabilities
	{
		#region IGLCmdImageCapabilities implementation

		public void PerformOperation (CmdImageInstructionSet instructionSet)
		{
			if (instructionSet == null)
				return;

			foreach (var inst in instructionSet.LoadImageData)
			{
				if (inst.Target == MgImageType.TYPE_1D)
				{
					if (inst.PixelFormat == (PixelFormat)All.CompressedTextureFormats)
					{
						GL.Ext.CompressedTextureSubImage1D (
							inst.TextureId,
							TextureTarget.Texture1D,
							inst.Level,
							inst.Slice,
							inst.Width,
							(PixelFormat)inst.InternalFormat,
							inst.Size,
							inst.Data
						);
					} 
					else
					{
						GL.Ext.TextureSubImage1D (
							inst.TextureId,
							TextureTarget.Texture1D,
							inst.Level,
							0,
							inst.Width,
							inst.PixelFormat,
							inst.PixelType,
							inst.Data
						);
					}
				} 
				else if (inst.Target == MgImageType.TYPE_2D)
				{
					if (inst.PixelFormat == (PixelFormat)All.CompressedTextureFormats)
					{
						GL.Ext.CompressedTextureSubImage2D (
							inst.TextureId,
							TextureTarget.Texture1D,
							inst.Level,
							0,
							inst.Slice,
							inst.Width,
							inst.Height,
							(PixelFormat)inst.InternalFormat,
							inst.Size,
							inst.Data
						);
					} 
					else
					{
						GL.Ext.TextureSubImage2D (
							inst.TextureId,
							TextureTarget.Texture2D,
							inst.Level,
							0,
							inst.Slice,
							inst.Width,
							inst.Height,
							inst.PixelFormat,
							inst.PixelType,
							inst.Data
						);
					}
				}
				else if (inst.Target == MgImageType.TYPE_3D)
				{
					if (inst.PixelFormat == (PixelFormat)All.CompressedTextureFormats)
					{
						GL.Ext.CompressedTextureSubImage3D (
							inst.TextureId,
							TextureTarget.Texture1D,
							inst.Level,
							0,
							0,
							inst.Slice,
							inst.Width,
							inst.Height,
							inst.Depth,
							(PixelFormat)inst.InternalFormat,
							inst.Size,
							inst.Data
						);
					} else
					{
						GL.Ext.TextureSubImage3D (
							inst.TextureId,
							TextureTarget.Texture3D,
							inst.Level,
							0,
							0,
							inst.Slice,
							inst.Width,
							inst.Height,
							inst.Depth,
							inst.PixelFormat,
							inst.PixelType,
							inst.Data
						);
					}
				}
			}
		}

		#endregion
	}
}

