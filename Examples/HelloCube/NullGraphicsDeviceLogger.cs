using System;
using DryIoc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Audio.OpenAL;
using MonoGame.Audio.OpenAL.DesktopGL;
using MonoGame.Platform.DesktopGL;
using MonoGame.Platform.DesktopGL.Graphics;
using MonoGame.Platform.DesktopGL.Input;
using OpenTK;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Graphics;

namespace HelloCube
{
	public class MockGraphicsDeviceLogger : IGraphicsDeviceLogger
	{
		#region IGraphicsDeviceLogger implementation
		public void Log (string message)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}

}
