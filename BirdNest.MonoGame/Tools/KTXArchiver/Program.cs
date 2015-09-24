using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using KtxSharp;
using KtxSharp.JsonNET;
using BirdNest.Core;

namespace KTXArchiver
{
	class MainClass
	{
		static void IsLegalEncoderFormat ()
		{
			for (int xdim_3d = 4; xdim_3d <= 12; ++xdim_3d)
			{
				for (int ydim_3d = 4; ydim_3d <= 12; ++ydim_3d)
				{
					bool is_legal_2d = (xdim_3d == ydim_3d) || (xdim_3d == ydim_3d + 1) || ((xdim_3d == ydim_3d + 2) && !(xdim_3d == 6 && ydim_3d == 4)) || (xdim_3d == 8 && ydim_3d == 5) || (xdim_3d == 10 && ydim_3d == 5) || (xdim_3d == 10 && ydim_3d == 6);
					if (is_legal_2d)
					{
						Debug.WriteLine ("Block{0}x{1},", xdim_3d, ydim_3d);
					}
				}
			}
		}


		public static int Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Stopwatch timer = new Stopwatch ();
			timer.Start ();

//			if (args.Length != 1)
//			{
//				return -1;
//			}
//
//			string blockFile = args [0];
			// open file using IBlockFileSerializer 
			BlockFile block = null;
			using (var fs = File.OpenRead ("blockFile001.json"))
			{
				IBlockFileSerializer des = new JsonNETSerializer ();
				block = des.Read (fs);
			}

			var generator = new MockMipmapGenerator ();
			var encoder = new FasTCEncoder ();
			var packer = new FasTCPacker ();

			var archiver = new KTXArchiver (generator, encoder, packer);
			archiver.EncodingRequired = false;

			try
			{
				var buffer = new byte[4096];

				archiver.Initialise(buffer);
				List<string> results = archiver.Run (new []{ block });

				foreach (var fileName in results)
				{
					using (var fs = File.OpenRead (fileName))
					using (var br = new BinaryReader (fs))
					{
						var topHeader = new KTXHeader ();

						br.Read (buffer, 0, topHeader.KTX_HEADER_SIZE);
						topHeader.Populate (buffer);
						if (topHeader.Instructions.Result != KTXError.Success)
						{
							throw new InvalidDataException ("KTX not found");
						}
						br.BaseStream.Seek(topHeader.KTX_HEADER_SIZE + topHeader.BytesOfKeyValueData, SeekOrigin.Begin);
						int imageSize = br.ReadInt32();
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine ("Exception : " + ex.Message);
			}

			timer.Stop ();				
			Console.WriteLine ("Encoder done : " + timer.Elapsed);

			return 0;
 		}
	}
}
