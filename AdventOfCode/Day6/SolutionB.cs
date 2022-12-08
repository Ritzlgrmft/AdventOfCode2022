using System;
namespace AdventOfCode.Day6
{
	public class SolutionB
	{
		public static void DoWork()
		{
			var data = System.IO.File.ReadLines(@"../../../Day6/Input.txt").First();
			var start = 0;
			while (true)
			{
				var marker = data.Substring(start, 14).ToCharArray();
				var grouped = marker.GroupBy(c => c);
				if (grouped.Count() == 14)
				{
					break;
				}
				start++;
			}
			Console.WriteLine(start + 14);
		}
	}
}

