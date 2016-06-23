using System;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class IndexTypeValidation
	{
		[TestCase]
		public void NoIndexBufferPassedIn()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var actual = composer.ExtractIndexType (null, 0);
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void BufferInButNotIndexedDrawAnd32Bit()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT32,
			};

			var actual = composer.ExtractIndexType (indexBuffer, 0);
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16Bit()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT16,
			};

			var actual = composer.ExtractIndexType (indexBuffer, GLCommandBufferFlagBits.UseIndexBuffer);
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16BitIndexedIndirectDraw()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT16,
			};

			var actual = composer.ExtractIndexType (indexBuffer, GLCommandBufferFlagBits.CmdDrawIndexedIndirect);
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16BitIndexedDraw()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT16,
			};

			var actual = composer.ExtractIndexType (indexBuffer, GLCommandBufferFlagBits.CmdDrawIndexed);
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is32BitIndexedIndirectDraw()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT32,
			};

			var actual = composer.ExtractIndexType (indexBuffer, GLCommandBufferFlagBits.CmdDrawIndexedIndirect);
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is32BitIndexedDraw()
		{	
			var factory = new MockVertexBufferFactory ();
			var composer = new ItemComposer (factory);

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT32,
			};

			var actual = composer.ExtractIndexType (indexBuffer, GLCommandBufferFlagBits.CmdDrawIndexed);
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}
	}
}

