using System.Collections.Generic;

namespace BirdNest.MonoGame
{
	public class ShaderProgramGroup
	{
		private readonly ShaderProgramRoot mRoot;
		public List<RenderUnit> Passes { get; private set; }
		public ShaderProgramGroup (ShaderProgramRoot root)
		{
			mRoot = root;
			Passes = new List<RenderUnit> ();
		}

		public class Uniforms
		{

		}

		public void Clear ()
		{

		}
	}
}

