using MonoGame.Content;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class GLShaderProgram
	{
		public class ShaderComponent
		{
			public AssetIdentifier AssetId { get; set; }
			public int ShaderId { get; set; }
		}

		public ShaderComponent[] Shaders { get; set; }

		public int ProgramID {
			get;
			set;
		}
	}
}

