using System;
using System.Collections.Generic;
using BirdNest.MonoGame.Graphics;
using OpenTK.Graphics.OpenGL;
using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame
{
	public class GLShaderRegistry : IShaderRegistry
	{
		private readonly Dictionary<ulong, ShaderProgramData> mPrograms;
		private readonly IAssetManager mAssetManager;
		public GLShaderRegistry (IAssetManager manager)
		{
			mAssetManager = manager;
			mPrograms = new Dictionary<ulong, ShaderProgramData> ();
		}

		~GLShaderRegistry()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize(this);
		}

		private bool mDisposed = false;
		protected void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		void ReleaseManagedResources()
		{
			foreach (var program in mPrograms.Values)
			{
				GL.DeleteProgram (program.ProgramID);
			}
			mPrograms.Clear ();
		}

		#region IShaderRegistry implementation

		public bool TryGetValue (AssetIdentifier identifier, out ShaderProgramData result)
		{
			return mPrograms.TryGetValue (identifier.AssetId, out result);
		}

		public void Add (AssetInfo key, ShaderProgramData program)
		{
			mAssetManager.Add (key);
			mPrograms.Add (key.Identifier.AssetId, program);
		}

		public void Remove (ShaderProgramData program)
		{
			mPrograms.Remove (program.Identifier.AssetId);
			mAssetManager.Remove (program.Identifier);
		}

		#endregion
	}
}

