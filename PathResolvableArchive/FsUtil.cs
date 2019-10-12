using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PathResolvableArchive
{
	public static class FsUtil
	{
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
		private static extern bool PathRelativePathTo(
			[Out] StringBuilder pszPath,
			[In] string pszFrom, [In] FileAttributes dwAttrFrom, [In] string pszTo, [In] FileAttributes dwAttrTo
		);

		private const int MAX_PATH = 260;

		public static bool TryBuildRelativePath(DirectoryInfo baseDir, DirectoryInfo targetDir, out string result)
		{
			var sb = new StringBuilder(MAX_PATH);
			var success = PathRelativePathTo(sb, baseDir.FullName, baseDir.Attributes, targetDir.FullName, targetDir.Attributes);
			if (!success)
			{
				result = null;
				return false;
			}
			result = sb.ToString();
			return true;
		}

		public static bool TryBuildRelativePath(DirectoryInfo baseDir, FileInfo targetFile, out string result)
		{
			var sb = new StringBuilder(MAX_PATH);
			var success = PathRelativePathTo(sb, baseDir.FullName, baseDir.Attributes, targetFile.FullName, targetFile.Attributes);
			if (!success)
			{
				result = null;
				return false;
			}
			result = sb.ToString();
			return true;
		}
	}
}
