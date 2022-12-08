using System;
using System.Diagnostics.Metrics;

namespace AdventOfCode
{
	public class Solution1b
	{

		public static void doWork()
		{
			var sum = 0;
			int[] max = { 0, 0, 0 };
			foreach (string line in System.IO.File.ReadLines(@"../../../Day1/Input.txt"))
			{
				if (!string.IsNullOrEmpty(line))
				{
					sum += int.Parse(line);
				}
				else
				{
					max[0] = Math.Max(max[0], sum);
					Array.Sort(max);
					sum = 0;
				}
			}
			max[2] = Math.Max(max[2], sum);
			Console.WriteLine(max[0] + max[1] + max[2]);
		}
	}
}

