using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using BirdNest.MonoGame;
using System.Collections.Generic;
using GLSLSyntaxAST.CodeDom;

namespace GLSLSyntaxAST.CommandLine
{
	public class GLSLStructGenerator : IGLSLStructGenerator
	{
		private readonly IGLSLUniformExtractor mExtractor;
		public GLSLStructGenerator (IGLSLUniformExtractor extractor)
		{
			mExtractor = extractor;
		}

		#region IStructGenerator implementation

		public void Initialize ()
		{

		}

		private static CodeTypeDeclaration CreateClassType(string folderName)
		{
			var textures = new CodeTypeDeclaration (folderName);
			textures.IsClass = true;
			textures.TypeAttributes = TypeAttributes.Public;

			return textures;
		}

		private static void SetVersionNumber (CodeCompileUnit contentUnit, string value)
		{
			var attributeType = new CodeTypeReference (typeof(AssemblyVersionAttribute));
			var versionNumber = new CodeAttributeDeclaration (attributeType, new CodeAttributeArgument (new CodePrimitiveExpression (value)));
			contentUnit.AssemblyCustomAttributes.Add (versionNumber);
		}

		private static void AddStruct (CodeNamespace dest, StructInfo info)
		{
			if (info.StructType == GLSLStructType.Struct)
			{
				var structType = new CodeTypeDeclaration (info.Name);
				//structType.IsClass = false;
				structType.IsStruct = true;
				structType.TypeAttributes = TypeAttributes.Public | TypeAttributes.SequentialLayout | TypeAttributes.Sealed;
				var argument = new CodeAttributeArgument (new CodeFieldReferenceExpression (new CodeTypeReferenceExpression (typeof(LayoutKind).Name), "Sequential"));
				structType.CustomAttributes.Add (new CodeAttributeDeclaration (new CodeTypeReference ("StructLayout"), argument));

				dest.Types.Add (structType);

				foreach (var member in info.Members)
				{
					if (member.ClosestType != null)
					{
						var field1 = new CodeMemberField (member.ClosestType.MatchingType, member.Name);
						field1.Attributes = MemberAttributes.Public;
						structType.Members.Add (field1);
					}
					// single reference
					else if (member.ArrayDetails == null)
					{
						var arrayType = new CodeTypeReference (member.TypeString);
						var field1 = new CodeMemberField (arrayType, member.Name);
						field1.Attributes = MemberAttributes.Public;
						structType.Members.Add (field1);					
					}

				}
			}
//			var localVariable = "m" + alias;
//			var field1 = new CodeMemberField (typeof(string), localVariable);
//			folder.Members.Add (field1);
//
//			CodeMemberProperty property1 = new CodeMemberProperty ();
//			property1.Name = alias;
//			property1.Type = new CodeTypeReference ("System.String");
//			property1.Attributes = MemberAttributes.Public | MemberAttributes.Final;
//			property1.HasGet = true;
//			property1.HasSet = true;
//			property1.GetStatements.Add (new CodeMethodReturnStatement (new CodeFieldReferenceExpression (new CodeThisReferenceExpression (), localVariable)));
//			property1.SetStatements.Add (new CodeAssignStatement (new CodeFieldReferenceExpression (new CodeThisReferenceExpression (), localVariable), new CodePropertySetValueReferenceExpression ()));
//			folder.Members.Add (property1);
		}

		public void SaveAsAssembly (CodeDomProvider provider, GLSLAssembly assembly)
		{
			// Build the parameters for source compilation.
			var cp = new CompilerParameters();

			// Add an assembly reference.
			cp.ReferencedAssemblies.Add( "System.dll" );
			cp.ReferencedAssemblies.Add ("System.Runtime.InteropServices.dll");

			if (assembly.ReferencedAssemblies != null)
			{
				foreach (var assemblyName in assembly.ReferencedAssemblies)
				{
					cp.ReferencedAssemblies.Add( assemblyName );
				}
			}

			// Generate an executable instead of
			// a class library.
			cp.GenerateExecutable = false;

			// Set the assembly file name to generate.
			cp.OutputAssembly = System.IO.Path.Combine(assembly.Path,assembly.OutputAssembly);

			// Save the assembly as a physical file.
			cp.GenerateInMemory = false;

			var contentUnit = InitialiseCompileUnit (assembly);

			// Invoke compilation.
			CompilerResults cr = provider.CompileAssemblyFromDom(cp, contentUnit);

			if (cr.Errors.Count > 0)
			{
				Debug.WriteLine(string.Format("Source built into {0} unsuccessfully.", cr.PathToAssembly));				
				// Display compilation errors.
				foreach (CompilerError ce in cr.Errors)
				{
					Debug.WriteLine("  {0}", ce.ToString());		
				}
			}
			else
			{
				Debug.WriteLine(string.Format("Source built into {0} successfully.", cr.PathToAssembly));
			}
		}

