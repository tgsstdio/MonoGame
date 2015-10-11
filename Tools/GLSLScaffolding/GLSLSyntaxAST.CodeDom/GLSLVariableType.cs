using System;

namespace GLSLSyntaxAST.CodeDom
{
	public class GLSLVariableType
	{
		public Type MatchingType { get; set; }
		public string TypeNameKey { get; set;}
		public Type ComponentType { get; set; }
		public int NoOfComponents { get; set; }
	}
}

