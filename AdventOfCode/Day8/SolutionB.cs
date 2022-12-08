using System;
namespace AdventOfCode.Day8
{
	public class SolutionB
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

			var bestScenicScore = 0;
			for (row = 0; row < trees.GetLength(0); row++)
			{
				for (var column = 0; column < trees.GetLength(1); column++)
				{
					bestScenicScore = Math.Max(bestScenicScore, GetScenicScore(trees, row, column));
				}
			}
			Console.WriteLine(bestScenicScore);
		}

		static int GetScenicScore(int[,] trees, int row, int column)
		{
			var scoreFromTop = 0;
			for (var i = row - 1; i >= 0; i--)
			{
				scoreFromTop++;
				if (trees[i, column] >= trees[row, column])
				{
					break;
				}
			}

			var scoreFromBottom = 0;
			for (var i = row + 1; i < trees.GetLength(0); i++)
			{
				scoreFromBottom++;
				if (trees[i, column] >= trees[row, column])
				{
					break;
				}
			}

			var scoreFromLeft = 0;
			for (var i = column - 1; i >= 0; i--)
			{
				scoreFromLeft++;
				if (trees[row, i] >= trees[row, column])
				{
					break;
				}
			}

			var scoreFromRight = 0;
			for (var i = column + 1; i < trees.GetLength(1); i++)
			{
				scoreFromRight++;
				if (trees[row, i] >= trees[row, column])
				{
					break;
				}
			}

			return scoreFromTop * scoreFromBottom * scoreFromLeft * scoreFromRight;
		}
	}
}

