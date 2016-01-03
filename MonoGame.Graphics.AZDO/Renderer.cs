using System;

namespace MonoGame.Graphics.AZDO
{
	public class Renderer
	{
		public Renderer ()
		{
		}

		public class Marker
		{

		}

		public class Resource
		{

		}

		Mesh FindMesh ()
		{
			throw new NotImplementedException ();
		}

		public class Effect
		{
			public EffectPass[] Passes { get; set; }
		}

		public class MeshEffect
		{
			public int PassNumber { get; set;}
			public Effect Technique { get; set;}
			public Mesh Part { get; set; }
			public int Options { get; set; }
		}

		public void RegisterModelNode(MeshEffect[] nodes)
		{
			foreach (var entry in nodes)
			{
				var pass = entry.Technique.Passes [entry.PassNumber];
				RegisterMesh (entry.Part, pass, entry.Options);
			}
		}

		public class MeshMarkerCollection
		{
			public void Add(object item)
			{

			}

			public bool Exists(Mesh m, out Marker found)
			{
				throw new NotImplementedException ();
			}
		}

		public class Buffer
		{

		}

		public class Criteria
		{

		}

		public MeshMarkerCollection markers;
		public IConstantBufferCollection buffers;

		public void RegisterMesh(Mesh mesh, EffectPass pass, int options)
		{
			var vertexFormat = mesh.VertexFormat;

			Marker found;
			if (markers.Exists (mesh, out found))
			{

			}
			else
			{

			}

			var programId = pass.Match (vertexFormat, options);

			var id = mesh.GetBlockId();

			var candidates = buffers.Filter (mesh, pass, options);
			if (candidates.Length == 0)
			{
				buffers.Add (null);
			}

		}
	}
}

