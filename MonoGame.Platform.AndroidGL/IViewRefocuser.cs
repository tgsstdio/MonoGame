// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MonoGame.Platform.AndroidGL
{
	public interface IViewRefocuser
	{
		void Run();
		void Pause ();
		void Clear ();
		bool IsFocused { get; }
		void Refocus ();
		void MakeCurrent();
		void SwapBuffers ();
		void Resume();
	}
}
