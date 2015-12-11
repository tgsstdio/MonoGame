using OpenTK;

namespace ShadowMapping
{
	public class LightInfo : IPivot
	{
		public Vector3 Position {get;set;}

		public Vector3 Up {
			get;
			set;
		}

		public Vector3 Target {
			get;
			set;
		}

		public Matrix4 ViewMatrix {
			get;
			set;
		}

		public Matrix4 ProjectionMatrix {
			get;
			set;
		}

		public int PixelWidth {
			get;
			set;
		}

		public int PixelHeight {
			get;
			set;
		}

		public void Update()
		{
			ViewMatrix  = Matrix4.LookAt (Position, Target, Up);
			ProjectionMatrix = Matrix4.CreateOrthographic(PixelWidth, PixelHeight, 0, 1);
		}
	}
}

