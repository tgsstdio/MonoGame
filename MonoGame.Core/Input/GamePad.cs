// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary> 
    /// Supports querying the game controllers and setting the vibration motors.
    /// </summary>
    public partial class GamePad
    {
		public bool Back { get; set; }

		private IGamepadPlatform mPlatform;
		public GamePad (IGamepadPlatform platform)
		{
			mPlatform = platform;
		}

        /// <summary>
        /// Returns the capabilites of the connected controller.
        /// </summary>
		/// <param name="player">Player index for the controller you want to query.</param>
        /// <returns>The capabilites of the controller.</returns>
        public GamePadCapabilities GetCapabilities(PlayerIndex player)
        {
            // Make sure the player index is in range.
            var index = (int)player;
            if (index < (int)PlayerIndex.One || index > (int)PlayerIndex.Four)
                throw new InvalidOperationException();

			return mPlatform.GetCapabilities(index);
        }

        
        /// <summary>
        /// Gets the current state of a game pad controller with an independent axes dead zone.
        /// </summary>
		/// <param name="player">Player index for the controller you want to query.</param>
        /// <returns>The state of the controller.</returns>
        public GamePadState GetState(PlayerIndex player)
        {
            return GetState(player, GamePadDeadZone.IndependentAxes);
        }


        /// <summary>
        /// Gets the current state of a game pad controller, using a specified dead zone
        /// on analog stick positions.
        /// </summary>
		/// <param name="player">Player index for the controller you want to query.</param>
        /// <param name="deadZoneMode">Enumerated value that specifies what dead zone type to use.</param>
        /// <returns>The state of the controller.</returns>
        public GamePadState GetState(PlayerIndex player, GamePadDeadZone deadZoneMode)
        {
            // Make sure the player index is in range.
            var index = (int)player;
            if (index < (int)PlayerIndex.One || index > (int)PlayerIndex.Four)
                throw new InvalidOperationException();

            return mPlatform.GetState(index, deadZoneMode);
        }


        /// <summary>
        /// Sets the vibration motor speeds on the controller device if supported.
        /// </summary>
		/// <param name="player">Player index that identifies the controller to set.</param>
        /// <param name="leftMotor">The speed of the left motor, between 0.0 and 1.0. This motor is a low-frequency motor.</param>
        /// <param name="rightMotor">The speed of the right motor, between 0.0 and 1.0. This motor is a high-frequency motor.</param>
        /// <returns>Returns true if the vibration motors were set.</returns>
        public bool SetVibration(PlayerIndex player, float leftMotor, float rightMotor)
        {
            // Make sure the player index is in range.
            var index = (int)player;
            if (index < (int)PlayerIndex.One || index > (int)PlayerIndex.Four)
                throw new InvalidOperationException();

            return mPlatform.SetVibration(index, MathHelper.Clamp(leftMotor, 0.0f, 1.0f), MathHelper.Clamp(rightMotor, 0.0f, 1.0f));
        }
    }
}
