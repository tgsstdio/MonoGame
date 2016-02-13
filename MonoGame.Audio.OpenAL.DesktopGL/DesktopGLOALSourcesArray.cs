using OpenTK.Audio.OpenAL;
using System.Collections.Generic;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLOALSourcesArray : IOALSourceArray
	{
		private const int MAX_NUMBER_OF_SOURCES = 32;		
		private int[] allSourcesArray;
		private readonly SortedSet<int> mAvailable;
		public DesktopGLOALSourcesArray ()
		{
			allSourcesArray = new int[MAX_NUMBER_OF_SOURCES];
			AL.GenSources(allSourcesArray);

			mAvailable = new SortedSet<int>(allSourcesArray);
		}

		#region IOALSourceArray implementation

		public bool IsEmpty ()
		{
			return mAvailable.Count == 0;
		}

		public void Remove (int sourceId)
		{
			mAvailable.Remove (sourceId);
		}

		public void Add (int sourceId)
		{
			mAvailable.Add (sourceId);
		}

		public int First ()
		{
			return mAvailable.Min;
		}
		#endregion
	}
}

