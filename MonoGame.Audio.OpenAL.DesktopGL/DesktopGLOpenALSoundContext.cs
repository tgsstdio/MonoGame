using System;
using OpenTK.Audio.OpenAL;
using OpenTK;

namespace MonoGame.Audio.OpenAL.DesktopGL
{
	public class DesktopGLOpenALSoundContext : IOpenALSoundContext
	{
		private IntPtr _device;
		private ContextHandle _context;

		public DesktopGLOpenALSoundContext ()
		{
			_SoundInitException = null;
		}

		#region IOpenALSoundContext implementation

		public Exception InitialisationError {
			get {
				return _SoundInitException;
			}
		}
		private Exception _SoundInitException;
		public bool Initialise ()
		{
			try
			{
				_device = Alc.OpenDevice(string.Empty);
			}
			catch (Exception ex)
			{
				_SoundInitException = ex;
				return (false);
			}
			if (CheckError("Could not open AL device"))
			{
				return(false);
			}

			if (_device != IntPtr.Zero)
			{
				//_acontext = new AudioContext();
				_context = Alc.GetCurrentContext();
				//_oggstreamer = new OggStreamer();

				if (CheckError("Could not create AL context"))
				{
					Dispose ();
					return(false);
				}

				if (_context != ContextHandle.Zero)
				{
					Alc.MakeContextCurrent (_context);
					if (CheckError ("Could not make AL context current"))
					{
						Dispose ();
						return(false);
					}
					return (true);
				}
			}
			return (false);
		}

		private AlcError _lastOpenALError;
		public bool CheckError (string operation)
		{
			_lastOpenALError = Alc.GetError (_device);

			if (_lastOpenALError == AlcError.NoError) {
				return(false);
			}

			const string errorFmt = "OpenAL Error: {0}";
			Console.WriteLine (String.Format ("{0} - {1}",
				operation,
				//string.Format (errorFmt, Alc.GetString (_device, _lastOpenALError))));
				string.Format (errorFmt, _lastOpenALError)));
			return (true);
		}

		public IntPtr Device {
			get {
				return _device;
			}
		}

		#endregion

		#region IDisposable implementation

		~DesktopGLOpenALSoundContext()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mDisposed = false;
		protected void Dispose (bool disposed)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposed)
			{
				ReleaseManagedResources ();
			}

			mDisposed = true;
		}

		private void ReleaseManagedResources()
		{

		}

		private void ReleaseUnmanagedResources()
		{
			Alc.MakeContextCurrent (ContextHandle.Zero);
			if (_context != ContextHandle.Zero) {
				Alc.DestroyContext (_context);
				_context = ContextHandle.Zero;
			}
			if (_device != IntPtr.Zero) {
				Alc.CloseDevice (_device);
				_device = IntPtr.Zero;
			}
		}


		#endregion
	}
}

