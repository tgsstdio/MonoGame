using System;
using System.IO;
using KtxSharp;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace KTXArchiver
{
	public class ASTCEncoder : IMipmapEncoder
	{
		private const int HEADER_SIZE = 16;

		public enum CompileOption
		{
			Default = 0,
			LDRSRBG,
			LDRLinear,
		}

		public class EncoderFormatInfo
		{
			public EncoderFormat Format;
			public string CommandArgument;
			public int CompressedRgba;
			public int CompressedSrgb8Alpha8;
		}

		public enum EncoderFormat : int
		{
			Block4x4 = 0,
			Block5x4,
			Block5x5,
			Block6x5,
			Block6x6,
			Block8x5,
			Block8x6,
			Block8x8,
			Block10x5,
			Block10x6,
			Block10x8,
			Block10x10,
			Block12x10,
			Block12x12,
		}

		public enum EncoderRate : int
		{
			VeryFast = 0,
			Fast,
			Medium,
			Thorough,
			Exhaustive,
		}

		public long SkipHeader(Stream fs)
		{
			return fs.Seek (HEADER_SIZE, SeekOrigin.Begin);
		}

		public ASTCHeader ReadHeader(Stream fs)
		{
			var buffer = new byte[HEADER_SIZE];

			int count = fs.Read (buffer, 0, HEADER_SIZE);
			if (count != HEADER_SIZE)
			{
				throw new FileLoadException ("ASTC HEADER missing");
			}

			return new ASTCHeader (buffer);
		}

		public void Read(Stream fs, byte[] buffer, int offset, int count)
		{
			int bytesRead = fs.Read(buffer, offset, count);
			if (bytesRead != count)
			{
				throw new FileLoadException ("ASTC image data missing");
			}
		}

		private string InputFileName {
			get;
			set;
		}

		private string OutputFileName {
			get;
			set;
		}

		public string ProgramFile {
			get {
				return "astcenc.exe";
			}
		}

		private readonly string[] COMPILER_OPTIONS;
		private readonly string[] ENCODER_RATE_COMMAND_ARGS;
		public ASTCEncoder ()
		{
			COMPILER_OPTIONS = new string[]{"-c", "-cs", "-cl"};
			ENCODER_RATE_COMMAND_ARGS = new string[]{"-veryfast", "-fast", "-medium", "-thorough", "-exhaustive"};
			InitialiseFormatValues ();
		}

		public ASTCEncoder.CompileOption Option{ get; set; }
		private string DetermineCompilerOption ()
		{
			return COMPILER_OPTIONS [(int) Option];
		}

		public EncoderRate Rate {get;set;}
		private string DetermineRate ()
		{
			return ENCODER_RATE_COMMAND_ARGS [(int) Rate];
		}

		public string[] GenerateArguments (List<BlockImageInfo> images)
		{
			var commands = new List<string> ();
			foreach (var image in images)
			{
				foreach(var mipmap in image.Mipmaps)
				{
					InputFileName = mipmap.InputFile;
					OutputFileName = mipmap.OutputFile;
					commands.Add (BuildArguments());
				}
			}
			return commands.ToArray ();
		}

		public EncoderFormat BlockFormat { get; set; }
		private Dictionary<EncoderFormat, EncoderFormatInfo> mValues; 
		private void InitialiseFormatValues ()
		{
			mValues = new Dictionary<EncoderFormat, EncoderFormatInfo> ();
			mValues.Add (EncoderFormat.Block4x4,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block4x4,
					CommandArgument = "4x4",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc4X4Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc4X4Khr,
				});

			mValues.Add (EncoderFormat.Block5x4,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block5x4,
					CommandArgument = "5x4",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc5X4Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc5X4Khr,
				});

			mValues.Add (EncoderFormat.Block5x5,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block5x5,
					CommandArgument = "5x5",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc5X5Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc5X5Khr,
				});

			mValues.Add (EncoderFormat.Block6x5,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block6x5,
					CommandArgument = "6x5",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc6X5Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc6X5Khr,
				});

			mValues.Add (EncoderFormat.Block6x6,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block6x6,
					CommandArgument = "6x6",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc6X6Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc6X6Khr,
				});

			mValues.Add (EncoderFormat.Block8x5,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block8x5,
					CommandArgument = "8x5",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc8X5Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc8X5Khr,
				});

			mValues.Add (EncoderFormat.Block10x5,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block10x5,
					CommandArgument = "10x5",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc10X5Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc10X5Khr,
				});

			mValues.Add (EncoderFormat.Block10x6,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block10x6,
					CommandArgument = "10x6",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc10X6Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc10X6Khr,
				});

			mValues.Add (EncoderFormat.Block10x8,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block10x8,
					CommandArgument = "10x8",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc10X8Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc10X8Khr,
				});

			mValues.Add (EncoderFormat.Block10x10,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block10x10,
					CommandArgument = "10x10",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc10X10Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc10X10Khr,
				});

			mValues.Add (EncoderFormat.Block12x10,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block12x10,
					CommandArgument = "12x10",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc12X10Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc12X10Khr,
				});

			mValues.Add (EncoderFormat.Block12x12,
				new EncoderFormatInfo{ 
					Format = EncoderFormat.Block12x12,
					CommandArgument = "12x12",
					CompressedRgba = (int) KhrTextureCompressionAstcHdr.CompressedRgbaAstc12X12Khr,
					CompressedSrgb8Alpha8 = (int) KhrTextureCompressionAstcHdr.CompressedSrgb8Alpha8Astc12X12Khr,
				});
		}

		public int GetInternalFormat ()
		{	
			var blockInfo = mValues [BlockFormat]; 
			if (this.Option == CompileOption.LDRSRBG || this.Option == CompileOption.LDRLinear)
			{
				return blockInfo.CompressedSrgb8Alpha8;
			} 
			else
			{
				return blockInfo.CompressedRgba;
			}
		}

		private string DetermineFormat ()
		{	
			return mValues [BlockFormat].CommandArgument;
		}

		private string BuildArguments ()
		{
			string compileOption = DetermineCompilerOption ();
			string format = DetermineFormat ();
			string rate = DetermineRate ();
			return string.Format("{0} {1} {2} {3} {4}", compileOption, InputFileName, OutputFileName, format, rate);
		}

	}
}

