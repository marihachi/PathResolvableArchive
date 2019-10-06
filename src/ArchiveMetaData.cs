using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace PathResolvableArchive
{
	public class ArchiveMetaData
	{
		[JsonProperty("version")]
		public int Version;
		[JsonProperty("files")]
		public IEnumerable<FileDataInfo> Files;

		public class FileDataInfo
		{
			[JsonProperty("path")]
			public string Path;
			[JsonProperty("offset")]
			public string Offset;
			[JsonProperty("length")]
			public string Length;
		}

		public static ArchiveMetaData Build(IReadOnlyCollection<BufferWithPathInfo> files)
		{
			var fileDataInfos = new List<FileDataInfo>();
			var offset = 0;
			foreach (var file in files)
			{
				var length = file.Buffer.Count();
				fileDataInfos.Add(new FileDataInfo() { Path = file.Path, Offset = offset.ToString(), Length = length.ToString() });
				offset += length;
			}

			return new ArchiveMetaData()
			{
				Version = 1,
				Files = fileDataInfos
			};
		}

		public string ToJson()
		{
			return JToken.FromObject(this).ToString(Formatting.None);
		}

		public static ArchiveMetaData FromJson(string json)
		{
			return JToken.Parse(json).ToObject<ArchiveMetaData>();
		}
	}
}
