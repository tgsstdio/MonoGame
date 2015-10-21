using System;

namespace BirdNest.MonoGame
{
	public class DefaultDrawElementsCommandFilter : IDrawElementsCommandFilter
	{
		private DrawElementsIndirectCommand[] mCommands;
		public DefaultDrawElementsCommandFilter (DrawElementsIndirectCommand[] commands)
		{
			mCommands = commands;
		}

		#region IDrawElementsCommandFilter implementation

		public DrawElementsIndirectCommand[] ToArray ()
		{
			return mCommands;
		}

		#endregion

		~DefaultDrawElementsCommandFilter()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				mCommands = null;
			}
			mDisposed = true;
		}
	}
}

