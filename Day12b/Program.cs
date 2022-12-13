// read input
var heightsList = new List<char[]>();
foreach (string line in File.ReadLines(@"../../../Input.txt"))
{
	heightsList.Add(line.ToCharArray());
}
var heights = heightsList.ToArray();

// find start and target
Position target = new Position();
var startPositions = new List<Position>();
for (var row = 0; row < heights.Length; row++)
{
	for (var col = 0; col < heights[row].Length; col++)
	{
		if (heights[row][col] == 'S' || heights[row][col] == 'a')
		{
			startPositions.Add(new Position { Row = row, Column = col });
			heights[row][col] = 'a';
		}
		else if (heights[row][col] == 'E')
		{
			target = new Position { Row = row, Column = col };
			heights[row][col] = 'z';
		}
	}
}

//var stepsMin = int.MaxValue;
//foreach(var start in startPositions)
//{
//	stepsMin = CalculateSteps(start, stepsMin);
//}
Console.WriteLine(startPositions.Select(p => CalculateSteps(p)).Min());

int CalculateSteps(Position start)
{
	var positionsToEvaluate = new List<Position>() { start };
	var evaluatedPositions = new List<Position>();
	var targetReached = false;
	var step = 0;
	while (!targetReached)
	{
		var newPositionsToEvaluate = new List<Position>();
		foreach (var pos in positionsToEvaluate)
		{
			if (pos.Row == target.Row && pos.Column == target.Column)
			{
				targetReached = true;
			}

			if (pos.Row > 0)
			{
				var nextPos = new Position { Row = pos.Row - 1, Column = pos.Column };
				if (IsPositionPossible(evaluatedPositions, pos, nextPos) && !newPositionsToEvaluate.Contains(nextPos))
				{
					newPositionsToEvaluate.Add(nextPos);
				}
			}
			if (pos.Row < heights.Length - 1)
			{
				var nextPos = new Position { Row = pos.Row + 1, Column = pos.Column };
				if (IsPositionPossible(evaluatedPositions, pos, nextPos) && !newPositionsToEvaluate.Contains(nextPos))
				{
					newPositionsToEvaluate.Add(nextPos);
				}
			}
			if (pos.Column > 0)
			{
				var nextPos = new Position { Row = pos.Row, Column = pos.Column - 1 };
				if (IsPositionPossible(evaluatedPositions, pos, nextPos) && !newPositionsToEvaluate.Contains(nextPos))
				{
					newPositionsToEvaluate.Add(nextPos);
				}
			}
			if (pos.Column < heights[pos.Row].Length - 1)
			{
				var nextPos = new Position { Row = pos.Row, Column = pos.Column + 1 };
				if (IsPositionPossible(evaluatedPositions, pos, nextPos) && !newPositionsToEvaluate.Contains(nextPos))
				{
					newPositionsToEvaluate.Add(nextPos);
				}
			}
			evaluatedPositions.Add(pos);
		}
		positionsToEvaluate = newPositionsToEvaluate;
		if (positionsToEvaluate.Count == 0)
		{
			targetReached = true;
			step = int.MaxValue;
		}
		if (!targetReached)
		{
			step++;
		}
	}

	Console.WriteLine($"{start.ToString()}: {step}");
	return step;
}

bool IsPositionPossible(List<Position> evaluatedPositions, Position pos, Position nextPos)
{
	if (evaluatedPositions.Contains(nextPos))
	{
		return false;
	}
	return heights[nextPos.Row][nextPos.Column] <= heights[pos.Row][pos.Column] + 1;
}

struct Position
{
	public int Row { get; set; }
	public int Column { get; set; }

	public string ToString()
	{
		return $"{Row}-{Column}";
	}
}

