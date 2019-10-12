using Newtonsoft.Json;

namespace PathResolvableArchive
{
	public class ArchiveItemInfo
	{
		public ArchiveItemInfo(string path, string offset, string length)
		{
			this.Path = path;
			this.Offset = offset;
			this.Length = length;
		}

		[JsonProperty("path")]
		public string Path { get; set; }
		[JsonProperty("offset")]
		public string Offset { get; set; }
		[JsonProperty("length")]
		public string Length { get; set; }
	}
}
