using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PathResolvableArchive
{
	public class ArchiveBuilder
	{
		private class FileItemRaw
		{
			[JsonProperty("path")]
			public string Path;
			[JsonProperty("offset")]
			public int Offset;
			[JsonProperty("length")]
			public int Length;
		}

		public Stream BuildStream(IReadOnlyCollection<FileData> files)
		{
			var memory = new MemoryStream();
			try
			{
				var fileItems = new List<FileItemRaw>();
				var offset = 0;
				foreach (var file in files)
				{
					var length = file.Buffer.Count();
					fileItems.Add(new FileItemRaw() { Path = file.Path, Offset = offset, Length = length });
					offset += length;
				}

				var metadata = new
				{
					version = 1,
					files = fileItems
				};
				var metadataJson = JToken.FromObject(metadata).ToString(Formatting.None);

				var writer = new BinaryWriter(memory);

				// write magic-number
				writer.Write(Encoding.ASCII.GetBytes("PRA"));

				// write length of the json metadata
				writer.Write(metadataJson.Length);

				// write the json metadata
				writer.Write(Encoding.UTF8.GetBytes(metadataJson));

				// file data
				foreach (var file in files)
				{
					writer.Write(file.Buffer.ToArray());
				}
			}
			catch
			{
				memory.Dispose();
				throw;
			}

			memory.Position = 0;

			return memory;
		}
	}
}
