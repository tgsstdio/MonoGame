namespace MonoGame.Graphics
{
	public class DefaultRenderSlotCache : IRenderSlotCache
	{
		private readonly RenderSlot[] mSlots;
		public DefaultRenderSlotCache (RenderSlot[] slots)
		{
			mSlots = slots;
		}

		#region IRenderSlotCache implementation

		public bool TryGetValue (byte index, out RenderSlot result)
		{
			if (index >= mSlots.Length)
			{
				result = null;
				return false;
			}

			result = mSlots [index];
			return true;
		}

		#endregion
	}
}

