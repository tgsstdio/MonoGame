using System;
using MonoGame.Core;
using System.IO;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLTitleContainer : BaseTitleContainer
	{
		public DesktopGLTitleContainer ()
			: base(
				AppDomain.CurrentDomain.BaseDirectory,
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
			return File.Exists(fullPath);
		}

		#endregion
	}
}

