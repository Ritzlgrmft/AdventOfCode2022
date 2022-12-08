using System;
namespace AdventOfCode.Day4
{
	public class SolutionA
	{
		public static void DoWork()
		{
			var sum = 0;
			foreach (string line in System.IO.File.ReadLines(@"../../../Day4/Input.txt"))
			{
				var lineParts = line.Split('-', ',');
				var range1 = CreateRange(int.Parse(lineParts[0]), int.Parse(lineParts[1]));
				var range2 = CreateRange(int.Parse(lineParts[2]), int.Parse(lineParts[3]));
				var score = Contains(range1, range2) ? 1 : 0;
				sum += score;
			}
			Console.WriteLine(sum);
		}

		static IEnumerable<int> CreateRange(int start, int end)
		{
			return Enumerable.Range(start, end - start + 1);
		}

		static bool Contains(IEnumerable<int> list1, IEnumerable<int> list2)
		{
			var intersetCount = list1.Intersect(list2).Count();
			return intersetCount == list1.Count() || intersetCount == list2.Count();
		}
	}
}

