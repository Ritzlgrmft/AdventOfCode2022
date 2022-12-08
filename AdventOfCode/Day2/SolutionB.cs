using System;
namespace AdventOfCode.Day2
{
	public class SolutionB
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
				var result = ParseResult(choices[1]);
				var myChoice = CalculateMyChoice(opponentsChoice, result);
				var score = (int)result + (int)myChoice;
				sum += score;
			}
			Console.WriteLine(sum);
		}

		private static Choice CalculateMyChoice(Choice opponentsChoice, Result result)
		{
			switch (result)
			{
				case Result.Draw:
					return opponentsChoice;
				case Result.Lost:
					return decreaseChoice(opponentsChoice);
				case Result.Win:
					return increaseChoice(opponentsChoice);
				default:
					throw new Exception("Invalid opponentsChoice " + opponentsChoice);
			}
		}

		static Choice increaseChoice(Choice oldChoice)
		{
			var newChoice = ((int)oldChoice) + 1;
			if (newChoice > 3)
			{
				newChoice = 1;
			}
			return (Choice)newChoice;
		}

		static Choice decreaseChoice(Choice oldChoice)
		{
			var newChoice = ((int)oldChoice) - 1;
			if (newChoice == 0)
			{
				newChoice = 3;
			}
			return (Choice)newChoice;
		}

		static Choice ParseChoice(String value)
		{
			switch (value)
			{
				case "A":
					return Choice.Rock;
				case "B":
					return Choice.Paper;
				case "C":
					return Choice.Scissors;
				default:
					throw new Exception("Invalid value " + value);
			}
		}

		static Result ParseResult(String value)
		{
			switch (value)
			{
				case "X":
					return Result.Lost;
				case "Y":
					return Result.Draw;
				case "Z":
					return Result.Win;
				default:
					throw new Exception("Invalid value " + value);
			}
		}
	}
}

