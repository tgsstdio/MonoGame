using System;

namespace BirdNest.Rendering
{
	public struct ClearState
	{
		public bool ClearColour { get; set; }
		public bool ClearStencil { get; set; }
		public bool ClearDepth {get;set;}
	}
}

