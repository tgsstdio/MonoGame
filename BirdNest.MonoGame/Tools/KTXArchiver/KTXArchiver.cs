using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using BirdNest.Core;

namespace KTXArchiver
{
	public class KTXArchiver
	{
		private readonly IMipmapGenerator mGenerator;
		private readonly IMipmapEncoder mEncoder;
		private readonly IKTXPacker mPacker;

		public KTXArchiver (IMipmapGenerator generator, IMipmapEncoder encoder, IKTXPacker packer)
		{
			mGenerator = generator;
			mEncoder = encoder;
			mPacker = packer;
			EncodingRequired = true;
			MipmapExtension = ".ktx";
		}

		public bool EncodingRequired {
			get;
			set;
		}

		private static async Task<int[]> RunAllEncodingProcesses (IMipmapEncoder encoder, string[] arguments)
		{
			var tasks = new List<Task<int>> ();
			foreach(var arg in arguments)
			{
				tasks.Add (RunEncoderAsync (encoder, arg));
			}			

			return await Task.WhenAll(tasks.ToArray ());
		}	

		// http://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
		private static async Task<int> RunEncoderAsync(IMipmapEncoder encoder, string args)
		{
			using (var process = new Process
				{
					StartInfo =
					{
						FileName = encoder.ProgramFile,
						Arguments = args,
						UseShellExecute = false, 
						CreateNoWindow = true,
						// FOR DECODING
						RedirectStandardOutput = false, 
						RedirectStandardError = false
					},
					EnableRaisingEvents = true
				})
			{
				return await RunEncoderAsync(process).ConfigureAwait(false);
			}
		}    
		private static Task<int> RunEncoderAsync(Process process)
		{
			var tcs = new TaskCompletionSource<int>();

			process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
			//			process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
			//			process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

			bool started = process.Start();
			if (!started)
			{
				//you may allow for the process to be re-used (started = false) 
				//but I'm not sure about the guarantees of the Exited event in such a case
				throw new InvalidOperationException("Could not start process: " + process);
			}

			//			process.BeginOutputReadLine();
			//			process.BeginErrorReadLine();

			return tcs.Task;
		}

		public string MipmapExtension {
			get;
			set;
		}
		public List<string> Run(BlockFile[] blocks)
		{
			mGenerator.Initialise (MipmapExtension);
			var images = mGenerator.GenerateMipmaps (blocks);

			if (EncodingRequired)
			{
				var commands = mEncoder.GenerateArguments (images);
				var processes = RunAllEncodingProcesses (mEncoder, commands);
				processes.Wait ();
				foreach (var result in processes.Result)
				{
					if (result != 0)
					{
						throw new Exception ("Encoding step failed");
					}
				}
			}

			mPacker.Initialise (mBuffer);
			return mPacker.PackImages (images);
		}

		private byte[] mBuffer;
		public void Initialise(byte[] buffer)
		{
			mBuffer = buffer;
		}
	}
}

