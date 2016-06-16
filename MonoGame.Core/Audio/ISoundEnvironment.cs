namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEnvironment
	{
		float DistanceScale { get; set; }
		float DopplerScale { get; set; }
		float SpeedOfSound { get; set; }
	}
}

