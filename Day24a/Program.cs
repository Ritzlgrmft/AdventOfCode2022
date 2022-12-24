using System.IO;

int valleyWidth = 0, valleyHeight;
var blizzards = new List<Blizzard>();
var row = 0;
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	valleyWidth = line.Length - 2;
	var chars = line.ToCharArray();
	for (var col = 1; col < chars.Length - 1; col++)
	{
		if (chars[col] == '>' || chars[col] == 'v' || chars[col] == '<' || chars[col] == '^')
		{
			blizzards.Add(new Blizzard(row - 1, col - 1, chars[col]));
		}
	}
	row++;
}
valleyHeight = row - 2;

var positions = new List<(int row, int col)>() { (-1, 0) };
var goalReached = false;
var minute = 1;
while (!goalReached)
{
	// move blizzards
	foreach (var blizzard in blizzards)
	{
		switch (blizzard.Direction)
		{
			case '>':
				blizzard.Col = (blizzard.Col + 1) % valleyWidth;
				break;
			case '<':
				blizzard.Col = (blizzard.Col - 1 + valleyWidth) % valleyWidth;
				break;
			case 'v':
				blizzard.Row = (blizzard.Row + 1) % valleyHeight;
				break;
			case '^':
				blizzard.Row = (blizzard.Row - 1 + valleyHeight) % valleyHeight;
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	// check positions
	var newPositions = new List<(int row, int col)>();
	foreach (var pos in positions)
	{
		if (pos.row == valleyHeight - 1 && pos.col == valleyWidth - 1)
		{
			goalReached = true;
		}
		else
		{
			var potentialPositions = new List<(int row, int col)>() { pos };
			if (pos.col > 0 && pos.row >= 0)
			{
				potentialPositions.Add((pos.row, pos.col - 1));
			}
			if (pos.col < valleyWidth - 1 && pos.row >= 0)
			{
				potentialPositions.Add((pos.row, pos.col + 1));
			}
			if (pos.row > 0)
			{
				potentialPositions.Add((pos.row - 1, pos.col));
			}
			if (pos.row < valleyHeight - 1)
			{
				potentialPositions.Add((pos.row + 1, pos.col));
			}
			newPositions.AddRange(potentialPositions.Where(p => !blizzards.Any(b => b.Row == p.row && b.Col == p.col)));
		}
	}
	positions = newPositions;

	Console.WriteLine($"End of minute {minute}: {positions.Count} positions");
	minute++;
}

class Blizzard
{
	public int Row;
	public int Col;
	public char Direction;

	public Blizzard(int row, int col, char direction)
	{
		Row = row;
		Col = col;
		Direction = direction;
	}
}