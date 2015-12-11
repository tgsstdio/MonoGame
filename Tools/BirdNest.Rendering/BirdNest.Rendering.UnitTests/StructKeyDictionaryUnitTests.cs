using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	[TestFixture]
	public class StructKeyDictionaryUnitTests
	{
		/// <summary>
		/// Structs seem to work but don't rely on floating point members to be 
		/// corrected; probably needs custom values for code.
		/// </summary>
		public struct SomeKey
		{
			public ulong Key0 { get; set;}
			public float Key1 { get; set;}
		}

		[TestCase]
		[ExpectedException(typeof(ArgumentException))]
		public void DuplicateSameKey ()
		{
			const uint FIRST_KEY = 13;
			const float SECOND_KEY = 7.0f;
			var temp = new SomeKey{Key0 = FIRST_KEY, Key1 = SECOND_KEY};
			
			var dict = new Dictionary<SomeKey, int> ();
			dict.Add (temp, 0);
			dict.Add (temp, 1);
		}

		[TestCase]
		public void DuplicateFindKey ()
		{
			const uint FIRST_KEY = 13;
			const float SECOND_KEY = 7.0f;
			var temp = new SomeKey{Key0 = FIRST_KEY, Key1 = SECOND_KEY};

			var dict = new Dictionary<SomeKey, int> ();
			dict.Add (temp, 0);
			Assert.AreEqual (1, dict.Keys.Count);
			bool actual = dict.ContainsKey (temp);
			Assert.IsTrue (actual);
		}

		[TestCase]
		public void DuplicateFindAnotherKey ()
		{
			const uint FIRST_KEY = 13;
			const uint ALT_KEY = 23;
			const float SECOND_KEY = 7.0f;
			var insertedKey = new SomeKey{Key0 = FIRST_KEY, Key1 = SECOND_KEY};
			var searchKey = new SomeKey{Key0 = ALT_KEY, Key1 = SECOND_KEY};

			var dict = new Dictionary<SomeKey, int> ();
			dict.Add (insertedKey, 0);
			Assert.AreEqual (1, dict.Keys.Count);
			bool actual = dict.ContainsKey (searchKey);
			Assert.IsFalse(actual);
		}

		[TestCase]
		public void DuplicateFindSameValues ()
		{
			const uint FIRST_KEY = 13;
			const float SECOND_KEY = 7.0f;
			var insertedKey = new SomeKey{Key0 = FIRST_KEY, Key1 = SECOND_KEY};
			var searchKey = new SomeKey{Key0 = FIRST_KEY, Key1 = SECOND_KEY};

			var dict = new Dictionary<SomeKey, int> ();
			dict.Add (insertedKey, 0);
			Assert.AreEqual (1, dict.Keys.Count);
			bool actual = dict.ContainsKey (searchKey);
			Assert.IsTrue(actual);
		}
	}
}

