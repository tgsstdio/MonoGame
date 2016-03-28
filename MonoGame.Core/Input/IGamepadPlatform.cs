namespace Microsoft.Xna.Framework.Input
{
	public interface IGamepadPlatform
	{
		bool SetVibration (int index, float leftMotor, float rightMotor);

		GamePadState GetState (int index, GamePadDeadZone deadZoneMode);

		GamePadCapabilities GetCapabilities (int index);
	}
}

