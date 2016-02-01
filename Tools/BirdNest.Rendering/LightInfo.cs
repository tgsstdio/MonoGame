using MonoGame.Shaders;

namespace BirdNest.Rendering
{
	public class LightInfo : IViewer
	{
		public InstanceIdentifier Id {
			get;
			set;
		}

		#region IViewer implementation

		public LayerType GetLayerType ()
		{
			return LayerType.DepthMap;
		}

		#endregion
	}
}

