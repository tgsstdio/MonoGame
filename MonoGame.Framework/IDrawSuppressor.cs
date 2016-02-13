using System;

namespace Microsoft.Xna.Framework
{
	public interface IDrawSuppressor
	{
		bool SuppressDraw { get; set; }
		void AddBeforeExit(Action doTask);
		void Cleanup();
	}
}

