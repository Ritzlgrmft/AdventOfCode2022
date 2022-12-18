var cubes = new List<(int x, int y, int z)>();
foreach (var line in File.ReadLines(@"../../../Input.txt"))
{
	var parts = line.Split(",");
	cubes.Add((x: int.Parse(parts[0]), y: int.Parse(parts[1]), z: int.Parse(parts[2])));
}

var neighbors = CountNeighbors(cubes);

var emptyFields = GetEmptyFields(cubes);
var (minX, maxX, minY, maxY, minZ, maxZ) = GetDimensions(cubes);
List<(int x, int y, int z)> emptyArea;
var trappedNeighbors = 0;
while (emptyFields.Count > 0)
{
	// retrieve empty area
	emptyArea = new List<(int x, int y, int z)>();
	ExpandEmptyArea(emptyFields[0]);

	// check if one side of the area touches the outer bounds
	// if yes, it is not trapped
	var (minXArea, maxXArea, minYArea, maxYArea, minZArea, maxZArea) = GetDimensions(emptyArea);
	if (minX != minXArea && maxX != maxXArea && minY != minYArea && maxY != maxYArea && minZ != minZArea && maxZ != maxZArea)
	{
		trappedNeighbors += emptyArea.Count * 6 - CountNeighbors(emptyArea);
	}
}

Console.WriteLine(cubes.Count * 6 - neighbors - trappedNeighbors);

bool AreNeighbors((int x, int y, int z) cube1, (int x, int y, int z) cube2)
{
	return (cube1.x == cube2.x && cube1.y == cube2.y && Math.Abs(cube1.z - cube2.z) == 1) ||
			(cube1.x == cube2.x && Math.Abs(cube1.y - cube2.y) == 1 && cube1.z == cube2.z) ||
			(Math.Abs(cube1.x - cube2.x) == 1 && cube1.y == cube2.y && cube1.z == cube2.z);
}

void ExpandEmptyArea((int x, int y, int z) field)
{
	emptyFields.Remove(field);
	emptyArea.Add(field);

	var emptyNeighbor = emptyFields.Find(f => AreNeighbors(field, f));
	while (emptyNeighbor != (0, 0, 0))
	{
		ExpandEmptyArea(emptyNeighbor);
		emptyNeighbor = emptyFields.Find(f => AreNeighbors(field, f));
	}
}

(int minX, int maxX, int minY, int maxY, int minZ, int MaxZ) GetDimensions(List<(int x, int y, int z)> cubes)
{
	var minX = int.MaxValue;
	var maxX = int.MinValue;
	var minY = int.MaxValue;
	var maxY = int.MinValue;
	var minZ = int.MaxValue;
	var maxZ = int.MinValue;
	foreach (var cube in cubes)
	{
		minX = Math.Min(minX, cube.x);
		minY = Math.Min(minY, cube.y);
		minZ = Math.Min(minZ, cube.z);
		maxX = Math.Max(maxX, cube.x);
		maxY = Math.Max(maxY, cube.y);
		maxZ = Math.Max(maxZ, cube.z);
	}
	return (minX, maxX, minY, maxY, minZ, maxZ);
}

List<(int x, int y, int z)> GetEmptyFields(List<(int x, int y, int z)> cubes)
{
	var emptyFields = new List<(int x, int y, int z)>();
	var (minX, maxX, minY, maxY, minZ, maxZ) = GetDimensions(cubes);
	for (var x = minX; x <= maxX; x++)
	{
		for (var y = minY; y <= maxY; y++)
		{
			for (var z = minZ; z <= maxZ; z++)
			{
				if (!cubes.Any(c => c.x == x && c.y == y && c.z == z))
				{
					emptyFields.Add((x, y, z));
				}
			}
		}
	}
	return emptyFields;
}

int CountNeighbors(List<(int x, int y, int z)> cubes)
{
	var neighbors = 0;
	for (var i = 0; i < cubes.Count; i++)
	{
		var cube1 = cubes[i];
		for (var j = i + 1; j < cubes.Count; j++)
		{
			var cube2 = cubes[j];
			if (AreNeighbors(cube1, cube2))
			{
				neighbors += 2;
			}
		}
	}
	return neighbors;
}