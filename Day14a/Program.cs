const int AIR = 0;
const int ROCK = 1;
const int SAND = 2;

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
	}
}

// count sand
var numberOfRestingSand = 0;
var isEndlessFalling = false;
while (!isEndlessFalling)
{
	var pos = (500, 0);
	var isResting = false;
	while (!isResting && !isEndlessFalling)
	{
		if (map[pos.Item1, pos.Item2 + 1] == AIR)
		{
			pos = (pos.Item1, pos.Item2 + 1);
			if (pos.Item2 == map.GetLength(1) - 1)
			{
				isEndlessFalling = true;
			}
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
	if (!isEndlessFalling)
	{
		numberOfRestingSand++;
	}
}
Console.WriteLine(numberOfRestingSand);
