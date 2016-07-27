using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IBlendCapabilities
	{
		void EnableLogicOp (bool logicOpEnable);
		void LogicOp (MgLogicOp logicOp);
	
		bool IsEnabled(uint index);

		GLQueueRendererColorBlendState Initialize(uint noOfAttachments);

		void EnableBlending (uint index, bool value);

		void SetColorMask (uint index, MgColorComponentFlagBits colorMask);

		void ApplyBlendSeparateFunction (
			uint index,
			MgBlendFactor colorSource,
			MgBlendFactor colorDest,
			MgBlendFactor alphaSource,
			MgBlendFactor alphaDest);
	}
}

