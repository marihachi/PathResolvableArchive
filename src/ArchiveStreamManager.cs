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

			using(var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
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
		}

		public static List<BufferWithPathInfo> ReadArchive(Stream stream)
		{
			var result = new List<BufferWithPathInfo>();

			stream.Position = 0;

			using(var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				/* reading header */

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

				/* reading payload */

				var baseOffset = (int)stream.Position;

				if (metadata.Version == 1)
				{
					foreach (var file in metadata.Files)
					{
						stream.Position = baseOffset + int.Parse(file.Offset);
						var buffer = reader.ReadBytes(int.Parse(file.Length));
						result.Add(new BufferWithPathInfo { Buffer = buffer, Path = file.Path });
					}
				}
				else
				{
					throw new FormatException("format version is not supported");
				}
			}

			return result;
		}
	}
}
