using System.Collections.Specialized;
using BirdNest.MonoGame.Core;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	/// <summary>
	/// Modern GL extension checker.
	/// // For OpenGL 3.0 and higher
	/// </summary>
	public class GLExtensionChecker : IExtensionChecker
	{
		public StringCollection Extensions {get; private set;}

		public void Initialize()
		{
			Extensions = new StringCollection();
			AfterVersion3_0 ();

			ProirToVersion3_0 ();
		}

		#region IExtensionChecker implementation

		public bool HasExtension (string extension)
		{
			return Extensions.Contains(extension);
		}

		void AfterVersion3_0 ()
		{
			int count = GL.GetInteger (GetPName.NumExtensions);
			for (int i = 0; i < count; i++)
			{
				string extension = GL.GetString (StringNameIndexed.Extensions, i);
				Extensions.Add (extension);
			}
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}
		#endregion
	}
}

