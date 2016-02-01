
namespace NewFences
{
	public class DefaultFenceIndexVariable : IFenceIndexVariable
	{
		public int IndexMaximum {
			get;
			set;
		}

		public DefaultFenceIndexVariable (int max)
		{
			Index = 0;
			IndexMaximum = max;
		}

		#region IFenceIndexer implementation

		public void Increment ()
		{
			Index = (Index + 1) % IndexMaximum;
		}

		public int Index {
			get;
			private set;
		}

		#endregion
	}
}

