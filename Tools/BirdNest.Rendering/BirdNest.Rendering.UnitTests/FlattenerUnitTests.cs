using NUnit.Framework;
using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.Rendering.UnitTests
{
	[TestFixture]
	public class FlattenerUnitTests
	{
		[TestCase]
		public void Flatten ()
		{
			var s1 = new SceneNode ();
			s1.ObjectModel = new ObjectModelInfo ();
			var EXPECTED_FORMAT = ModelUserFormat.Simple;
			var EXPECTED_USAGE = ModelBufferUsage.Static;
			var EXPECTED_BLOCK = new BlockIdentifier {BlockId = 13};
			var EXPECTED_MODEL_ID = new AssetIdentifier {
				AssetId = 10000
			};
			s1.ObjectModel.Asset = new AssetInfo{
				Identifier = EXPECTED_MODEL_ID,
				AssetType= AssetType.Mesh, 
				Block = EXPECTED_BLOCK,
				Name = "BLOCK 13",
				Version = 0,
			};
			s1.ObjectModel.Meshes = new MeshInfo[1];
			var mesh = new MeshInfo ();
			const uint EXPECTED_INDEX = 0;
			mesh.MeshIndex = EXPECTED_INDEX;
			mesh.Format = EXPECTED_FORMAT;
			mesh.Usage = EXPECTED_USAGE;
			var EXPECTED_SHADER_ID = new AssetIdentifier {
				AssetId = 10001
			};
			mesh.Program = EXPECTED_SHADER_ID;
			mesh.State = new DrawState{Id = new InstanceIdentifier{InstanceId =2000}};
			s1.ObjectModel.Meshes [0] = mesh;

			INodeFlattener roller = new SceneNodeFlattener ();
			roller.Parse (s1);
			Assert.AreEqual (1, roller.Passes.Count);
			var pass = roller.Passes [0];
			Assert.AreSame (s1, pass.Origin);
			Assert.AreEqual (EXPECTED_INDEX, pass.MeshIndex);
			Assert.AreEqual (EXPECTED_MODEL_ID, pass.Model);
			Assert.AreEqual (EXPECTED_BLOCK, pass.Block);
			Assert.AreEqual (EXPECTED_FORMAT, pass.Format);
			Assert.AreEqual (EXPECTED_SHADER_ID, pass.Program);
		}

		[TestCase]
		public void FlattenNullObjectModel ()
		{
			var s1 = new SceneNode ();
			s1.ObjectModel = null;

			INodeFlattener roller = new SceneNodeFlattener ();
			roller.Parse (s1);
			Assert.AreEqual (0, roller.Passes.Count);
		}

		[TestCase]
		public void FlattenNullMeshes ()
		{
			var s1 = new SceneNode ();
			s1.ObjectModel = new ObjectModelInfo ();
			s1.ObjectModel.Meshes = null;

			INodeFlattener roller = new SceneNodeFlattener ();
			roller.Parse (s1);
			Assert.AreEqual (0, roller.Passes.Count);
		}

		[TestCase]
		public void Flatten2 ()
		{
			var s1 = new SceneNode ();
			s1.ObjectModel = new ObjectModelInfo ();
			var EXPECTED_FORMAT_0 = ModelUserFormat.Simple;
			var EXPECTED_USAGE_0 = ModelBufferUsage.Static;
			var EXPECTED_BLOCK = new BlockIdentifier {BlockId = 13};
			var EXPECTED_MODEL_ID = new AssetIdentifier {
				AssetId = 10000
			};
			s1.ObjectModel.Asset = new AssetInfo{
				Identifier = EXPECTED_MODEL_ID,
				AssetType= AssetType.Mesh, 
				Block = EXPECTED_BLOCK,
				Name = "BLOCK 13",
				Version = 0,
			};
			s1.ObjectModel.Meshes = new MeshInfo[2];
			var mesh_0 = new MeshInfo ();
			const uint EXPECTED_INDEX_0 = 0;
			mesh_0.MeshIndex = EXPECTED_INDEX_0;
			mesh_0.Format = EXPECTED_FORMAT_0;
			mesh_0.Usage = EXPECTED_USAGE_0;
			var EXPECTED_SHADER_ID_0 = new AssetIdentifier {
				AssetId = 10001
			};
			mesh_0.Program = EXPECTED_SHADER_ID_0;
			mesh_0.State = new DrawState{Id = new InstanceIdentifier{InstanceId =2000}};
			s1.ObjectModel.Meshes [0] = mesh_0;

			var mesh_1 = new MeshInfo ();
			const uint EXPECTED_INDEX_1 = 1;
			var EXPECTED_FORMAT_1 = ModelUserFormat.Detailed;
			var EXPECTED_USAGE_1 = ModelBufferUsage.Dynamic;
			mesh_1.MeshIndex = EXPECTED_INDEX_1;
			mesh_1.Format = EXPECTED_FORMAT_1;
			mesh_1.Usage = EXPECTED_USAGE_1;
			var EXPECTED_SHADER_ID_1 = new AssetIdentifier {
				AssetId = 10002
			};
			mesh_1.Program = EXPECTED_SHADER_ID_1;
			mesh_1.State = new DrawState{Id = new InstanceIdentifier{InstanceId =2000}};
			s1.ObjectModel.Meshes [1] = mesh_1;

			INodeFlattener roller = new SceneNodeFlattener ();
			roller.Parse (s1);
			Assert.AreEqual (2, roller.Passes.Count);
			var pass_0 = roller.Passes [0];
			Assert.AreSame (s1, pass_0.Origin);
			Assert.AreEqual (EXPECTED_INDEX_0, pass_0.MeshIndex);
			Assert.AreEqual (EXPECTED_MODEL_ID, pass_0.Model);
			Assert.AreEqual (EXPECTED_BLOCK, pass_0.Block);
			Assert.AreEqual( EXPECTED_USAGE_0, pass_0.Usage);
			Assert.AreEqual (EXPECTED_FORMAT_0, pass_0.Format);
			Assert.AreEqual (EXPECTED_SHADER_ID_0, pass_0.Program);

			var pass_1 = roller.Passes [1];
			Assert.AreSame (s1, pass_0.Origin);
			Assert.AreEqual (EXPECTED_INDEX_1, pass_1.MeshIndex);
			Assert.AreEqual (EXPECTED_MODEL_ID, pass_1.Model);
			Assert.AreEqual (EXPECTED_BLOCK, pass_1.Block);
			Assert.AreEqual( EXPECTED_USAGE_1, pass_1.Usage);
			Assert.AreEqual (EXPECTED_FORMAT_1, pass_1.Format);
			Assert.AreEqual (EXPECTED_SHADER_ID_1, pass_1.Program);
		}
	}
}

