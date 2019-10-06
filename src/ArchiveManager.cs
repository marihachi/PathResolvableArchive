using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PathResolvableArchive
{
	public static class ArchiveManager
	{
		public static void Write(Stream stream, IReadOnlyCollection<ArchiveItem> items)
		{
			var metadataRaw = ArchiveMetaDataRaw.Build(items);
			var metadataJson = metadataRaw.ToJson();

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
				foreach (var file in items)
				{
					writer.Write(file.Buffer.ToArray());
				}
			}
		}

		public static ArchiveMetaData ReadMetaData(Stream stream)
		{
			stream.Position = 0;

			using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				// read magic-number (3 bytes)
				const int magicNumberSize = 3;
				if (Encoding.ASCII.GetString(reader.ReadBytes(magicNumberSize)) != "PRA")
				{
					throw new FormatException("invalid PRA header");
				}

				// read length of the json metadata (4 bytes)
				const int metadataLnegthSize = 4;
				var metadataLnegth = reader.ReadInt32();

				// read the json metadata
				var metadataBuffer = reader.ReadBytes(metadataLnegth);
				var metadataJson = Encoding.UTF8.GetString(metadataBuffer);
				var metadataRaw = ArchiveMetaDataRaw.FromJson(metadataJson);

				if (metadataRaw.Version != 1)
				{
					throw new FormatException("format version is not supported");
				}

				return new ArchiveMetaData(metadataRaw, magicNumberSize + metadataLnegthSize + metadataLnegth);
			}
		}

		public static ArchiveItem ReadItem(Stream stream, ArchiveMetaData metadata, ArchiveItemInfo itemInfo)
		{
			using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				if (metadata.Raw.Version == 1)
				{
					// read a file data
					stream.Position = metadata.PayloadBeginOffset + int.Parse(itemInfo.Offset);
					var buffer = reader.ReadBytes(int.Parse(itemInfo.Length));

					return new ArchiveItem { Buffer = buffer, Path = itemInfo.Path };
				}
				else
				{
					throw new FormatException("format version is not supported");
				}
			}
		}

		public static List<ArchiveItem> ReadItems(Stream stream, ArchiveMetaData metadata)
		{
			var result = new List<ArchiveItem>();

			using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				if (metadata.Raw.Version == 1)
				{
					foreach (var file in metadata.Raw.ItemInfos)
					{
						// read a file data
						stream.Position = metadata.PayloadBeginOffset + int.Parse(file.Offset);
						var buffer = reader.ReadBytes(int.Parse(file.Length));

						result.Add(new ArchiveItem { Buffer = buffer, Path = file.Path });
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
