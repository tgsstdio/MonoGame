using Magnesium.OpenGL;


namespace HelloMagnesium
{
	public class MockShaderProgramCache : IShaderProgramCache
	{
		#region IShaderProgramCache implementation

		public void BindDescriptorSet ()
		{
			throw new System.NotImplementedException ();
		}

		public byte DescriptorSetIndex {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		public GLCmdDescriptorSetParameter DescriptorSet {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		public int ProgramID {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		public int VBO {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		#endregion
	}

}
