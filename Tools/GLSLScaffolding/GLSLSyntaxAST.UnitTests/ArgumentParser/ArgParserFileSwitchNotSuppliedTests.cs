using System;
using NUnit.Framework;
using GLSLSyntaxAST.CodeDom;

namespace GLSLSyntaxAST.UnitTests
{
	[TestFixture]
	public class ArgParserFileSwitchNotSuppliedTests
	{
		[TestCase]
		public void NoArguments()
		{
			var parser = new ArgumentParser ();
			var args = new string[]{ };
			Assert.Throws (typeof(ArgumentParser.FileSwitchNotSuppliedException), () => parser.Parse (args));
		}

		[TestCase]
		public void AssemblySwitchOnly()
		{
			var parser = new ArgumentParser ();
			var args = new string[]{ "-a", "Sample.dll"};
			Assert.Throws (typeof(ArgumentParser.FileSwitchNotSuppliedException), () => parser.Parse (args));
		}

		[TestCase]
		public void CodeSwitchOnly()
		{
			var parser = new ArgumentParser ();
			var args = new string[]{ "-c", "Sample.dll"};
			Assert.Throws (typeof(ArgumentParser.FileSwitchNotSuppliedException), () => parser.Parse (args));
		}
	}
}

