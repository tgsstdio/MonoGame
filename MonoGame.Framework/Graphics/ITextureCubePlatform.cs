// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using MonoGame.Graphics;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface ITextureCubePlatform
	{
		void SetData<T> (CubeMapFace face, int level, IntPtr dataPtr, int xOffset, int yOffset, int width, int height);

		void GetData<T> (CubeMapFace cubeMapFace, T[] data);

		void Construct (IWeakReferenceCollection owner, int size, bool mipMap, SurfaceFormat format, bool renderTarget);
	}
}

