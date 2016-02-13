using System;

namespace Microsoft.Xna.Framework
{
	public class DrawSupressor : IDrawSuppressor
	{
		#region IDrawSuppression implementation

		public bool SuppressDraw {
			get;
			set;
		}

		#endregion

		private Action mDoCleanup {get;set;}
		public void AddBeforeExit (Action doTask)
		{
			mDoCleanup = doTask;
		}

		public void Cleanup ()
		{
			mDoCleanup ();
			SuppressDraw = true;
		}
	}
}

