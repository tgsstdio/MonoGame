using System;
using NUnit.Framework;
using GLSLSyntaxAST.CodeDom;

namespace GLSLSyntaxAST.UnitTests
{
	[TestFixture]
	public class CodeDomUniformFields
	{
		const string UNIFORMS_TEST_CASE = "uniform uint currentLight;";

		[Test ()]
		public void ExtractUniformFields ()
		{
			const int expected = 1;
			IGLSLTypeLookup lookup = new OpenTKTypeLookup ();
			lookup.Initialize ();
			IGLSLUniformExtractor test = new GLSLUniformExtractor (lookup);
			test.Initialize ();
			int actual = test.Extract (UNIFORMS_TEST_CASE);
			Assert.AreEqual (expected, actual);
			Assert.AreEqual (0, test.Blocks.Count);
			Assert.AreEqual (1, test.Uniforms.Count);
			Assert.AreEqual (0, test.Attributes.Count);
		}

		[Test()]
		public void ExpressUniformFields()
		{
			const string expected = "translation_unit\n"
				+ " external_declaration\n"
				+ "  declaration\n"
				+ "   single_declaration\n"
				+ "    fully_specified_type\n"
				+ "     type_qualifier\n"
				+ "      storage_qualifier\n"
				+ "       UNIFORM\n"
				+ "     UINT\n"
				+ "    IDENTIFIER\n";
			IGLSLTypeLookup lookup = new OpenTKTypeLookup ();
			lookup.Initialize ();
			IGLSLUniformExtractor test = new GLSLUniformExtractor (lookup);
			test.Initialize ();
			var actual = test.ExpressTree (UNIFORMS_TEST_CASE);
			Assert.AreEqual (expected, actual);
		}
	}
}

