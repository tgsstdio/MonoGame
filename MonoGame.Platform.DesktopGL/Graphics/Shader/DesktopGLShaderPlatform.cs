using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using MonoGame.Platform.DesktopGL.Graphics;

#if MONOMAC
using MonoMac.OpenGL;
#elif DESKTOPGL
using OpenTK.Graphics.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
#endif

namespace MonoGame.Platform.DesktopGL.Graphics
{
	public class DesktopGLShaderPlatform : IShaderPlatform
	{
		#region IShaderPlatform implementation

		public void GraphicsDeviceResetting()
		{
			if (_shaderHandle != -1)
			{
				if (GL.IsShader(_shaderHandle))
				{
					GL.DeleteShader(_shaderHandle);
					GraphicsExtensions.CheckGLError();
				}
				_shaderHandle = -1;
			}
		}

		// The shader handle.
		private int _shaderHandle = -1;

		// We keep this around for recompiling on context lost and debugging.
		private string _glslCode;

		private struct Attribute
		{
			public VertexElementUsage usage;
			public int index;
			public string name;
			public int location;
		}

		private Attribute[] _attributes;

		public bool GenerateHashKey (byte[] shaderBytecode, out int hashKey)
		{
			hashKey = MonoGame.Utilities.Hash.ComputeHash(shaderBytecode);
			return true;
		}

		public void Construct(BinaryReader reader, bool isVertexShader, byte[] shaderBytecode)
		{
			_glslCode = System.Text.Encoding.ASCII.GetString(shaderBytecode);

			var attributeCount = (int)reader.ReadByte();
			_attributes = new Attribute[attributeCount];
			for (var a = 0; a < attributeCount; a++)
			{
				_attributes[a].name = reader.ReadString();
				_attributes[a].usage = (VertexElementUsage)reader.ReadByte();
				_attributes[a].index = reader.ReadByte();
				reader.ReadInt16(); //format, unused
			}
		}

		#endregion


	}
}

