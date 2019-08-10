using System.Collections.Generic;
using System.Linq;

namespace PathResolvableArchive
{
	public class Utility
	{
		public static ArchiveMetaData BuildMetaData(IReadOnlyCollection<PathResolvableData> files)
		{
			var fileDataInfos = new List<ArchiveMetaData.FileDataInfo>();
			var offset = 0;
			foreach (var file in files)
			{
				var length = file.Buffer.Count();
				fileDataInfos.Add(new ArchiveMetaData.FileDataInfo() { Path = file.Path, Offset = offset, Length = length });
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
