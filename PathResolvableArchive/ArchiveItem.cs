using System.Collections.Generic;

namespace PathResolvableArchive
{
	public class ArchiveItem
	{
		public ArchiveItem(string path, IReadOnlyList<byte> buffer)
		{
			this.Path = path;
			this.Buffer = buffer;
		}

		public string Path { get; set; }
		public IReadOnlyList<byte> Buffer { get; set; }
	}
}
