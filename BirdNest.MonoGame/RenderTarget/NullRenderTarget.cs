using System;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class NullRenderTarget : IRenderTarget
	{
		public NullRenderTarget (Func<InstanceIdentifier> nextId)
		{
			Id = nextId ();
		}

		#region IRenderTarget implementation

		public InstanceIdentifier Id {
			get;
			private set;
		}

		public void Switch ()
		{

		}

		#endregion

		public void Dispose ()
		{
			// empty
		}


	}
}

