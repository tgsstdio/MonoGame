using System;
using System.Collections.Generic;

namespace MonoGame.Graphics
{
	public class GPUResourceList<TClass, TStruct> : IResourceList, IDisposable
		where TClass : class
		where TStruct : struct
	{
		private int mBufferID;
		private DrawPrimitive mPrimitive;

		public int Index { get; private set; }

		public GPUResourceList(			
			int index,
			DrawPrimitive primitiveType)
		{
			Index = index;
			mPrimitive = primitiveType;
		}

		public void Copy(IEnumerable<TClass> items)
		{

		}

		~GPUResourceList()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources()
		{

		}

		void ReleaseManagedResources()
		{

		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}
	}
}

