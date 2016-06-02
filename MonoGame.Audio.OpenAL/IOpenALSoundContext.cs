using System;

namespace MonoGame.Audio.OpenAL
{
	public interface IOpenALSoundContext : IDisposable
	{
		IntPtr Device { get; }		
		bool Initialize();

		/// <summary>
		/// Checks the error state of the OpenAL driver. If a value that is not AlcError.NoError
		/// is returned, then the operation message and the error code is output to the console.
		/// </summary>
		/// <param name="operation">the operation message</param>
		/// <returns>true if an error occurs, and false if not.</returns>
		bool CheckError(string operation);
		Exception InitialisationError { get; }
	}
}

