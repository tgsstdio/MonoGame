using System;

namespace StrawHat
{
	public class StateGroupWriter : IStateGroupWriter
	{
		private StateGroup mInstance;
		public StateGroupWriter ()
		{
			mInstance = null;
		}

		#region IStateGroupWriter implementation

		public IStateGroupWriter Begin ()
		{
			mInstance = new StateGroup ();
			return this;
		}

		public IStateGroupWriter SetPassNumber(byte num)
		{
			mInstance.PassNumber = num;
			return this;
		}

		public IStateGroupWriter SetBlend (BlendState state)
		{
			mInstance.BlendValues = state;
			return this;
		}

		public IStateGroupWriter SetTechnique (ShaderTechnique tech)
		{
			mInstance.Technique = tech;
			return this;
		}

		public IStateGroupWriter SetRasterizer (RasterizerState state)
		{
			mInstance.RasterizerValues = state;
			return this;
		}

		public IStateGroupWriter SetShaderProgram (ShaderProgram prog)
		{
			mInstance.Program = prog;
			return this;
		}

		public IStateGroupWriter SetDepthStencilState (DepthStencilState state)
		{
			mInstance.DepthStencilValues = state;
			return this;
		}

		public StateGroup End ()
		{
			var result = mInstance;
			mInstance = null;
			return result;
		}

		#endregion
	}
}

