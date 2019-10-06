using Newtonsoft.Json;

namespace PathResolvableArchive
{
	public class ArchiveItemInfo
	{
		public ArchiveItemInfo() { }
		public ArchiveItemInfo(string path, string offset, string length)
		{
			this.Path = path;
			this.Offset = offset;
			this.Length = length;
		}

		[JsonProperty("path")]
		public string Path { get; }
		[JsonProperty("offset")]
		public string Offset { get; }
		[JsonProperty("length")]
		public string Length { get; }
	}
}
