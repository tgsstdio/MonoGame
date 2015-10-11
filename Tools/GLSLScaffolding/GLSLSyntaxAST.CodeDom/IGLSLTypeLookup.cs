using System;

namespace GLSLSyntaxAST.CodeDom
{
	public interface IGLSLTypeLookup
	{
		void Initialize();
		GLSLVariableType FindClosestType (string typeName);
	}
}

