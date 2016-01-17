using System;
using MonoGame.Content;

namespace BirdNest.Rendering.UnitTests
{
	public class MockShaderProgramCache : IShaderProgramCache
	{
		#region IShaderProgramCache implementation
		public bool TryGetValue (AssetIdentifier id, out IShaderProgram result)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

