using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCmdBufferStore<TData> where TData : class 
	{
		public GLCmdBufferStore ()
		{
			Items = new List<TData>();
		}

		public List<TData> Items { get; private set;	}

		public void Add(TData item)
		{
			Items.Add (item);
		}

		public int LastIndex()
		{
			if (Items.Count == 0)
			{
				return int.MinValue;
			}
			else
			{
				return Items.Count - 1;
			}
		}

		public bool LastValue(out TData item)
		{
			if (Items.Count == 0)
			{
				item = null;
				return false;
			}
			else
			{
				item = Items[Items.Count - 1];
				return true;
			}
		}

		public void Clear()
		{
			Items.Clear ();
		}
	}
}

