using System;
using MonoGame.Graphics;

namespace NewFences
{
	public class MeshBuffer : IMeshBuffer
	{
		public void UpdateAll (int index)
		{
			throw new NotImplementedException ();
		}

		public IBufferSyncObject Fence {
			get;
			set;
		}

		public int BufferId {
			get {
				throw new NotImplementedException ();
			}
		}

		public ISyncObject[] Fences {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public float Factor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
	}
}
