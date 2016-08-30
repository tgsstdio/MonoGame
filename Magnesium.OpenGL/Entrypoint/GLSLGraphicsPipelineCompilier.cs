using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Magnesium.OpenGL
{
	public class GLSLGraphicsPipelineCompilier : IGLGraphicsPipelineCompiler
	{
		private IGLShaderModuleEntrypoint mEntrypoint;
		public GLSLGraphicsPipelineCompilier(IGLShaderModuleEntrypoint entrypoint)
		{
			mEntrypoint = entrypoint;
		}

		public int Compile(MgGraphicsPipelineCreateInfo info)
		{		
			var modules = new List<int>();
			foreach (var stage in info.Stages)
			{
				//var shaderType = ShaderType.VertexShader;
				//if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				//{
				//	shaderType = ShaderType.FragmentShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				//{
				//	shaderType = ShaderType.VertexShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
				//{
				//	shaderType = ShaderType.ComputeShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
				//{
				//	shaderType = ShaderType.GeometryShader;
				//}
				var module = (GLShaderModule)stage.Module;
				Debug.Assert(module != null);
				if (module.ShaderId.HasValue)
				{
					modules.Add(module.ShaderId.Value);
				}
				else
				{
					using (var ms = new MemoryStream())
					{
						module.Info.Code.CopyTo(ms, (int)module.Info.CodeSize.ToUInt32());
						ms.Seek(0, SeekOrigin.Begin);
						// FIXME : Encoding type 
						using (var sr = new StreamReader(ms))
						{
							string fileContents = sr.ReadToEnd();
							module.ShaderId = CompileShader(stage.Stage, fileContents, string.Empty);
							modules.Add(module.ShaderId.Value);
						}
					}
				}
			}
			return GLSLTextShader.LinkShaders(modules.ToArray());
		}

		internal int CompileShader(MgShaderStageFlagBits stage, string fileContents, string shaderPrefix)
		{
			int retVal = mEntrypoint.CreateShaderModule(stage);
			// GL.CreateShader(type);
			//string includePath = ".";

			// GLSL has this annoying feature that the #version directive must appear first. But we 
			// want to inject some #define shenanigans into the shader. 
			// So to do that, we need to split for the part of the shader up to the end of the #version line,
			// and everything after that. We can then inject our defines right there.
			var strTuple = VersionSplit(fileContents);
			string versionStr = strTuple.Item1;
			string shaderContents = strTuple.Item2;

			var builder = new StringBuilder();
			builder.AppendLine(versionStr);
			builder.AppendLine(shaderPrefix);
			builder.Append(shaderContents);

			mEntrypoint.CompileShaderModule(retVal, builder.ToString());

			//GL.ShaderSource(retVal, builder.ToString());
			//GL.CompileShader(retVal);

			int compileStatus = 0;
			GL.GetShader(retVal, ShaderParameter.CompileStatus, out compileStatus);

			int glinfoLogLength = 0;
			GL.GetShader(retVal, ShaderParameter.InfoLogLength, out glinfoLogLength);
			if (glinfoLogLength > 1)
			{
				string buffer = GL.GetShaderInfoLog(retVal);
				if (compileStatus != (int)All.True)
				{
					throw new Exception("Shader Compilation failed for shader with the following errors: " + buffer);
				}
				//				else {
				//					Console.WriteLine("Shader Compilation succeeded for shader '{0}', with the following log:", _shaderFilename);
				//				}
				//
				//				Console.WriteLine(buffer);
			}

			if (compileStatus != (int)All.True)
			{
				GL.DeleteShader(retVal);
				retVal = 0;
			}

			return retVal;
		}

		private static Tuple<string, string> VersionSplit(string srcString)
		{
			int length = srcString.Length;
			int substrStartPos = 0;
			int eolPos = 0;
			for (eolPos = substrStartPos; eolPos < length; ++eolPos)
			{
				if (srcString[eolPos] != '\n')
				{
					continue;
				}

				if (MatchVersionLine(srcString, substrStartPos, eolPos + 1))
				{
					return DivideString(srcString, eolPos + 1);
				}

				substrStartPos = eolPos + 1;
			}

			// Could be on the last line (not really--the shader will be invalid--but we'll do it anyways)
			if (MatchVersionLine(srcString, substrStartPos, length))
			{
				return DivideString(srcString, eolPos + 1);
			}

			return new Tuple<string, string>("", srcString);
		}
	}
}

