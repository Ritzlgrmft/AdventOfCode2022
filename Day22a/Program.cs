// parse input
var mapAsList = new List<char[]>();
List<int> numbers = new List<int>();
List<char> directions = new List<char>();
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	if (line.StartsWith(' ') || line.StartsWith('.') || line.StartsWith('#'))
	{
		mapAsList.Add(line.ToCharArray());
	}
	else if (!string.IsNullOrEmpty(line))
	{
		(numbers, directions) = ParseMovements(line);
	}
}
var map = mapAsList.ToArray();

// find start position
var row = 0;
var col = Array.FindIndex(map[0], c => c == '.');
var direction = Direction.RIGHT;

for (var index = 0; index < numbers.Count; index++)
{
	// move
	switch (direction)
	{
		case Direction.RIGHT:
			col = MoveRight(row, col, numbers[index]);
			break;
		case Direction.DOWN:
			row = MoveDown(row, col, numbers[index]);
			break;
		case Direction.LEFT:
			col = MoveLeft(row, col, numbers[index]);
			break;
		case Direction.UP:
			row = MoveUp(row, col, numbers[index]);
			break;
	}

	// turn
	if (index < directions.Count)
	{
		if (directions[index] == 'R')
		{
			direction = (Direction)(((int)direction + 1) % 4);
		}
		else
		{
			direction = (Direction)(((int)direction + 3) % 4);
		}
	}

	Console.WriteLine($"{row} {col} {direction}");
}

var result = 1000 * (row + 1) + 4 * (col + 1) + direction;
Console.WriteLine(result);





int MoveRight(int row, int col, int number)
{
	int nextCol;
	if (col == map[row].Length - 1 || map[row][col + 1] == ' ')
	{
		nextCol = Array.FindIndex(map[row], c => c != ' ');
	}
	else
	{
		nextCol = col + 1;
	}

	if (map[row][nextCol] == '.')
	{
		if (number == 1)
		{
			return nextCol;
		}
		else
		{
			return MoveRight(row, nextCol, number - 1);
		}
	}
	else
	{
		return col;
	}
}

int MoveLeft(int row, int col, int number)
{
	int nextCol;
	if (col == 0 || map[row][col - 1] == ' ')
	{
		nextCol = Array.FindLastIndex(map[row], c => c != ' ');
	}
	else
	{
		nextCol = col - 1;
	}

	if (map[row][nextCol] == '.')
	{
		if (number == 1)
		{
			return nextCol;
		}
		else
		{
			return MoveLeft(row, nextCol, number - 1);
		}
	}
	else
	{
		return col;
	}
}

int MoveDown(int row, int col, int number)
{
	int nextRow;
	if (row == map.Length - 1 || map[row + 1].Length <= col || map[row + 1][col] == ' ')
	{
		nextRow = 0;
		while (map[nextRow].Length <= col || map[nextRow][col] == ' ')
		{
			nextRow++;
		}
	}
	else
	{
		nextRow = row + 1;
	}

	if (map[nextRow][col] == '.')
	{
		if (number == 1)
		{
			return nextRow;
		}
		else
		{
			return MoveDown(nextRow, col, number - 1);
		}
	}
	else
	{
		return row;
	}
}

int MoveUp(int row, int col, int number)
{
	int nextRow;
	if (row == 0 || map[row - 1].Length <= col || map[row - 1][col] == ' ')
	{
		nextRow = map.Length - 1;
		while (map[nextRow].Length <= col || map[nextRow][col] == ' ')
		{
			nextRow--;
		}
	}
	else
	{
		nextRow = row - 1;
	}

	if (map[nextRow][col] == '.')
	{
		if (number == 1)
		{
			return nextRow;
		}
		else
		{
			return MoveUp(nextRow, col, number - 1);
		}
	}
	else
	{
		return row;
	}
}

(List<int> numbers, List<char> directions) ParseMovements(string line)
{
	var numbers = new List<int>();
	var directions = new List<char>();
	var numberAsString = "";
	foreach (var c in line.ToCharArray())
	{
		if (char.IsNumber(c))
		{
			numberAsString += c;
		}
		else
		{
			numbers.Add(int.Parse(numberAsString));
			numberAsString = "";
			directions.Add(c);
		}
	}
	numbers.Add(int.Parse(numberAsString));
	return (numbers: numbers, directions: directions);
}

enum Direction
{
	RIGHT = 0,
	DOWN = 1,
	LEFT = 2,
	UP = 3
}