using System;
namespace AdventOfCode.Day3
{
	public class SolutionB
	{
		public static void DoWork()
		{
			var sum = 0;
			foreach (var lines in System.IO.File.ReadLines(@"../../../Day3/Input.txt").Chunk(3))
			{
				var intersect = lines[0].ToCharArray().Intersect(lines[1].ToCharArray()).Intersect(lines[2].ToCharArray()).First();
				var score = GetScore(intersect);
				sum += score;
			}
			Console.WriteLine(sum);
		}

		static int GetScore(char c)
		{
			if (c >= 'a' && c <= 'z')
			{
				return (int)c - (int)'a' + 1;
			}
			else if (c >= 'A' && c <= 'Z')
			{
				return (int)c - (int)'A' + 27;
			}
			else
			{
				throw new Exception("Invalid character " + c);
			}
		}

	}
}

