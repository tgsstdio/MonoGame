﻿using System.Collections.Specialized;
using OpenTK.Graphics.ES20;
using MonoGame.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class Es20GLSpecificExtensionLookup : IGLExtensionLookup
	{
		public StringCollection Extensions {get; private set;}

		public Es20GLSpecificExtensionLookup ()
		{
			Extensions = new StringCollection();			
		}

		public void Initialise()
		{
			Extensions.Clear ();

			ProirToVersion3_0 ();
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}

		public bool HasExtension (string extension)
		{
			return Extensions.Contains(extension);
		}
	}
}

