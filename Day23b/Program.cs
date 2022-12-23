using System.IO;

var elves = new List<(int row, int col)>();
var row = 0;
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	var chars = line.ToCharArray();
	for (var col = 0; col < chars.Length; col++)
	{
		if (chars[col] == '#')
		{
			elves.Add((row, col));
		}
	}
	row++;
}

var direction = Direction.NORTH;
var round = 1;
while (true)
{
	// find new positions
	var newPositions = new List<(int elf, int row, int col)>();
	for (var elfIndex = 0; elfIndex < elves.Count; elfIndex++)
	{
		var elf = elves[elfIndex];
		if (elves.Count(e => Enumerable.Range(elf.row - 1, 3).Contains(e.row) && Enumerable.Range(elf.col - 1, 3).Contains(e.col)) > 1)
		{
			for (var d = 0; d < 4; d++)
			{
				var newPos = GetNewPosition((Direction)(((int)direction + d) % 4), elf);
				if (newPos.valid)
				{
					newPositions.Add((elfIndex, newPos.row, newPos.col));
					break;
				}
			}
		}
	}
	Console.WriteLine($"{round}: {newPositions.Count}");
	if (newPositions.Count == 0)
	{
		break;
	}

	// move elves
	foreach (var newPos in newPositions)
	{
		if (newPositions.Where(p => p.row == newPos.row && p.col == newPos.col).Count() == 1)
		{
			elves[newPos.elf] = (newPos.row, newPos.col);
		}
	}

	//PrintGrid();

	direction = (Direction)(((int)direction + 1) % 4);
	round++;
}

Console.WriteLine(round);

(bool valid, int row, int col) GetNewPosition(Direction direction, (int row, int col) elf)
{
	switch (direction)
	{
		case Direction.NORTH:
			return (!elves.Any(e => e.row == elf.row - 1 && Enumerable.Range(elf.col - 1, 3).Contains(e.col)), elf.row - 1, elf.col);
		case Direction.SOUTH:
			return (!elves.Any(e => e.row == elf.row + 1 && Enumerable.Range(elf.col - 1, 3).Contains(e.col)), elf.row + 1, elf.col);
		case Direction.WEST:
			return (!elves.Any(e => e.col == elf.col - 1 && Enumerable.Range(elf.row - 1, 3).Contains(e.row)), elf.row, elf.col - 1);
		case Direction.EAST:
			return (!elves.Any(e => e.col == elf.col + 1 && Enumerable.Range(elf.row - 1, 3).Contains(e.row)), elf.row, elf.col + 1);
		default:
			throw new InvalidOperationException();
	}
}

void PrintGrid()
{
	var minRow = elves.Select(e => e.row).Min();
	var maxRow = elves.Select(e => e.row).Max();
	var minCol = elves.Select(e => e.col).Min();
	var maxCol = elves.Select(e => e.col).Max();

	for (var row = minRow; row <= maxRow; row++)
	{
		for (var col = minCol; col <= maxCol; col++)
		{
			if (elves.Any(e => e.row == row && e.col == col))
			{
				Console.Write("#");
			}
			else
			{
				Console.Write(".");
			}
		}
		Console.WriteLine();
	}
}

int CountEmptyTiles()
{
	var minRow = elves.Select(e => e.row).Min();
	var maxRow = elves.Select(e => e.row).Max();
	var minCol = elves.Select(e => e.col).Min();
	var maxCol = elves.Select(e => e.col).Max();

	var sum = 0;
	for (var row = minRow; row <= maxRow; row++)
	{
		for (var col = minCol; col <= maxCol; col++)
		{
			if (!elves.Any(e => e.row == row && e.col == col))
			{
				sum++;
			}
		}
	}
	return sum;
}

enum Direction
{
	NORTH = 0,
	SOUTH = 1,
	WEST = 2,
	EAST = 3
}