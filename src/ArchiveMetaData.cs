using Newtonsoft.Json;
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
			public int Offset;
			[JsonProperty("length")]
			public int Length;
		}

		public static ArchiveMetaData Build(IReadOnlyCollection<PathResolvableData> files)
		{
			var fileDataInfos = new List<FileDataInfo>();
			var offset = 0;
			foreach (var file in files)
			{
				var length = file.Buffer.Count();
				fileDataInfos.Add(new FileDataInfo() { Path = file.Path, Offset = offset, Length = length });
				offset += length;
			}

			return new ArchiveMetaData()
			{
				Version = 1,
				Files = fileDataInfos
			};
		}
	}
}
