using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLShaderModule : IMgShaderModule
	{
		public int? ShaderId { get; set; }
		public MgShaderModuleCreateInfo Info { get; set; }

		public void Destroy()
		{
			if (ShaderId.HasValue)
			{
				GL.DeleteShader(ShaderId.Value);
				ShaderId = null;
			}
		}

		#region IMgShaderModule implementation
		private bool mIsDisposed = false;
		public void DestroyShaderModule (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			if (ShaderId.HasValue)
			{
				GL.DeleteShader(ShaderId.Value);
				ShaderId = null;
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

