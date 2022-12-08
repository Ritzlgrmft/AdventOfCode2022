using System;
namespace AdventOfCode.Day3
{
	public class SolutionA
	{
		public static void DoWork()
		{
			var sum = 0;
			foreach (string line in System.IO.File.ReadLines(@"../../../Day3/Input.txt"))
			{
				var part1 = line.Substring(0, line.Length / 2);
				var part2 = line.Substring(line.Length / 2);
				var intersect = part1.ToCharArray().Intersect(part2.ToCharArray()).First();
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

