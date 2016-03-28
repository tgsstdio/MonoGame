// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Input
{
	public class DefaultGamepadPlatform : IGamepadPlatform
    {
		public bool Back { get; set; }

        public GamePadCapabilities GetCapabilities(int index)
        {
            GamePadCapabilities capabilities = new GamePadCapabilities();

            capabilities.IsConnected = (index == 0);
            capabilities.HasBackButton = true;

            return capabilities;
        }

        public GamePadState GetState(int index, GamePadDeadZone deadZoneMode)
        {
            GamePadState state;
            if (index == 0 && Back)
            {
                // Consume state
                Back = false;
                state = new GamePadState(new GamePadThumbSticks(), new GamePadTriggers(), new GamePadButtons(Buttons.Back), new GamePadDPad());
            }
            else
                state = new GamePadState();

            return state;
        }

        public bool SetVibration(int index, float leftMotor, float rightMotor)
        {
            return false;
        }
    }
}
