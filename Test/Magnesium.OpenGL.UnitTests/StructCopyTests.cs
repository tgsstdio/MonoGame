using System;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class StructCopyTests
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct Point
		{
			public int X { get; set;}
			public float Y { get; set;}
		}

		[TestCase]
		public void Test1()
		{
			var p = new [] {
				new Point { X = 1, Y = 5.0f },
				new Point { X = 2, Y = 15.0f },
			};

			// Initialize unmanged memory to hold the struct.
			int stride = Marshal.SizeOf(typeof(Point));
			IntPtr pnt = Marshal.AllocHGlobal(stride * p.Length);

			try
			{

				// Copy the struct to unmanaged memory.
				{
					int offset = 0;
					for(int i = 0; i < p.Length; ++i)
					{
						IntPtr dest = IntPtr.Add(pnt, offset);
						Marshal.StructureToPtr(p[i], dest, false);
						offset += stride;
					}
				}

				// Create another point.

				// Set this Point to the value of the 
				// Point in unmanaged memory. 
				var actual = new Point[p.Length];
				{
					int offset = 0;
					for(int i = 0; i < p.Length; ++i)
					{
						IntPtr dest = IntPtr.Add(pnt,  offset);
						actual[i] = (Point)Marshal.PtrToStructure(dest, typeof(Point));
						offset += stride;
					}
				}

				for(int i = 0; i < p.Length; ++i)
				{
					Assert.AreEqual(p[i].X, actual[i].X, string.Format("{0}.X", i));
					Assert.AreEqual(p[i].Y, actual[i].Y, string.Format("{0}.Y", i));
				}
			}
			finally
			{
				// Free the unmanaged memory.
				Marshal.FreeHGlobal(pnt);
			}
		}
	}
}

