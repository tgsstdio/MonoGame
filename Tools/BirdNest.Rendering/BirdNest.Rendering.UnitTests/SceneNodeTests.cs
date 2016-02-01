using System;
using NUnit.Framework;
using MonoGame.Content;
using MonoGame.Shaders;

namespace BirdNest.Rendering.UnitTests
{
	[TestFixture]
	public class SceneNodeTests
	{
		static SceneNode GenerateNode (ulong blockId, ulong instanceId)
		{
			var model = new ObjectModelInfo ();
			model.Asset = new AssetInfo ();
			model.Asset.AssetType = AssetType.Model;
			model.Asset.Block = new BlockIdentifier {
				BlockId = blockId
			};
			model.Meshes = new MeshInfo [1];
			model.Meshes[0].Usage = ModelBufferUsage.Static;
			model.Meshes[0].Format = ModelUserFormat.Simple;
			var s1 = new SceneNode {
				Id = new InstanceIdentifier {
					InstanceId = instanceId
				},
				ObjectModel = model
			};
			return s1;
		}

		[TestCase]
		public void ClearSlots()
		{
			// take a bunch of scene nodes and generate a number of slots
			//   if required, create a group based on
			// i) model format (per shader??)
			// ii) static or dynamic 
			// iii) its block origin
			var s1 = GenerateNode (20000, 1000);

			IMeshSlotCollator g = new VertexBufferSlotCollator ();
			MeshSlot[] slots = g.Slots ();
			Assert.AreEqual (0, slots.Length);
			g.Clear ();
			slots = g.Slots ();
			Assert.AreEqual (0, slots.Length);
			g.Collate (s1);
			slots = g.Slots ();
			Assert.AreEqual (1, slots.Length);
			g.Clear ();
			slots = g.Slots ();
			Assert.AreEqual (0, slots.Length);
		}

		[TestCase]
		public void CompoundMeshes()
		{
			// take a "slot", fill a vbo with their data
			// and generate markers for reuse.
			var slots = new MeshSlot[10];
			IMeshMarker marker = new MeshMarker ();
			var entries = marker.GenerateMarkers (slots);
		}

		[TestCase]
		public void GenerateCommands()
		{
			// take markers and generate commands based on 
			// actual scene nodes; sharing is important
		}

		[TestCase]
		public void BindBuffersWithNodes()
		{
			// foreach scene node bind components to the appropriate binder
		}
	}
}

