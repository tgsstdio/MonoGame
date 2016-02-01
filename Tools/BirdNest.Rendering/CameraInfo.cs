using MonoGame.Shaders;

namespace BirdNest.Rendering
{
	public class CameraInfo : IViewer
	{
		public InstanceIdentifier Id {
			get;
			set;
		}

		#region IViewer implementation

		public LayerType GetLayerType ()
		{
			return LayerType.Visible;
		}

		#endregion
	}
}

