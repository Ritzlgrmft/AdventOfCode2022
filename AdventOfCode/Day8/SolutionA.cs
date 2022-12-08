using System;
namespace AdventOfCode.Day8
{
	public class SolutionA
	{
		public static void DoWork()
		{
			var trees = new int[99, 99];
			var row = 0;
			foreach (string line in File.ReadLines(@"../../../Day8/Input.txt"))
			{
				var column = 0;
				foreach (var height in line.ToCharArray().Select(c => int.Parse(new String(c, 1))))
				{
					trees[row, column] = height;
					column++;
				}
				row++;
			}

			var sum = 0;
			for (row = 0; row < trees.GetLength(0); row++)
			{
				for (var column = 0; column < trees.GetLength(1); column++)
				{
					if (IsTreeVisible(trees, row, column))
					{
						sum++;
					}
				}
			}
			Console.WriteLine(sum);
		}

		static bool IsTreeVisible(int[,] trees, int row, int column)
		{
			var visibleFromTop = true;
			for (var i = 0; i < row; i++)
			{
				if (trees[i, column] >= trees[row, column])
				{
					visibleFromTop = false;
				}
			}

			var visibleFromBottom = true;
			for (var i = row + 1; i < trees.GetLength(0); i++)
			{
				if (trees[i, column] >= trees[row, column])
				{
					visibleFromBottom = false;
				}
			}

			var visibleFromLeft = true;
			for (var i = 0; i < column; i++)
			{
				if (trees[row, i] >= trees[row, column])
				{
					visibleFromLeft = false;
				}
			}

			var visibleFromRight = true;
			for (var i = column + 1; i < trees.GetLength(1); i++)
			{
				if (trees[row, i] >= trees[row, column])
				{
					visibleFromRight = false;
				}
			}

			return visibleFromTop || visibleFromBottom || visibleFromLeft || visibleFromRight;
		}
	}
}

