// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenTK.Platform.Android;
using DryIoc;
using MonoGame.Platform.AndroidGL.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Android.Content.PM;
using MonoGame.Platform.AndroidGL.Input.Touch;
using MonoGame.Core;

namespace MonoGame.Platform.AndroidGL.Example
{
	using Container = DryIoc.Container;

	class NullResumeManager : IResumeManager
	{
		#region IResumeManager implementation
		public void LoadContent ()
		{
			throw new NotImplementedException ();
		}
		public void Draw ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}


	

}
