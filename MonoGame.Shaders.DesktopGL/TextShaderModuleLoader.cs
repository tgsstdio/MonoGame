using System;
using System.IO;
using Magnesium.OpenGL;
using Magnesium;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class TextShaderModuleLoader : IGLSLShaderModuleLoader
	{
		#region IGLSLShaderModuleLoader implementation

		public int Compile (MgShaderModuleCreateInfo info)
		{
			using (var sr = new StreamReader (info.Code))
			{			
				ShaderType type = null;

				string fileContents = sr.ReadToEnd ();
				return GLSLTextShaderManager.CompileShader(type, fileContents, string.Empty);
			}
		}

		#endregion
	}
}

