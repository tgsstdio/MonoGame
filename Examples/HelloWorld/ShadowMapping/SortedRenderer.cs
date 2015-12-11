using BirdNest.MonoGame;

namespace ShadowMapping
{
	class SortedRenderer : ISortedRenderer
	{
		public ISortedPassHelper Add (Pass frame_0)
		{
			return new PassHelper (this, frame_0);
		}

		public void Sort ()
		{
			
		}
	}

	class PassHelper : ISortedPassHelper
	{
		public SortedRenderer Renderer;
		public Pass Parent;

		public PassHelper (SortedRenderer r, Pass p)
		{
			Renderer = r;
			Parent = p;
		}

		public ISortedPassHelper Outputs(params TextureOutput[] output)
		{
			return this;
		}

		public ISortedPassHelper Requires (params TextureOutput[] shadowMap)
		{
			return this;
		}
	}

}

