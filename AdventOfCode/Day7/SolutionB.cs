using System;
namespace AdventOfCode.Day7
{
	public class SolutionB
	{
		static Directory root = new Directory();

		public static void DoWork()
		{
			Directory currentDirectory = root;
			foreach (string line in System.IO.File.ReadLines(@"../../../Day7/Input.txt"))
			{
				var lineParts = line.Split(' ');
				if (lineParts[0] == "$")
				{
					currentDirectory = ParseCommand(lineParts, currentDirectory);
				}
				else if (lineParts[0] == "dir")
				{
					currentDirectory.SubDirectories.Add(new Directory { Name = lineParts[1], Parent = currentDirectory });
				}
				else
				{
					currentDirectory.Files.Add(new File { Name = lineParts[1], Size = int.Parse(lineParts[0]) });
				}
			}

			var directoySizes = new List<int>();
			int totalSize = CalculateDirectorySize(directoySizes, root);
			int freeSpace = 70000000 - totalSize;
			int spaceToBeFreed = 30000000 - freeSpace;

			Console.WriteLine(directoySizes.Where(s => s >= spaceToBeFreed).Min());
		}

		static Directory ParseCommand(String[] lineParts, Directory currentDirectory)
		{
			if (lineParts[1] == "cd")
			{
				return ParseCommandCd(lineParts, currentDirectory);
			}
			else
			{
				return currentDirectory;
			}
		}

		static Directory ParseCommandCd(String[] lineParts, Directory currentDirectory)
		{
			if (lineParts[2] == "/")
			{
				return root;
			}
			else if (lineParts[2] == "..")
			{
				return currentDirectory.Parent;
			}
			else
			{
				var newDirectory = currentDirectory.SubDirectories.First(d => d.Name == lineParts[2]);
				if (newDirectory == null)
				{
					throw new Exception("unknown sub directory " + lineParts[2]);
				}
				return newDirectory;
			}
		}

		static int CalculateDirectorySize(List<int> directoySizes, Directory currentDirectory)
		{
			int size = currentDirectory.Files.Sum(f => f.Size);
			foreach (var subDirectory in currentDirectory.SubDirectories)
			{
				size += CalculateDirectorySize(directoySizes, subDirectory);
			}

			directoySizes.Add(size);

			return size;
		}

		class File
		{
			public string Name { get; set; }
			public int Size { get; set; }
		}

		class Directory
		{
			public Directory()
			{
				Files = new List<File>();
				SubDirectories = new List<Directory>();
			}
			public String Name { get; set; }
			public List<File> Files { get; }
			public List<Directory> SubDirectories { get; }
			public Directory? Parent { get; set; }
		}
	}
}

