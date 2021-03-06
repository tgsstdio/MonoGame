﻿using MonoGame.Content;
using OpenTK.Graphics.OpenGL;
using MonoGame.Content.Blocks;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class GLSLBinaryProgramWriter : IBinaryShaderProgramWriter
	{
		private readonly IFileSystem mFileSystem;
		private readonly IShaderInfoLookup mLookup;
		public GLSLBinaryProgramWriter (IFileSystem fs, IShaderInfoLookup lookup)
		{
			mFileSystem = fs;
			mLookup = lookup;
		}

		#region IBinaryShaderProgramWriter implementation

		private byte[] mBuffer;
		private int mBufferSize;
		public void Initialise (int bufSize)
		{
			mBufferSize = bufSize;
			mBuffer = new byte[mBufferSize];
		}

		public void WriteBinary (GLShaderProgram program)
		{
			int length;
			BinaryFormat format;
			GL.GetProgramBinary<byte>(program.ProgramID, mBufferSize, out length, out format, mBuffer);

			ShaderInfo found;
			if (mLookup.TryGetValue(program.Identifier, out found))
			{
				string programFilePath = program.Identifier.AssetId + "_glsl.bin";
				using (var output = mFileSystem.OpenStream (program.Block, programFilePath))
				{
					output.Write(mBuffer, 0, length);
				}
			}
		}

		#endregion
	}
}

