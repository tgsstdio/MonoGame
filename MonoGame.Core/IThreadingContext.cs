using System;

namespace Microsoft.Xna.Framework
{
	public interface IThreadingContext
	{
		bool IsOnUIThread();

		/// <summary>
		/// Throws an exception if the code is not currently running on the UI thread.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the code is not currently running on the UI thread.</exception>
		void EnsureUIThread();

		/// <summary>
		/// Runs the given action on the UI thread and blocks the current thread while the action is running.
		/// If the current thread is the UI thread, the action will run immediately.
		/// </summary>
		/// <param name="action">The action to be run on the UI thread</param>
		void BlockOnUIThread(Action action);
	}
}

