using Newtonsoft.Json;
using System.IO;

namespace MonoGame.Content.Blocks.JsonNET
{
	public class JsonNETSerializer : IBlockFileSerializer
	{
		#region IBlockFileSerializer implementation

		public string GetBlockPath (BlockIdentifier id)
		{
			return id.BlockId + ".json";
		}

		public BlockFile Read (Stream fs)
		{
			using (var sReader = new StreamReader(fs))
			using (var jsReader = new JsonTextReader(sReader))				
			{
				jsReader.SupportMultipleContent = true;
				bool hasData = jsReader.Read ();
				if (hasData)
				{	   
					var serializer = new JsonSerializer ();
					return serializer.Deserialize<BlockFile> (jsReader);
				}
			}

			return null;
		}

		public void Write (Stream writer, BlockFile block)
		{
			using (var stringWriter = new StreamWriter(writer))
			using (var jsonWriter = new JsonTextWriter(stringWriter))				
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize (jsonWriter, block);
			}
		}

		public async System.Threading.Tasks.Task<BlockFile> ReadAsync (Stream reader)
		{
			using (var stringReader = new StreamReader (reader))
			{
				string token = await stringReader.ReadToEndAsync ();
				return JsonConvert.DeserializeObject<BlockFile>(token);
			}

		}

		public async System.Threading.Tasks.Task WriteAsync (Stream writer, BlockFile block)
		{
			using (var stringWriter = new StreamWriter (writer))
			{
				string token = JsonConvert.SerializeObject(block);				
				await stringWriter.WriteLineAsync (token);
			}
		}
		#endregion


	}
}

