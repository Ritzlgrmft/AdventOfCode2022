using System;

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
var chamberBase = 0L;
InitializeChamber();

// let the rocks fall
var maxIndex = 1_000_000_000_000L;
var height = 0L;
var count = 0L;
var calculatedHeights = new List<CalculatedHeight>();
CalculatedHeight? previousResult = null;
while (previousResult == null)
{
	// with the test input, rocks.Length is good enough
	// but with the real data, we need some more stabilization
	var delta = rocks.Length * 20;
	height = CalculateHeight(height, delta);
	count += delta;

	previousResult = calculatedHeights.Find(ch => ch.JetIndex == jetIndex);
	if (previousResult == null)
	{
		calculatedHeights.Add(new CalculatedHeight { JetIndex = jetIndex, Iterations = count, Height = height });
	}
}

var countDelta = count - previousResult.Iterations;
var heightDelta = height - previousResult.Height;
var iterations = maxIndex / countDelta;
var remainingIterations = maxIndex - iterations * countDelta - previousResult.Iterations;

var totalHeight = previousResult.Height + iterations * heightDelta + CalculateHeight(height, remainingIterations) - height;
Console.WriteLine(totalHeight);

long CalculateHeight(long height, long maxIndex)
{
	var startHeight = height;
	for (var index = 1L; index <= maxIndex; index++)
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
				ReduceChamber(height);
			}
		}
	}
	return height;
}

bool IsPlacementPossible(int col, long row, int[,] rock)
{
	for (var i = 0; i < GetRockHeight(rock); i++)
	{
		for (var j = 0; j < GetRockWidth(rock); j++)
		{
			if (rock[i, j] == ROCK && chamber[row - chamberBase + i, col + j] != AIR)
			{
				return false;
			}
		}

	}
	return true;
}

void PutRockIntoChamber(int col, long row, int[,] rock)
{
	for (var i = 0; i < GetRockHeight(rock); i++)
	{
		for (var j = 0; j < GetRockWidth(rock); j++)
		{
			if (rock[i, j] == ROCK)
			{
				chamber[row - chamberBase + i, col + j] = ROCK;
			}
		}

	}
}

void InitializeChamber()
{
	for (var i = 0; i < chamber.GetLength(0); i++)
	{
		for (var j = 0; j < chamber.GetLength(1); j++)
		{
			chamber[i, j] = AIR;
		}
	}
	chamberBase = 0L;
}

void ReduceChamber(long height)
{
	if (height - chamberBase > 500)
	{
		for (var i = 0; i < chamber.GetLength(0) - 450; i++)
		{
			for (var j = 0; j < GetChamberWidth(); j++)
			{
				chamber[i, j] = chamber[i + 450, j];
			}
		}
		chamberBase += 450;
	}
}

void PrintChamber(long height)
{
	for (var row = height - 1; row >= chamberBase; row--)
	{
		for (var col = 0; col < GetChamberWidth(); col++)
		{
			Console.Write(chamber[row - chamberBase, col] == AIR ? "." : "#");
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

class CalculatedHeight
{
	public long JetIndex { get; set; }
	public long Iterations { get; set; }
	public long Height { get; set; }
}