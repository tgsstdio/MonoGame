using Magnesium;

namespace MonoGame.Textures.FreeImageNET
{
	public interface IMgTexture
	{
		void DestroyTexture(IMgDevice device, MgAllocationCallbacks allocator);
	}

}

