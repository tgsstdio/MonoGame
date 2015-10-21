using System;
using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	public class PassOptimizer : IPassOptimizer
	{
		private readonly IShaderProgramCache mCache;
		private IPassOptimizerLogger mLogger;
		public PassOptimizer (IShaderProgramCache cache, IPassOptimizerLogger logger, IFrameDuplicator frameSet)
		{
			mCache = cache;
			mLogger = logger;
		}

		#region IPassOptimizer implementation

		IShaderProgram DeriveShader (RenderPass pass)
		{
			// derive actual shader program
			IShaderProgram program;
			if (mCache.TryGetValue (pass.Program, out program))
			{
				return program;
			}
			else
			{
				mLogger.Log ("Shader not registered");
				return null;
			}

		}

		public void Optimize (IList<RenderPass> passes)
		{
			// FOREACH pass in passes
			foreach(var pass in passes)
			{
				IShaderProgram program = DeriveShader (pass);

				// work out all (render targets, layer type, index) => frames

				// setup cameras and lights

				// collate mesh to common group

				// generate combined vbo(s) from groups 

				// generate marker for reuse

				// build draws command 

				// add filters

				// setup ssbo buffer blocks to binders
			}



			// sort passes based on selected order
			// A) render target
			// B) shader program
			// C) uniforms 
			// D) vbo
			// E) command filter
			// F) draw state
		}

		#endregion
	}
}

