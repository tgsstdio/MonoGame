namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEnvironment
	{
		float MasterVolume {get; set; }
		float DistanceScale { get; set; }
		float DopplerScale { get; set; }
		float SpeedOfSound { get; set; }
	}
}

