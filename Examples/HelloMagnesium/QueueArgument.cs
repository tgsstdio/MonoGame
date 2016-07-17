using Magnesium;
using Microsoft.Xna.Framework;

namespace HelloMagnesium
{
	public class QueueArgument
	{
		public IMgQueue Queue { get; set; }
		public uint FrameIndex { get; set; }
		public GameTime GameTime { get; set; }
	}
}

