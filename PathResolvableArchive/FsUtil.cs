using System;

namespace PathResolvableArchive
{
	public static class FsUtil
	{
		public static string RelativePath(string basePath, string targetPath)
		{
			var baseUri = new Uri(basePath);
			var targetUri = new Uri(targetPath);
			var relativeUri = baseUri.MakeRelativeUri(targetUri);
			var relativePath = relativeUri.ToString().Replace('/', '\\');

			return relativePath;
		}
	}
}
