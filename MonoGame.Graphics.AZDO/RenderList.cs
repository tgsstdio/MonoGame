using System.Collections.Generic;

namespace MonoGame.Graphics.AZDO
{
	public class RenderList : IRenderList
	{
		#region IRenderList implementation
		private readonly List<DrawItem> mItems;
		public RenderList ()
		{
			mItems = new List<DrawItem> ();
		}

		public bool Push (DrawItem[] items, out DrawItemOffset output)
		{
			output = new DrawItemOffset{Offset = mItems.Count, Count = items.Length};
			foreach (var i in items)
			{
				mItems.Add (i);
			}
			return true;
		}

		public DrawItem[] ToArray ()
		{
			return mItems.ToArray ();
		}

		#endregion
	}
}

