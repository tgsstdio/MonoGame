using System;

namespace BirdNest.Rendering.UnitTests
{
	public class MockShaderProgramCache : IShaderProgramCache
	{
		#region IShaderProgramCache implementation
		public bool TryGetValue (BirdNest.MonoGame.Core.AssetIdentifier id, out IShaderProgram result)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

