using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	public class SceneNodeFlattener : INodeFlattener
	{
		private readonly List<RenderPass> mPasses;
		public SceneNodeFlattener ()
		{
			mPasses = new List<RenderPass> ();
		}

		public IList<RenderPass> Passes {
			get {
				return mPasses;
			}
		}

		#region INodeFlattener implementation
		public void Parse (SceneNode node)
		{
			if (node.ObjectModel != null && node.ObjectModel.Meshes != null)
			{
				foreach (var mesh in node.ObjectModel.Meshes)
				{
					var pass = new RenderPass ();
					pass.Origin = node;
					pass.Program = mesh.Program;
					pass.Block = node.ObjectModel.Asset.Block;
					pass.Model = node.ObjectModel.Asset.Identifier;
					pass.MeshIndex = mesh.MeshIndex;
					pass.Usage = mesh.Usage;
					pass.Format = mesh.Format;
					pass.State = mesh.State;
					mPasses.Add (pass);
				}
			}
		}
		#endregion
	}

}

