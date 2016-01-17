using System;
using MonoGame.Content;

namespace BirdNest.Rendering
{
	public class MeshSlot
	{
		public BlockIdentifier Block { get; set; }
		public ModelUserFormat Format {get;set;}
		public ModelBufferUsage Usage {get;set;}

		public MeshSlot ()
		{
			Format = ModelUserFormat.Simple;
			Usage = ModelBufferUsage.Static;
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || ! (obj is MeshSlot)) 
				return false;
			else 
			{
				var right = obj as MeshSlot;
				return 
					(this.Block.BlockId == right.Block.BlockId)
				&&
					(this.Format == right.Format)
				&&
					(this.Usage == right.Usage);
			}
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				// Maybe nullity checks, if these are objects not primitives!
				hash = hash * 23 + Block.GetHashCode();
				hash = hash * 23 + Format.GetHashCode();
				hash = hash * 23 + Usage.GetHashCode();
				return hash;
			}
		}
	}
}

