using System.Collections.Generic;

namespace MonoGame.Graphics.AZDO
{
	public class MeshMarkerRegistry : IMeshMarkerRegistry
	{
		private readonly List<Mesh> mSources;
		public MeshMarkerRegistry ()
		{
			mSources = new List<Mesh> ();
		}

		#region IMeshMarkerRegistry implementation

		public IList<Mesh> Sources {
			get {
				return mSources;
			}
		}

		#endregion

//
//		public void RegisterModelNode(MeshEffect[] nodes)
//		{
//			foreach (var entry in nodes)
//			{
//				var pass = entry.Technique.Passes [entry.PassNumber];
//				RegisterMesh (entry.Part, pass, entry.Options);
//			}
//		}
//
//		public void RegisterMesh(Mesh mesh, EffectPass pass, ushort options)
//		{
//			var vertexFormat = mesh.VertexFormat;
//
//			Marker found;
//			if (markers.Exists (mesh, out found))
//			{
//
//			}
//			else
//			{
//
//			}			 
//
//			EffectShaderVariant program;
//			if (pass.Variants.TryGetValue(options, out program))
//			{
//				var id = mesh.GetBlockId();
//
//				var candidates = mBuffers.Filter (mesh, pass, options);
//				if (candidates.Length == 0)
//				{
//					mBuffers.Add (null);
//				}
//			}
//
//		}
	}
}

