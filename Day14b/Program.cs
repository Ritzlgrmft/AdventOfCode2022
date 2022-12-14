const int AIR = 0;
const int ROCK = 1;
const int SAND = 2;
const int FLOOR = 3;

// initialize map
var map = new int[1000, 200];
for (var x = 0; x < map.GetLength(0); x++)
{
	for (var y = 0; y < map.GetLength(1); y++)
	{
		map[x, y] = AIR;
	}
}

// draw rocks
var highestY = 0;
foreach (var line in File.ReadLines(@"../../../Input.txt"))
{
	var paths = line.Split(" -> ").Select(p => p.Split(",")).Select(a => (int.Parse(a[0]), int.Parse(a[1]))).ToArray();
	for (var p = 0; p < paths.Length - 1; p++)
	{
		var minX = Math.Min(paths[p].Item1, paths[p + 1].Item1);
		var maxX = Math.Max(paths[p].Item1, paths[p + 1].Item1);
		var minY = Math.Min(paths[p].Item2, paths[p + 1].Item2);
		var maxY = Math.Max(paths[p].Item2, paths[p + 1].Item2);
		if (minX == maxX)
		{
			// draw vertical
			for (var y = minY; y <= maxY; y++)
			{
				map[minX, y] = ROCK;
			}
		}
		else
		{
			// draw horizontal
			for (var x = minX; x <= maxX; x++)
			{
				map[x, minY] = ROCK;
			}
		}
		highestY = Math.Max(maxY, highestY);
	}
}

// draw floor
for (var x = 0; x < map.GetLength(0); x++)
{
	map[x, highestY + 2] = FLOOR;
}

// count sand
var numberOfRestingSand = 0;
var isSourceBlocked = false;
while (!isSourceBlocked)
{
	var pos = (500, 0);
	if (map[pos.Item1, pos.Item2] != AIR)
	{
		isSourceBlocked = true;
	}

	var isResting = false;
	while (!isResting && !isSourceBlocked)
	{
		if (map[pos.Item1, pos.Item2 + 1] == AIR)
		{
			pos = (pos.Item1, pos.Item2 + 1);
		}
		else if (map[pos.Item1 - 1, pos.Item2 + 1] == AIR)
		{
			pos = (pos.Item1 - 1, pos.Item2 + 1);
		}
		else if (map[pos.Item1 + 1, pos.Item2 + 1] == AIR)
		{
			pos = (pos.Item1 + 1, pos.Item2 + 1);
		}
		else
		{
			map[pos.Item1, pos.Item2] = SAND;
			isResting = true;
		}
	}
	if (!isSourceBlocked)
	{
		numberOfRestingSand++;
	}
}
Console.WriteLine(numberOfRestingSand);
