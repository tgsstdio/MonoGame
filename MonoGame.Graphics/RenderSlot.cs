
namespace MonoGame.Graphics
{
	public class RenderSlot
	{
		public byte SlotId { get; set; }
		public IConstantBufferCollection Buffers {get;set;}
		public IEffectCache Effects {get;set;}
	}
}