		public void SaveAsCode (CodeDomProvider provider, GLSLAssembly assembly, IGLSLUniformExtractor extractor, CodeGeneratorOptions options)
		{
			string outputFile = Path.GetFileNameWithoutExtension (assembly.OutputAssembly) + ".cs";
			string absolutePath = Path.Combine(assembly.Path,outputFile);

			using (var writer = new StreamWriter(absolutePath, false))
			{
				var contentUnit = InitialiseCompileUnit (assembly);

				provider.GenerateCodeFromCompileUnit (contentUnit, writer, options);
			}
		}

		void DeclareStructs (CodeNamespace contentNs)
		{
			foreach (var block in mExtractor.Blocks)
			{
				AddStruct (contentNs, block);
			}
		}

		static CodeMemberMethod GenerateProtectedMethod (CodeTypeDeclaration dest, string methodName)
		{
			var method = new CodeMemberMethod ();
			method.Name = methodName;
			method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			dest.Members.Add (method);
			return method;
		}

		static void PopulateStatements (List<string> names, CodeMemberMethod dest, string methodName, params CodeExpression[] args)
		{
			foreach (var name in names)
			{
				dest.Statements.Add (new CodeMethodInvokeExpression (new CodeFieldReferenceExpression (new CodeThisReferenceExpression (), name), methodName, args));
			}
		}

		void GenerateVertexBuffer (CodeNamespace contentNs)
		{
			var vBuffer = CreateClassType ("VertexBuffer2");
			contentNs.Types.Add (vBuffer);

			vBuffer.BaseTypes.Add(new CodeTypeReference(typeof(VertexBuffer)));

			// constructor
			var constructor = new CodeConstructor();
			//constructor.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "constructor"));
			constructor.Attributes =MemberAttributes.Public | MemberAttributes.Final;
			//constructor.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "constructor"));
			vBuffer.Members.Add (constructor);

			var attributes = new List<string> ();
			foreach (var member in mExtractor.Attributes)
			{
				if (member.Direction.StartsWith ("in", StringComparison.Ordinal))
				{
					var field1 = new CodeMemberField ();
					field1.Name = member.Name;
					field1.Attributes = MemberAttributes.Public;

					var attributeType = typeof(int);
					if (member.ClosestType.ComponentType == typeof(float))
					{
						attributeType = typeof(FloatAttributeBinding);

					}
					else if (member.ClosestType.ComponentType == typeof(int) || member.ClosestType.ComponentType == typeof(uint))
					{
						attributeType = typeof(IntAttributeBinding);	
					}

					if (attributeType != typeof(int))
					{
						field1.Type = new CodeTypeReference(attributeType);	

						var objCreation = new CodeObjectCreateExpression (field1.Type);
						objCreation.Parameters.Add (new CodePrimitiveExpression (member.Name));					
						if (member.Layout.Location.HasValue)
						{
							objCreation.Parameters.Add (new CodePrimitiveExpression (member.Layout.Location.Value));
						}
						else
						{
							objCreation.Parameters.Add (new CodePrimitiveExpression (0));
						}

						if (member.ClosestType != null)
						{
							objCreation.Parameters.Add (new CodePrimitiveExpression (member.ClosestType.NoOfComponents));
						}
						else
						{
							objCreation.Parameters.Add (new CodePrimitiveExpression (0));
						}

						constructor.Statements.Add (
							new CodeAssignStatement (
									// left
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(),
										member.Name
									),
									// right
									objCreation
							)
						);
					}

					vBuffer.Members.Add (field1);
					attributes.Add (member.Name);
				}
			}

			// protected abstract void InitialiseBuffers ();
			var initMethod = GenerateProtectedMethod (vBuffer, "InitialiseBuffers");
			PopulateStatements (attributes, initMethod, "Initialise");

			// protected abstract void BindBuffersManually(int programID);
			var bindManally = GenerateProtectedMethod(vBuffer, "BindBuffersManually");
			bindManally.Parameters.Add (new CodeParameterDeclarationExpression(typeof(int), "programID"));
			PopulateStatements (attributes, bindManally, "BindManually", new CodeArgumentReferenceExpression("programID"));

			// protected abstract void ReleaseManagedResources ();
			var rmrMethod = GenerateProtectedMethod(vBuffer, "ReleaseManagedResources");
			PopulateStatements (attributes, rmrMethod, "Dispose");
		}

