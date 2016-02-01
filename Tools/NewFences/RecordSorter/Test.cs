using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoGame.Graphics;

namespace RecordSorter
{
	[TestFixture ()]
	public class Test
	{


		public class Bucket : IEquatable<Bucket>
		{
			public byte Slot {get;set;}
			public int Target { get; set; }
			public int Program { get; set; }
			public int Buffer { get; set; }
			public ushort BufferMask { get; set; }

			#region IEquatable implementation

			public bool Equals (Bucket other)
			{
				return Slot == other.Slot
					&& Target == other.Target
					&& Program == other.Program
					&& Buffer == other.Buffer
					&& BufferMask == other.BufferMask;
			}

			public override bool Equals (object obj)
			{
				var other = obj as Bucket;
				return other != null 
						&& Slot == other.Slot	
						&& Target == other.Target
						&& Program == other.Program
						&& Buffer == other.Buffer
						&& BufferMask == other.BufferMask;
			}

			public override int GetHashCode()
			{
				unchecked
				{
					int hash = 17;
					// Maybe nullity checks, if these are objects not primitives!
					hash = hash * 23 + Slot.GetHashCode();
					hash = hash * 23 + Target.GetHashCode();
					hash = hash * 23 + Program.GetHashCode();
					hash = hash * 23 + Buffer.GetHashCode();
					hash = hash * 23 + BufferMask.GetHashCode();
					return hash;
				}
			}
			#endregion
		}

		[Test ()]
		public void ModeLessThan ()
		{
			var a = new Record{ Mode = DrawMode.Lines, UniformsIndex = 0 };
			var b = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 0 };

			var result = a.CompareTo (b);
			Assert.AreEqual (-1, result);
		}

		[Test ()]
		public void ModeGreaterThan ()
		{
			var a = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 0 };
			var b = new Record{ Mode = DrawMode.Lines, UniformsIndex = 0 };

			var result = a.CompareTo (b);
			Assert.AreEqual (1, result);
		}

		[Test ()]
		public void UniformsLessThan ()
		{
			var a = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 0 };
			var b = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 1 };

			var result = a.CompareTo (b);
			Assert.AreEqual (-1, result);
		}

		[Test ()]
		public void UniformsGreaterThan ()
		{
			var a = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 2 };
			var b = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 1 };

			var result = a.CompareTo (b);
			Assert.AreEqual (1, result);
		}

		[Test ()]
		public void EqualsTest ()
		{
			var a = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 0, MarkerIndex = 1, Flags = DrawItemBitFlags.DepthBufferEnabled };
			var b = new Record{ Mode = DrawMode.Polygon, UniformsIndex = 0, MarkerIndex = 1, Flags = DrawItemBitFlags.DepthBufferEnabled  };

			var result = a.CompareTo (b);
			Assert.AreEqual (0, result);
		}

		[Test ()]
		public void MinimumSize ()
		{
			var result = Marshal.SizeOf (typeof(DrawItem));

			bool lessThan64 = (result <= 64);

			Assert.IsTrue (lessThan64);
		}

		[Test ()]
		public void DrawItemToRecord ()
		{
			// where is the pass value
			// ANSWER : PASSES WOULD HOLD THE DRAW ITEMS 

			// INPUT : passes and draw items.
			// OUTPUT : bucket, groups & records
		}
	}
}

