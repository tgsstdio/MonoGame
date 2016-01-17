
namespace MonoGame.Graphics
{
	public interface IRenderSlotCache
	{
		bool TryGetValue(byte index, out RenderSlot result);
	}
}

