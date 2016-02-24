// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework
{
	public interface IGamePlatform : IDisposable
	{
		void BeforeInitialize ();
		bool BeforeRun();
		void Present();
		void RunLoop (Action doTick);
		void StartRunLoop();
		bool BeforeUpdate(GameTime gameTime);
		bool BeforeDraw(GameTime gameTime);
		void EnterFullScreen();
		void ExitFullScreen();
		TimeSpan TargetElapsedTimeChanging(TimeSpan value);
		void BeginScreenDeviceChange (bool willBeFullScreen);
		void EndScreenDeviceChange (string screenDeviceName, int clientWidth, int clientHeight);
		void TargetElapsedTimeChanged();
		void ResetElapsedTime();
		//void OnIsMouseVisibleChanged();

		bool IsMouseVisible {get;set;}

		void Log (string message);
		IGameWindow Window { get; }
		GameRunBehavior DefaultRunBehavior { get; }
		void AddAsyncHandler(EventHandler<EventArgs> handler);
		void RemoveAsyncHandler(EventHandler<EventArgs> handler);
		void Exit();
	}
}