		void GenerateProgram (CodeNamespace contentNs)
		{
			var program = CreateClassType ("Program");

			program.BaseTypes.Add(new CodeTypeReference(typeof(RenderUnit)));

			foreach (var member in mExtractor.Uniforms)
			{
				var field1 = new CodeMemberField (typeof(int), member.Name);
				field1.Attributes = MemberAttributes.Public;
				program.Members.Add (field1);
			}

			var defaultConstructor = new CodeConstructor ();
			defaultConstructor.Attributes = MemberAttributes.Public;
			program.Members.Add (defaultConstructor);

			defaultConstructor.Parameters.Add (new CodeParameterDeclarationExpression(typeof(VertexBuffer), "v"));
			defaultConstructor.Parameters.Add (new CodeParameterDeclarationExpression(typeof(IDrawElementsCommandFilter), "f"));

			defaultConstructor.BaseConstructorArgs.Add (new CodeArgumentReferenceExpression ("v"));
			defaultConstructor.BaseConstructorArgs.Add (new CodeArgumentReferenceExpression ("f"));

			var fields = new List<string> ();
			foreach (var member in mExtractor.Blocks)
			{
				if (member.StructType == GLSLStructType.Buffer)
				{
					CodeTypeReference bufferType = null;
					// buffer array 
					if (member.Members.Count == 1 && member.Members[0].ArrayDetails.TypeInformation.StructType == GLSLStructType.Struct)
					{
						bufferType = new CodeTypeReference (typeof(ShaderStorageBuffer<>));
						bufferType.TypeArguments.Add(new CodeTypeReference (member.Members[0].TypeString));
					}
					bufferType = bufferType ?? new CodeTypeReference (typeof(int));
//
					var field1 = new CodeMemberField (bufferType, member.Name);
					field1.Attributes = MemberAttributes.Public;
//					if (member.Layout != null && member.Layout.Binding.HasValue)
//					{
//						field1.InitExpression = new CodePrimitiveExpression (member.Layout.Binding.Value);
//					}

					var parameter = new CodeParameterDeclarationExpression(bufferType, member.Name);
					defaultConstructor.Parameters.Add (parameter);
					defaultConstructor.Statements.Add (
						new CodeAssignStatement (
							// left
							new CodePropertyReferenceExpression (
								new CodeThisReferenceExpression (),
								member.Name
							),
							// right
							new CodeArgumentReferenceExpression (member.Name)
						)
					);

					program.Members.Add (field1);
					fields.Add (field1.Name);
				}
			}
			contentNs.Types.Add (program);

			// protected abstract void BindShaderStorage();
			var bind = GenerateProtectedMethod (program, "BindShaderStorage");
			PopulateStatements (fields, bind, "Bind");

			// protected abstract void UnbindShaderStorage();
			var unbind = GenerateProtectedMethod (program, "UnbindShaderStorage");
			PopulateStatements (fields, unbind, "Unbind");

			// protected abstract void ReleaseManagedResources();
			var rmr = GenerateProtectedMethod (program, "ReleaseManagedResources");
			PopulateStatements (fields, rmr, "Dispose");
			foreach (var name in fields)
			{
				rmr.Statements.Add(new CodeAssignStatement(
					// left
					new CodePropertyReferenceExpression (
						new CodeThisReferenceExpression (),
						name
					),
					new CodePrimitiveExpression (null))
				);
			}

			var uniforms = CreateClassType ("Uniforms");
			var inputBindings = CreateClassType ("InputBindings");
			var outputBindings = CreateClassType ("OutputBindings");
//			var bufferBindings = CreateClassType ("BufferBindings");

			var uniformConstructor = new CodeConstructor ();
			uniformConstructor.Attributes = MemberAttributes.Public;
			uniforms.Members.Add (uniformConstructor);
			if (uniforms.Members.Count > 0)
			{				
				contentNs.Types.Add (uniforms);
			}
			AddUniforms (uniforms, uniformConstructor);

			var initUniforms = new CodeMemberMethod();
			initUniforms.Name = "InitialiseUniforms";
			initUniforms.Attributes = MemberAttributes.Public | MemberAttributes.Override;
			initUniforms.Parameters.Add (new CodeParameterDeclarationExpression(typeof(int), "programID"));
			program.Members.Add (initUniforms);

			AddAttributes (inputBindings, outputBindings);
			//SetBufferBindings (bufferBindings);


			if (inputBindings.Members.Count > 0)
			{
				contentNs.Types.Add (inputBindings);
				var method = new CodeMemberMethod ();
				method.Attributes = MemberAttributes.Public;
				method.Name = "SetInputs";
				method.Parameters.Add (new CodeParameterDeclarationExpression (new CodeTypeReference ("InputBindings"), "bindings"));
				program.Members.Add (method);
			}
			if (outputBindings.Members.Count > 0)
			{
				var method = new CodeMemberMethod ();
				method.Name = "SetOutputs";
				method.Attributes = MemberAttributes.Public;
				method.Parameters.Add (new CodeParameterDeclarationExpression (new CodeTypeReference ("OutputBindings"), "bindings"));
				program.Members.Add (method);
				contentNs.Types.Add (outputBindings);
			}
//			if (bufferBindings.Members.Count > 0)
//			{
//				var method = new CodeMemberMethod ();
//				method.Name = "SetBuffers";
//				method.Attributes = MemberAttributes.Public;
//				method.Parameters.Add (new CodeParameterDeclarationExpression (new CodeTypeReference ("BufferBindings"), "bindings"));
//				program.Members.Add (method);
//				contentNs.Types.Add (bufferBindings);
//			}
		}

