using System;
namespace AdventOfCode.Day2
{
	public class SolutionA
	{
		enum Choice
		{
			Rock = 1,
			Paper = 2,
			Scissors = 3
		}

		enum Result
		{
			Lost = 0,
			Draw = 3,
			Win = 6
		}

		public static void doWork()
		{
			var sum = 0;
			foreach (string line in System.IO.File.ReadLines(@"../../../Day2/Input.txt"))
			{
				var choices = line.Split(' ');
				var opponentsChoice = ParseChoice(choices[0]);
				var myChoice = ParseChoice(choices[1]);
				var score = (int)EvaluateChoices(opponentsChoice, myChoice) + (int)myChoice;
				sum += score;
			}
			Console.WriteLine(sum);
		}

		private static Result EvaluateChoices(Choice opponentsChoice, Choice myChoice)
		{
			if (opponentsChoice == myChoice)
			{
				return Result.Draw;
			}
			else if ((int)opponentsChoice % 3 == (int)myChoice - 1)
			{
				return Result.Win;
			}
			else
			{
				return Result.Lost;
			}
		}

		static Choice ParseChoice(String value)
		{
			switch (value)
			{
				case "A":
				case "X":
					return Choice.Rock;
				case "B":
				case "Y":
					return Choice.Paper;
				case "C":
				case "Z":
					return Choice.Scissors;
				default:
					throw new Exception("Invalid value " + value);
			}
		}
	}
}

