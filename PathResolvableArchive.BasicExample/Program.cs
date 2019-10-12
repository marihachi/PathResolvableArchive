using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathResolvableArchive.BasicExample
{
	public class Program
	{
		public static void Help()
		{
			Console.WriteLine("PathResolvableArchive.Example");
			Console.WriteLine("Usage:");
			Console.WriteLine("generate <dirPath>");
			Console.WriteLine("metadata <filePath>");
		}

		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Help();
				return;
			}

			var command = args[0];

			if (command == "generate")
			{
				if (args.Length < 2)
				{
					Help();
					return;
				}

				var baseDirInfo = new DirectoryInfo(args[1]);
				if (!baseDirInfo.Exists)
				{
					Console.WriteLine("base directory is not found");
					return;
				}
				Console.WriteLine($"base directory: {baseDirInfo.FullName}");

				var archiveItems = new List<ArchiveItem>();
				var outputFilePath = Path.GetFullPath(Path.Combine(baseDirInfo.FullName, "..", $"{baseDirInfo.Name}.pra"));

				foreach (var fileInfo in baseDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
				{
					// path
					string relativeFilePath = FsUtil.RelativePath(baseDirInfo.FullName, fileInfo.FullName);

					// buffer
					byte[] buffer;
					using (var fileStream = fileInfo.OpenRead())
					{
						buffer = new byte[fileStream.Length];
						fileStream.Read(buffer, 0, buffer.Length);
					}

					Console.WriteLine($"* Path=\"{relativeFilePath}\", Length={buffer.Length}");
					archiveItems.Add(new ArchiveItem(relativeFilePath, buffer));
				}

				using (var fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write))
				{
					ArchiveManager.Write(fileStream, archiveItems);
				}
			}
			else if (command == "metadata")
			{
				if (args.Length < 2)
				{
					Help();
					return;
				}

				var archiveFileInfo = new FileInfo(args[1]);
				if (!archiveFileInfo.Exists)
				{
					Console.WriteLine("file is not found");
					return;
				}
				Console.WriteLine($"target file: {archiveFileInfo.FullName}");

				ArchiveMetaData metadata;

				using (var fileStream = archiveFileInfo.OpenRead())
				{
					metadata = ArchiveManager.ReadMetaData(fileStream);
				}

				Console.WriteLine(metadata.Raw.ToJson());
			}
			else
			{
				Help();
			}
		}
	}
}
