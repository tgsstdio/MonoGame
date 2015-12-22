// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IShaderPlatform
	{
		void GraphicsDeviceResetting ();

		bool GenerateHashKey (byte[] shaderBytecode, out int hashKey);

		void Construct (BinaryReader reader, bool isVertexShader, byte[] shaderBytecode);
	}    
}

