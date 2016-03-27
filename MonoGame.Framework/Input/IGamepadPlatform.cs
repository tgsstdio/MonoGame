namespace Microsoft.Xna.Framework.Input
{
	public interface IGamepadPlatform
	{
		bool SetVibration (int index, float left, float right);

		GamePadState GetState (int index, GamePadDeadZone deadZoneMode);

		GamePadCapabilities GetCapabilities (int index);
	}
}

