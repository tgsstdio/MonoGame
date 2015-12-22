using System;

namespace StrawHat
{
	/// <summary>
	/// Graphic implementation of sync object.
	/// </summary>
	public class SyncObject
	{
		public int Index {get;set;}
		public IntPtr ObjectPtr {get;set;}
		public float Timeout {get;set;}
	}
}

