using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace PathResolvableArchive
{
	public class ArchiveMetaData
	{
		public ArchiveMetaData(ArchiveMetaDataRaw metadataRaw, int payloadBeginOffset)
		{
			this.Raw = metadataRaw;
			this.PayloadBeginOffset = payloadBeginOffset;
		}

		public ArchiveMetaDataRaw Raw { get; set; }
		public int PayloadBeginOffset { get; set; }
	}

	public class ArchiveMetaDataRaw
	{
		public ArchiveMetaDataRaw() { }
		public ArchiveMetaDataRaw(int version, IReadOnlyList<ArchiveItemInfo> itemInfos)
		{
			this.Version = version;
			this.ItemInfos = itemInfos;
		}

		[JsonProperty("version")]
		public int Version { get; }
		[JsonProperty("files")]
		public IReadOnlyList<ArchiveItemInfo> ItemInfos { get; }

		public static ArchiveMetaDataRaw Build(IReadOnlyCollection<ArchiveItem> items)
		{
			var fileDataInfos = new List<ArchiveItemInfo>();
			var offset = 0;
			foreach (var item in items)
			{
				var length = item.Buffer.Count();
				fileDataInfos.Add(new ArchiveItemInfo(item.Path, offset.ToString(), length.ToString()));
				offset += length;
			}

			return new ArchiveMetaDataRaw(1, fileDataInfos);
		}

		public string ToJson()
		{
			return JToken.FromObject(this).ToString(Formatting.None);
		}

		public static ArchiveMetaDataRaw FromJson(string json)
		{
			return JToken.Parse(json).ToObject<ArchiveMetaDataRaw>();
		}
	}
}
