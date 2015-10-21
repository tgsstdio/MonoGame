using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	public class VertexBufferSlotCollator : IMeshSlotCollator
	{
		private readonly Dictionary<MeshSlot, List<SceneNode>> mSlots;
		public VertexBufferSlotCollator ()
		{
			mSlots = new Dictionary<MeshSlot, List<SceneNode>>();
		}

		#region IPassCollator implementation
		public void Clear ()
		{
			mSlots.Clear ();
		}

		public void Collate (SceneNode node)
		{
			foreach (var mesh in node.ObjectModel.Meshes)
			{
				var key = new MeshSlot ();
				key.Block = node.ObjectModel.Asset.Block;
				key.Format = mesh.Format;
				key.Usage = mesh.Usage;

				List<SceneNode> result;
				if (mSlots.TryGetValue (key, out result))
				{
					result.Add (node);
				} else
				{
					result = new List<SceneNode> ();				
					result.Add (node);
					mSlots.Add (key, result);
				}
			}
		}

		public MeshSlot[] Slots ()
		{
			var result = new MeshSlot[mSlots.Keys.Count];
			mSlots.Keys.CopyTo(result, 0);		
			return result;
		}
		#endregion
	}
}

