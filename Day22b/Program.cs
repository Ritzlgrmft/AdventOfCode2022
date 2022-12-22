const int CUBE_SIZE = 50;

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
	//move
	(row, col, direction) = Move(row, col, numbers[index], direction);
	Console.WriteLine($"Moved to {row} {col} {direction}");

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
		Console.WriteLine($"Turned to {direction}");
	}

}

var result = 1000 * (row + 1) + 4 * (col + 1) + direction;
Console.WriteLine(result);

(int, int, Direction) Move(int row, int col, int number, Direction direction)
{
	switch (direction)
	{
		case Direction.RIGHT:
			return MoveRight(row, col, number);
		case Direction.DOWN:
			return MoveDown(row, col, number);
		case Direction.LEFT:
			return MoveLeft(row, col, number);
		case Direction.UP:
			return MoveUp(row, col, number);
		default:
			throw new InvalidOperationException();
	}
}

(int, int, Direction) MoveRight(int row, int col, int number)
{
	var faceRow = row / CUBE_SIZE;
	var faceCol = col / CUBE_SIZE;
	var rowInCube = row % CUBE_SIZE;
	var colInCube = col % CUBE_SIZE;

	int nextRow, nextCol;
	Direction nextDirection;
	if (colInCube == CUBE_SIZE - 1)
	{
		if (faceRow == 0 && faceCol == 2)
		{
			nextRow = 3 * CUBE_SIZE - 1 - rowInCube;
			nextCol = 2 * CUBE_SIZE - 1;
			nextDirection = Direction.LEFT;
		}
		else if (faceRow == 1 && faceCol == 1)
		{
			nextRow = CUBE_SIZE - 1;
			nextCol = 2 * CUBE_SIZE + rowInCube;
			nextDirection = Direction.UP;
		}
		else if (faceRow == 2 && faceCol == 1)
		{
			nextRow = CUBE_SIZE - 1 - rowInCube;
			nextCol = 3 * CUBE_SIZE - 1;
			nextDirection = Direction.LEFT;
		}
		else if (faceRow == 3 && faceCol == 0)
		{
			nextRow = 3 * CUBE_SIZE - 1;
			nextCol = CUBE_SIZE + rowInCube;
			nextDirection = Direction.UP;
		}
		else
		{
			nextRow = row;
			nextCol = col + 1;
			nextDirection = Direction.RIGHT;
		}
	}
	else
	{
		nextRow = row;
		nextCol = col + 1;
		nextDirection = Direction.RIGHT;
	}

	if (map[nextRow][nextCol] == '.')
	{
		if (number == 1)
		{
			return (nextRow, nextCol, nextDirection);
		}
		else
		{
			return Move(nextRow, nextCol, number - 1, nextDirection);
		}
	}
	else
	{
		return (row, col, Direction.RIGHT);
	}
}

(int, int, Direction) MoveLeft(int row, int col, int number)
{
	var faceRow = row / CUBE_SIZE;
	var faceCol = col / CUBE_SIZE;
	var rowInCube = row % CUBE_SIZE;
	var colInCube = col % CUBE_SIZE;

	int nextRow, nextCol;
	Direction nextDirection;
	if (colInCube == 0)
	{
		if (faceRow == 0 && faceCol == 1)
		{
			nextRow = 3 * CUBE_SIZE - 1 - rowInCube;
			nextCol = 0;
			nextDirection = Direction.RIGHT;
		}
		else if (faceRow == 1 && faceCol == 1)
		{
			nextRow = 2 * CUBE_SIZE;
			nextCol = rowInCube;
			nextDirection = Direction.DOWN;
		}
		else if (faceRow == 2 && faceCol == 0)
		{
			nextRow = CUBE_SIZE - 1 - rowInCube;
			nextCol = CUBE_SIZE;
			nextDirection = Direction.RIGHT;
		}
		else if (faceRow == 3 && faceCol == 0)
		{
			nextRow = 0;
			nextCol = CUBE_SIZE + rowInCube;
			nextDirection = Direction.DOWN;
		}
		else
		{
			nextRow = row;
			nextCol = col - 1;
			nextDirection = Direction.LEFT;
		}

	}
	else
	{
		nextRow = row;
		nextCol = col - 1;
		nextDirection = Direction.LEFT;
	}

	if (map[nextRow][nextCol] == '.')
	{
		if (number == 1)
		{
			return (nextRow, nextCol, nextDirection);
		}
		else
		{
			return Move(nextRow, nextCol, number - 1, nextDirection);
		}
	}
	else
	{
		return (row, col, Direction.LEFT);
	}
}

