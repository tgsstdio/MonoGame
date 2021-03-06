﻿using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using MonoGame.Content.Blocks;

namespace MonoGame.Content.Blocks.Protobuf
{
	public class BinarySerializer : IBlockFileSerializer
	{
		static BinarySerializer ()
		{
			RuntimeTypeModel.Default.Add (typeof(AssetType), false);

			RuntimeTypeModel.Default.Add (typeof(BlockIdentifier), false);

			RuntimeTypeModel.Default.Add (typeof(AssetIdentifier), false);

			RuntimeTypeModel.Default.Add (typeof(AssetInfo), false)
				.Add (1, "Identifier")
				//.Add (2, "AssetType") IGNORE
				.Add (3, "Name")
				.Add (4, "Version");
				//.Add (5, "Block") IGNORE TOO

			RuntimeTypeModel.Default.Add (typeof(TexturePageInfo), false)
				.Add (1, "Asset")
				.Add (2, "RelativePath");

			RuntimeTypeModel.Default.Add (typeof(TextureCatalog), false)
				.Add (1, "Width")
				.Add (2, "Height")
				.Add (3, "Depth")
				.Add (4, "MinFilter")
				.Add (5, "MagFilter")
				.Add (6, "TextureWrapS")
				.Add (7, "TextureWrapT");

			RuntimeTypeModel.Default.Add (typeof(TextureChapterInfo), false)
				.Add (1, "Catalog")
				.Add (2, "Pages");

			RuntimeTypeModel.Default.Add (typeof(ShaderInfo), false)
				.Add (1, "Asset")
				.Add (2, "ComputePath")
				.Add (3, "FragmentPath")
				.Add (4, "VertexPath");

			RuntimeTypeModel.Default.Add (typeof(BlockFile), false)
				.Add (1, "Identifier")
				.Add (2, "Chapters")
				.Add (3, "Shaders");


		}

		public string GetBlockPath (BlockIdentifier id)
		{
			return id.BlockId + ".pbb";
		}

		public BlockFile Read (Stream reader)
		{
			return Serializer.Deserialize<BlockFile> (reader);
		}

		public void Write(Stream writer, BlockFile block)
		{
			Serializer.Serialize (writer, block);
		}

		public async System.Threading.Tasks.Task<BlockFile> ReadAsync (Stream reader)
		{
			using (var memoryStream = new MemoryStream ())
			{
				await reader.CopyToAsync (memoryStream);
				return Serializer.Deserialize<BlockFile> (memoryStream);
			}
		}

		public async System.Threading.Tasks.Task WriteAsync (Stream writer, BlockFile block)
		{
			using (var memoryStream = new MemoryStream ())
			{
				Serializer.Serialize<BlockFile> (memoryStream, block);				
				await memoryStream.CopyToAsync (writer);
			}
		}
	}
}

