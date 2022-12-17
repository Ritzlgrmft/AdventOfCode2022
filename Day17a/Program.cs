const int AIR = 0;
const int ROCK = 1;

// get jet pattern
var jetPattern = File.ReadLines(@"../../../Input.txt").First().ToCharArray();
var jetIndex = 0;

var rocks = new[]
{
	new [,]
	{
		{ ROCK, ROCK, ROCK, ROCK }
	},
	new [,]
	{
		{ AIR, ROCK, AIR },
		{ ROCK, ROCK, ROCK },
		{ AIR, ROCK, AIR }
	},
	new [,]
	{
		{ ROCK, ROCK, ROCK },
		{ AIR, AIR, ROCK },
		{ AIR, AIR, ROCK }
	},
	new [,]
	{
		{ ROCK },
		{ ROCK },
		{ ROCK },
		{ ROCK }
	},
	new [,]
	{
		{ ROCK, ROCK },
		{ ROCK, ROCK }
	}
};
var rockIndex = 0;

// initialize chamber
var chamber = new int[10000, 7];
for (var i = 0; i < chamber.GetLength(0); i++)
{
	for (var j = 0; j < chamber.GetLength(1); j++)
	{
		chamber[i, j] = AIR;
	}
}

// let the rocks fall
var height = 0;
for (var index = 1; index <= 2022; index++)
{
	var col = 2;
	var row = height + 3;
	var rock = GetNextRock();

	var isResting = false;
	while (!isResting)
	{
		// jet movement
		if (GetNextJetMovement() == '<')
		{
			if (col > 0 && IsPlacementPossible(col - 1, row, rock))
			{
				col--;
			}
		}
		else
		{
			if (col + GetRockWidth(rock) < GetChamberWidth() && IsPlacementPossible(col + 1, row, rock))
			{
				col++;
			}
		}

		// rock movement
		if (row > 0 && IsPlacementPossible(col, row - 1, rock))
		{
			row--;
		}
		else
		{
			PutRockIntoChamber(col, row, rock);
			isResting = true;
			height = Math.Max(height, row + GetRockHeight(rock));
			Console.WriteLine($"Rock placed at {row}/{col}, new height {height}");
			//PrintChamber();
		}
	}
}

bool IsPlacementPossible(int col, int row, int[,] rock)
{
	for (var i = 0; i < GetRockHeight(rock); i++)
	{
		for (var j = 0; j < GetRockWidth(rock); j++)
		{
			if (rock[i, j] == ROCK && chamber[row + i, col + j] != AIR)
			{
				return false;
			}
		}

	}
	return true;
}

void PutRockIntoChamber(int col, int row, int[,] rock)
{
	for (var i = 0; i < GetRockHeight(rock); i++)
	{
		for (var j = 0; j < GetRockWidth(rock); j++)
		{
			if (rock[i, j] == ROCK)
			{
				chamber[row + i, col + j] = ROCK;
			}
		}

	}
}

void PrintChamber()
{
	for (var row = height - 1; row >= 0; row--)
	{
		for (var col = 0; col < GetChamberWidth(); col++)
		{
			Console.Write(chamber[row, col] == AIR ? "." : "#");
		}
		Console.WriteLine();
	}
}

int GetRockWidth(int[,] rock)
{
	return rock.GetLength(1);
}

int GetRockHeight(int[,] rock)
{
	return rock.GetLength(0);
}

int GetChamberWidth()
{
	return chamber.GetLength(1);
}

char GetNextJetMovement()
{
	var result = jetPattern[jetIndex];
	jetIndex = (jetIndex + 1) % jetPattern.Length;
	return result;
}

int[,] GetNextRock()
{
	var result = rocks[rockIndex];
	rockIndex = (rockIndex + 1) % rocks.Length;
	return result;
}