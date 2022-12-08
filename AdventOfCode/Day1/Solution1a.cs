using System;
using System.Diagnostics.Metrics;

namespace AdventOfCode
{
	public class Solution1a
	{

		public static void doWork()
		{
			var sum = 0;
			var max = 0;
			foreach (string line in System.IO.File.ReadLines(@"../../../Day1/Input.txt"))
			{
				if (!string.IsNullOrEmpty(line))
				{
					sum += int.Parse(line);
				}
				else
				{
					max = Math.Max(max, sum);
					sum = 0;
				}
			}
			max = Math.Max(max, sum);
			Console.WriteLine(max);
		}
	}
}

