using System;

namespace GLSLSyntaxAST.CodeDom
{
	public class StructMember
	{
		public string TypeString;
		public GLSLVariableType ClosestType;
		public string Name;
		public ArraySpecification ArrayDetails;
	}
}

