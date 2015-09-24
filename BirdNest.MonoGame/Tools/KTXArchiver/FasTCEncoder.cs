using System;
using System.Collections.Generic;

namespace KTXArchiver
{
	public class FasTCEncoder : IMipmapEncoder
	{
		private string[] mFormatStrings;
		public FasTCEncoder ()
		{
			NoOfThreads = 4;
			EncoderFormat = Format.BPTC;
			mFormatStrings = new string[]{ "BPTC", "PVRTC" };
			Quality = 50;
		}

		public enum Format : int
		{
			BPTC = 0,
			PVRTC
		}

		public Format EncoderFormat { get; set; }
		public uint NoOfThreads { get; set; }
		public uint Quality { get; set; }

		public string ProgramFile {
			get {
				return "tc.exe";
			}
		}

		#region IMipmapEncoder implementation

		private string GetNoOfThreadsArgument ()
		{
			return "-t " + NoOfThreads;
		}

		private string GetFormatArgument ()
		{
			return "-f " + mFormatStrings [(int)EncoderFormat];
		}

		private string GetQualityArgument ()
		{
			return "-q " + Quality.ToString ();
		}

		public string[] GenerateArguments (List<BlockImageInfo> images)
		{
			string noOfThreads = GetNoOfThreadsArgument ();
			string formatArg = GetFormatArgument ();
			string qualityArg = GetQualityArgument ();

			var arguments = new List<string> ();
			foreach(var image in images)			
			{
				foreach(var map in image.Mipmaps)
				{
					arguments.Add (string.Format ("{0} {1} {2} {3}", noOfThreads, formatArg, qualityArg, map.InputFile));
				}
			}
			return arguments.ToArray ();
		}
		#endregion
	}
}

