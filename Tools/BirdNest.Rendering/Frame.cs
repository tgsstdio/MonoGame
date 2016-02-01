using System.Collections.Generic;
using MonoGame.Shaders;

namespace BirdNest.Rendering
{
	public class Frame
	{
		public uint Order { get; set; }
		public IRenderTarget Target { get; set; }
		public IViewer Viewer { get; set; }
		public IShaderProgram Program {get;set;}	
		public List<OptimizedPass> Passes {get;set;}
	}
}

