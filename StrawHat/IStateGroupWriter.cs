using System;

namespace StrawHat
{
	public interface IStateGroupWriter
	{
		IStateGroupWriter Begin();
		IStateGroupWriter SetPassNumber(byte num);
		IStateGroupWriter SetBlend(BlendState state);
		IStateGroupWriter SetTechnique(ShaderTechnique tech);
		IStateGroupWriter SetShaderProgram(ShaderProgram prog);
		IStateGroupWriter SetRasterizer(RasterizerState state);
		IStateGroupWriter SetDepthStencilState(DepthStencilState state);
		StateGroup End();
	}

}

