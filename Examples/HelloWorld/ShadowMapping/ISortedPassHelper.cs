using BirdNest.MonoGame;

namespace ShadowMapping
{
	interface ISortedPassHelper
	{
		ISortedPassHelper Outputs (params TextureOutput[] output);
		ISortedPassHelper Requires (params TextureOutput[] shadowMap);
	}
}

