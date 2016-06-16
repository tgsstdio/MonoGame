using System;
using MonoGame.Core;
using System.IO;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLTitleContainer : BaseTitleContainer
	{
		public DesktopGLTitleContainer ()
			: base(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content"),
				Path.DirectorySeparatorChar == BackwardSlash ? ForwardSlash : BackwardSlash,
				Path.DirectorySeparatorChar
				)
		{

		}

		#region ITitleContainer implementation

		protected override Stream OpenSafeStream (string safeName)
		{
			var absolutePath = Path.Combine(Location, safeName);
			return File.OpenRead(absolutePath);
		}

		protected override bool StreamExists (string fullPath)
		{
			var absolutePath = Path.Combine(Location, fullPath);
			return File.Exists(absolutePath);
		}

		#endregion
	}
}

