using System.Runtime.InteropServices;
using OpenTK;

namespace ShadowMapping
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CameraData
	{
		public Matrix4 projectionMatrix;
		public Matrix4 inverseProjectionMatrix;
		public Matrix4 viewMatrix;
		public Matrix4 inverseViewMatrix;
		public float x;
		public float y;
		public float z;
		public float w;
		public Vector3 eye;
		public float fieldOfView;
	}
}

