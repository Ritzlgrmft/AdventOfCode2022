using System;
using System.Diagnostics;

namespace AdventOfCode.Day9
{
	public class SolutionA
	{
		public static void DoWork()
		{
			var visitedTailPositions = new List<Position>();
			var head = new Position { Row = 0, Column = 0 };
			var tail = new Position { Row = 0, Column = 0 };
			AddTailToVisitedPositions(visitedTailPositions, tail);
			foreach (string line in File.ReadLines(@"../../../Day9/Input.txt"))
			{
				var lineParts = line.Split(' ');
				var direction = lineParts[0];
				var count = int.Parse(lineParts[1]);
				for (var i = 0; i < count; i++)
				{
					head = MoveHead(direction, head);
					tail = MoveTail(head, tail);
					AddTailToVisitedPositions(visitedTailPositions, tail);
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

