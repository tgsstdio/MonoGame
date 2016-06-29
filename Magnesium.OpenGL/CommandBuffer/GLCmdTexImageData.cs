﻿using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL
{
	public class GLCmdTexImageData
	{
		// GLCopyTextureSubData
		public int TextureId {get; set;}
		public MgImageType Target { get; set; }
		public int Level { get; set; }
		public int Slice {	get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Depth { get; set; }
		public MgFormat Format { get; set; }

		public PixelInternalFormat InternalFormat {
			get;
			set;
		}

		public PixelFormat PixelFormat {
			get;
			set;
		}

		public PixelType PixelType {
			get;
			set;
		}

		public int Size {
			get;
			set;
		}

		public IntPtr Data { get; set;} 
	}
}

