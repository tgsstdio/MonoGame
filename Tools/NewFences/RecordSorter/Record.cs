using System;
using MonoGame.Graphics;

namespace RecordSorter
{
	public class Record : IComparable<Record>
	{
		public DrawMode Mode { get; set; }
		public byte UniformsIndex { get; set; }
		public uint MarkerIndex { get; set; }
		public DrawItemBitFlags Flags { get; set; }

		public RasterizerState RasterizerValues {get;set;}
		public StencilState StencilValues {get;set;}
		public BlendState BlendValues { get; set;}
		public DepthState DepthValues {get;set;}

		#region IComparable implementation

		public int CompareTo (Record other)
		{
			if (Mode < other.Mode)
			{
				return -1;
			} 
			else if (Mode > other.Mode)
			{
				return 1;
			}

			// UNIFORMS
			if (UniformsIndex < other.UniformsIndex)
			{
				return -1;
			}
			else if (UniformsIndex > other.UniformsIndex)
			{
				return 1;
			}

			// MarkerIndex
			if (MarkerIndex < other.MarkerIndex)
			{
				return -1;
			}
			else if (MarkerIndex > other.MarkerIndex)
			{
				return 1;
			}

			// FLAGS
			if (Flags < other.Flags)
			{
				return -1;
			}
			else if (Flags > other.Flags)
			{
				return 1;
			}

			int result = RasterizerValues.CompareTo (other.RasterizerValues);
			if (result != 0)
			{
				return result;
			}

			result = StencilValues.CompareTo (other.StencilValues);
			if (result != 0)
			{
				return result;
			}

			result = BlendValues.CompareTo (other.BlendValues);
			if (result != 0)
			{
				return result;
			}

			result = DepthValues.CompareTo (other.DepthValues);

			// last check
			return result;
		}

		#endregion
	}
}

