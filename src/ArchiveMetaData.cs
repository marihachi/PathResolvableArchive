using Newtonsoft.Json;
using System.Collections.Generic;

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
	}
}
