using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	public class FullDepthStencilCapabilities : IDepthStencilCapabilities
	{
		#region IDepthStencilCapabilities implementation
		private bool mIsDepthBufferEnabled;
		public bool IsDepthBufferEnabled {
			get {
				return mIsDepthBufferEnabled;
			}
		}

		public void Initialise ()
		{
			DisableDepthBuffer ();
			SetDepthBufferFunc (CompareFunction.Less);
			DisableStencilBuffer ();
		}

		public void EnableDepthBuffer ()
		{
			GL.Enable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = true;
		}

		public void DisableDepthBuffer ()
		{
			GL.Disable(EnableCap.DepthTest);
			mIsDepthBufferEnabled = false;
		}

		private static DepthFunction GetDepthFunction(CompareFunction compare)
		{
			switch (compare)
			{
			default:
			case CompareFunction.Always:
				return DepthFunction.Always;
			case CompareFunction.Equal:
				return DepthFunction.Equal;
			case CompareFunction.Greater:
				return DepthFunction.Greater;
			case CompareFunction.GreaterEqual:
				return DepthFunction.Gequal;
			case CompareFunction.Less:
				return DepthFunction.Less;
			case CompareFunction.LessEqual:
				return DepthFunction.Lequal;
			case CompareFunction.Never:
				return DepthFunction.Never;
			case CompareFunction.NotEqual:
				return DepthFunction.Notequal;
			}
		}

		public void SetDepthBufferFunc(CompareFunction func)
		{
			GL.DepthFunc (GetDepthFunction (func));
		}

		public void SetDepthMask (bool isMaskOn)
		{
			GL.DepthMask(isMaskOn);
		}

		public void EnableStencilBuffer()
		{
			GL.Disable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = true;
		}

		public void DisableStencilBuffer()
		{
			GL.Enable(EnableCap.StencilTest);
			mIsStencilBufferEnabled = false;
		}

		private bool mIsStencilBufferEnabled;
		public bool IsStencilBufferEnabled {
			get {
				return mIsStencilBufferEnabled;
			}
		}

		public void SetStencilWriteMask(int mask)
		{
			GL.StencilMask(mask);
		}

		#endregion

	}
}

