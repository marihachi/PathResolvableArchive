using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PathResolvableArchive
{
	public class ArchiveStreamManager
	{
		public static void WriteArchive(Stream stream, IReadOnlyCollection<PathResolvableData> files)
		{
			var metadata = Utility.BuildMetaData(files);
			var metadataJson = JToken.FromObject(metadata).ToString(Formatting.None);

			var writer = new BinaryWriter(stream);

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

		public ArchiveMetaData ReadHeader(Stream stream)
		{
			var reader = new BinaryReader(stream);

			if (Encoding.ASCII.GetString(reader.ReadBytes(3)) != "PRA")
			{
				throw new FormatException("invalid PRA header");
			}

			var metadataLnegth = reader.ReadInt32();
			var metadataBuffer = reader.ReadBytes(metadataLnegth);
			var metadataJson = Encoding.UTF8.GetString(metadataBuffer);
			var metadata = JToken.Parse(metadataJson).ToObject<ArchiveMetaData>();

			return metadata;
		}
	}
}
