using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PathResolvableArchive
{
	public static class ArchiveStreamManager
	{
		public static void WriteArchive(Stream stream, IReadOnlyCollection<BufferWithPathInfo> files)
		{
			var metadata = ArchiveMetaData.Build(files);
			var metadataJson = metadata.ToJson();

			stream.Position = 0;

			var writer = new BinaryWriter(stream);

			// write magic-number (3 bytes)
			writer.Write(Encoding.ASCII.GetBytes("PRA"));

			// write length of the json metadata (4 bytes)
			writer.Write(metadataJson.Length);

			// write the json metadata
			writer.Write(Encoding.UTF8.GetBytes(metadataJson));

			// file data
			foreach (var file in files)
			{
				writer.Write(file.Buffer.ToArray());
			}
		}

		public static ArchiveMetaData ReadHeader(Stream stream)
		{
			stream.Position = 0;

			var reader = new BinaryReader(stream);

			// read magic-number (3 bytes)
			if (Encoding.ASCII.GetString(reader.ReadBytes(3)) != "PRA")
			{
				throw new FormatException("invalid PRA header");
			}

			// read length of the json metadata (4 bytes)
			var metadataLnegth = reader.ReadInt32();

			// read the json metadata
			var metadataBuffer = reader.ReadBytes(metadataLnegth);
			var metadataJson = Encoding.UTF8.GetString(metadataBuffer);
			var metadata = ArchiveMetaData.FromJson(metadataJson);

			return metadata;
		}
	}
}
