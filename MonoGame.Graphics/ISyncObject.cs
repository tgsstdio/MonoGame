namespace MonoGame.Graphics
{
	public interface ISyncObject
	{
		long Duration { get; set; }
		int Factor { get; set; }
		uint TotalBlockingWaits { get; }
		uint TotalFailures { get; }
		int NoOfRetries { get; set;}
		bool IsReady();
	}
}

