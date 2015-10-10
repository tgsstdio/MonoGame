using OpenTK;

namespace ShadowMapping
{
	public class CameraInfo
	{
		public Matrix4 ProjectionMatrix { get; set;}
		public Matrix4 ViewMatrix { get; set;}
		public Vector3 Eye { get; set;}
		public Vector3 Target {get;set;}
		public Vector3 Up { get; set;}
		public float X {get;set;}
		public float Y {get;set;}
		public float FieldOfView {get;set;}
	}
}

