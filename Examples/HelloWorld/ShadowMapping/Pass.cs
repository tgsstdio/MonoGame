using System;
using MonoGame.Shaders;

namespace ShadowMapping
{
	public class Pass
	{	
		public InstanceIdentifier Id { get; private set; }
		public IRenderTarget Target { get; private set; }
		public IPivot Pivot { get; private set; }
		public ShaderProgram Program { get; private set; }

		public Pass (Func<InstanceIdentifier> identityGen, IRenderTarget target, IPivot lamp, ShaderProgram depthShader)
		{
			Id = identityGen ();
			Target = target;
			Pivot = lamp;
			Program = depthShader;
		}
	}

}