(int, int, Direction) MoveDown(int row, int col, int number)
{
	var faceRow = row / CUBE_SIZE;
	var faceCol = col / CUBE_SIZE;
	var rowInCube = row % CUBE_SIZE;
	var colInCube = col % CUBE_SIZE;

	int nextRow, nextCol;
	Direction nextDirection;
	if (rowInCube == CUBE_SIZE - 1)
	{
		if (faceRow == 0 && faceCol == 2)
		{
			nextRow = CUBE_SIZE + colInCube;
			nextCol = 2 * CUBE_SIZE - 1;
			nextDirection = Direction.LEFT;
		}
		else if (faceRow == 2 && faceCol == 1)
		{
			nextRow = 3 * CUBE_SIZE + colInCube;
			nextCol = CUBE_SIZE - 1;
			nextDirection = Direction.LEFT;
		}
		else if (faceRow == 3 && faceCol == 0)
		{
			nextRow = 0;
			nextCol = 2 * CUBE_SIZE + colInCube;
			nextDirection = Direction.DOWN;
		}
		else
		{
			nextRow = row + 1;
			nextCol = col;
			nextDirection = Direction.DOWN;
		}
	}
	else
	{
		nextRow = row + 1;
		nextCol = col;
		nextDirection = Direction.DOWN;
	}

	if (map[nextRow][nextCol] == '.')
	{
		if (number == 1)
		{
			return (nextRow, nextCol, nextDirection);
		}
		else
		{
			return Move(nextRow, nextCol, number - 1, nextDirection);
		}
	}
	else
	{
		return (row, col, Direction.DOWN);
	}
}

(int, int, Direction) MoveUp(int row, int col, int number)
{
	var faceRow = row / CUBE_SIZE;
	var faceCol = col / CUBE_SIZE;
	var rowInCube = row % CUBE_SIZE;
	var colInCube = col % CUBE_SIZE;

	int nextRow, nextCol;
	Direction nextDirection;
	if (rowInCube == 0)
	{
		if (faceRow == 0 && faceCol == 1)
		{
			nextRow = 3 * CUBE_SIZE + colInCube;
			nextCol = 0;
			nextDirection = Direction.RIGHT;
		}
		else if (faceRow == 0 && faceCol == 2)
		{
			nextRow = 4 * CUBE_SIZE - 1;
			nextCol = col - 2 * CUBE_SIZE;
			nextDirection = Direction.UP;
		}
		else if (faceRow == 2 && faceCol == 0)
		{
			nextRow = CUBE_SIZE + colInCube;
			nextCol = CUBE_SIZE;
			nextDirection = Direction.RIGHT;
		}
		else
		{
			nextRow = row - 1;
			nextCol = col;
			nextDirection = Direction.UP;
		}
	}
	else
	{
		nextRow = row - 1;
		nextCol = col;
		nextDirection = Direction.UP;
	}

	if (map[nextRow][nextCol] == '.')
	{
		if (number == 1)
		{
			return (nextRow, nextCol, nextDirection);
		}
		else
		{
			return Move(nextRow, nextCol, number - 1, nextDirection);
		}
	}
	else
	{
		return (row, col, Direction.UP);
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
