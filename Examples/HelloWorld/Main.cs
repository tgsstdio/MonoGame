namespace HelloWorld
{
	using System;

	class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			using (var game = new GameWorld())
			{
				// Run the game at 60 updates per second
				game.Run(60.0);
			}
		}
	}

}

