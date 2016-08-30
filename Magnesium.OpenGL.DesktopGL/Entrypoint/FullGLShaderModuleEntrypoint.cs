using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLShaderModuleEntrypoint : IGLShaderModuleEntrypoint
	{
		#region IGLShaderModuleEntrypoint implementation

		IGLErrorHandler mErrHandler;

		public FullGLShaderModuleEntrypoint (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		public int CompileProgram (MgGraphicsPipelineCreateInfo info)
		{
			var modules = new List<int> ();
			foreach (var stage in info.Stages)
			{
				var shaderType = ShaderType.VertexShader;
				if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				{
					shaderType = ShaderType.FragmentShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				{
					shaderType = ShaderType.VertexShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
				{
					shaderType = ShaderType.ComputeShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
				{
					shaderType = ShaderType.GeometryShader;
				}
				var module = (GLShaderModule) stage.Module;
				Debug.Assert(module != null);
				if (module.ShaderId.HasValue)
				{
					modules.Add (module.ShaderId.Value);
				}
				else
				{
					using (var ms = new MemoryStream ())
					{
						module.Info.Code.CopyTo (ms, (int)module.Info.CodeSize.ToUInt32 ());
						ms.Seek (0, SeekOrigin.Begin);
						// FIXME : Encoding type 
						using (var sr = new StreamReader (ms))
						{
							string fileContents = sr.ReadToEnd ();
							module.ShaderId = GLSLTextShader.CompileShader (shaderType, fileContents, string.Empty);
							modules.Add (module.ShaderId.Value);
						}
					}
				}
			}
			return GLSLTextShader.LinkShaders (modules.ToArray ());
		}

		public bool CheckUniformLocation (int programId, int location)
		{
			int locationQuery;
			GL.Ext.GetUniform(programId, location, out locationQuery);
			mErrHandler.LogGLError ("FullGLShaderModuleEntrypoint.CheckUniformLocation");
			return (locationQuery != -1);
		}

		public int GetActiveUniforms (int programId)
		{
			int noOfActiveUniforms;
			GL.GetProgram (programId, GetProgramParameterName.ActiveUniforms, out noOfActiveUniforms);
			mErrHandler.LogGLError ("FullGLShaderModuleEntrypoint.GetActiveUniforms");
			return noOfActiveUniforms;
		}

		public void DeleteShaderModule (int module)
		{
			GL.DeleteShader(module);
		}

		#endregion
	}
}

