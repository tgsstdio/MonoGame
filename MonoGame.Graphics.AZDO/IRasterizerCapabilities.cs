namespace MonoGame.Graphics.AZDO
{
	public interface IRasterizerCapabilities
	{
		void DisablePolygonOffset ();

		void EnablePolygonOffset (float slopeScaleDepthBias, float depthBias);

		void SetUsingCounterClockwiseWindings (bool b);

		void EnableScissorTest ();

		void DisableScissorTest ();

		bool ScissorTestEnabled {
			get;
		}

		void SetCullingMode (bool front, bool back);

		void EnableCulling ();

		void DisableCulling ();

		bool CullingEnabled {
			get;
		}

		void Initialise();
	}
}
