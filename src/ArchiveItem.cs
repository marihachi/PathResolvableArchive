using System.Collections.Generic;

namespace PathResolvableArchive
{
	public class ArchiveItem
	{
		public string Path;
		public IEnumerable<byte> Buffer;
	}
}
