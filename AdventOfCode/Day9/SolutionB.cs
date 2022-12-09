using System;
using System.Diagnostics;

namespace AdventOfCode.Day9
{
	public class SolutionB
	{
		public static void DoWork()
		{
			var visitedTailPositions = new List<Position>();
			var knots = new Position[10];
			for (var i = 0; i < 10; i++)
			{
				knots[i] = new Position { Row = 0, Column = 0 };
			}
			AddTailToVisitedPositions(visitedTailPositions, knots[0]);
			foreach (string line in File.ReadLines(@"../../../Day9/Input.txt"))
			{
				var lineParts = line.Split(' ');
				var direction = lineParts[0];
				var count = int.Parse(lineParts[1]);
				for (var i = 0; i < count; i++)
				{
					knots[0] = MoveHead(direction, knots[0]);
					for (var knot = 1; knot < 10; knot++)
					{
						knots[knot] = MoveTail(knots[knot - 1], knots[knot]);
					}
					AddTailToVisitedPositions(visitedTailPositions, knots[9]);
				}
			}
			Console.WriteLine(visitedTailPositions.Count);
		}

		private static void AddTailToVisitedPositions(List<Position> visitedTailPositions, Position tail)
		{
			if (!visitedTailPositions.Contains(tail))
			{
				visitedTailPositions.Add(tail);
			}
		}

		private static Position MoveHead(string direction, Position head)
		{

			switch (direction)
			{
				case "R":
					head.Column++;
					return head;
				case "L":
					head.Column--;
					return head;
				case "D":
					head.Row++;
					return head;
				case "U":
					head.Row--;
					return head;
				default:
					throw new Exception("unknown direction " + direction);
			}
		}

		private static Position MoveTail(Position head, Position tail)
		{
			// same row
			if (head.Row == tail.Row)
			{
				if (head.Column - tail.Column > 1)
				{
					tail.Column = head.Column - 1;
				}
				else if (head.Column - tail.Column < -1)
				{
					tail.Column = head.Column + 1;
				}
			}

			// same column
			else if (head.Column == tail.Column)
			{
				if (head.Row - tail.Row > 1)
				{
					tail.Row = head.Row - 1;
				}
				else if (head.Row - tail.Row < -1)
				{
					tail.Row = head.Row + 1;
				}
			}

			// head is diagonal adjacent
			else if (Math.Abs(head.Row - tail.Row) == 1 && Math.Abs(head.Column - tail.Column) == 1)
			{
				// do nothing
			}

			// head is right up
			else if (head.Column > tail.Column && head.Row < tail.Row)
			{
				tail.Row--;
				tail.Column++;
			}

			// head is left up
			else if (head.Column < tail.Column && head.Row < tail.Row)
			{
				tail.Row--;
				tail.Column--;
			}

			// head is right down
			else if (head.Column > tail.Column && head.Row > tail.Row)
			{
				tail.Row++;
				tail.Column++;
			}

			// head is left down
			else if (head.Column < tail.Column && head.Row > tail.Row)
			{
				tail.Row++;
				tail.Column--;
			}

			return tail;
		}

		struct Position
		{
			public int Row { get; set; }
			public int Column { get; set; }

			override public string ToString()
			{
				return $"{Row} {Column}";
			}
		}
	}
}