		public CodeCompileUnit InitialiseCompileUnit (GLSLAssembly assembly)
		{
			var contentUnit = new CodeCompileUnit ();
			var globalNs = new CodeNamespace ();
			globalNs.Comments.Add(new CodeCommentStatement("Namespace Comment"));
			contentUnit.Namespaces.Add (globalNs);
			contentUnit.ReferencedAssemblies.Add ("System.Runtime.InteropServices.dll");
			globalNs.Imports.Add(new CodeNamespaceImport("System.Runtime.InteropServices"));

			SetVersionNumber (contentUnit, assembly.Version);
			string nameSpace = assembly.Namespace;
			if (string.IsNullOrWhiteSpace (nameSpace))
			{
				nameSpace = System.IO.Path.GetFileNameWithoutExtension (assembly.OutputAssembly);
			}
			var contentNs = new CodeNamespace (nameSpace);


			contentUnit.Namespaces.Add (contentNs);

			DeclareStructs (contentNs);

			GenerateVertexBuffer (contentNs);

			GenerateProgram (contentNs);

			//defaultConstructor.Statements.Add (new CodeVariableDeclarationStatement (typeof(int), "testInt", new CodePrimitiveExpression (0)));

			return contentUnit;
		}
		#endregion

		private void SetBufferBindings(CodeTypeDeclaration dest)
		{
			foreach (var block in mExtractor.Blocks)
			{
				if (block.StructType == GLSLStructType.Buffer)
				{
					foreach (var member in block.Members)
					{
						var typeDecl = (member.ArrayDetails != null)
							?  new CodeTypeReference (member.ArrayDetails.TypeInformation.Name + "[]")
							: new CodeTypeReference (member.TypeString);						
						var field1 = new CodeMemberField (typeDecl, member.Name);
						field1.Attributes = MemberAttributes.Public;
						dest.Members.Add (field1);
					}
				}			
			}
		}

		static void AddUniformMember (CodeTypeDeclaration dest, StructMember member, CodeConstructor defaultConstructor)
		{
			if (member.ClosestType != null)
			{
				var field1 = new CodeMemberField (member.ClosestType.MatchingType, member.Name);
				field1.Attributes = MemberAttributes.Public;
				dest.Members.Add (field1);
			}
			else if (member.ArrayDetails != null)
			{
				var arrayType = new CodeTypeReference (member.ArrayDetails.TypeInformation.Name + "[]");
				var field1 = new CodeMemberField (arrayType, member.Name);
				field1.Attributes = MemberAttributes.Public;
				dest.Members.Add (field1);
				defaultConstructor.Statements.Add (new CodeVariableDeclarationStatement (arrayType, member.Name, new CodeArrayCreateExpression (arrayType, member.ArrayDetails.ArraySize)));
			}
		}

		void AddUniforms (CodeTypeDeclaration dest, CodeConstructor defaultConstructor)
		{
			foreach (var member in mExtractor.Uniforms)
			{
				AddUniformMember (dest, member, defaultConstructor);
			}
		}

		void AddAttributes (CodeTypeDeclaration inputBindings, CodeTypeDeclaration outputBindings)
		{
			foreach (var member in mExtractor.Attributes)
			{
				if (member.ClosestType != null)
				{
					if (member.Layout != null)
					{
						if (member.Direction == "in" || member.Direction == "inout")
						{
							AddLocationIndex (member, inputBindings);
						}
						if (member.Direction == "out" || member.Direction == "inout")
						{
							AddLocationIndex (member, outputBindings);
						}
					}
				}
			}
		}

		static void AddLocationIndex (InputAttribute member, CodeTypeDeclaration dest)
		{
			var field1 = new CodeMemberField (typeof(int), member.Name);
			field1.Attributes = MemberAttributes.Public;
			if (member.Layout.Location.HasValue)
			{
				field1.InitExpression = new CodePrimitiveExpression (member.Layout.Location.Value);
			}
			dest.Members.Add (field1);
		}
	}
}

