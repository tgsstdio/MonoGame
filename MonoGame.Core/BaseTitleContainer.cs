using System;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame.Core
{
	public abstract class BaseTitleContainer : ITitleContainer
	{
		public static readonly char ForwardSlash = '/';
		public static readonly string ForwardSlashString = new string(ForwardSlash, 1);
		public static readonly char BackwardSlash = '\\';

		protected char NotSeparator { get; private set; }
		protected char Separator { get; private set; }

		protected BaseTitleContainer (string location, char notSeparator, char separator)
		{
			Location = location;
			NotSeparator = notSeparator;
			Separator = separator;
		}

		#region ITitleContainer implementation
		protected abstract System.IO.Stream OpenSafeStream (string safeName);
		protected abstract bool StreamExists (string fullPath);

		public bool Exists (string name)
		{
			// Normalize the file path.
			var safeName = GetFilename(name);

			// We do not accept absolute paths here.
			if (Path.IsPathRooted(safeName))
				throw new ArgumentException("Invalid filename. TitleContainer.Exists requires a relative path.");

			return StreamExists (safeName);
		}

		public virtual System.IO.Stream OpenStream (string name)
		{
			// Normalize the file path.
			var safeName = GetFilename(name);

			// We do not accept absolute paths here.
			if (Path.IsPathRooted(safeName))
				throw new ArgumentException("Invalid filename. TitleContainer.OpenStream requires a relative path.");

			return OpenSafeStream (safeName);
		}

		private string NormalizeFilePathSeparators(string name)
		{
			return name.Replace(NotSeparator, Separator);
		}

		// TODO: This is just path normalization.  Remove this
		// and replace it with a proper utility function.  I'm sure
		// this same logic is duplicated all over the code base.
		public string GetFilename(string name)
		{
			return NormalizeFilePathSeparators(new Uri("file:///" + name).LocalPath.Substring(1));
		}

		public string Location {
			get;
			protected set;
		}

		#endregion
	}
}

