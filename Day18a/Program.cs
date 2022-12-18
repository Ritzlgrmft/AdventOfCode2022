var cubes = new List<(int x, int y, int z)>();
foreach (var line in File.ReadLines(@"../../../Input.txt"))
{
	var parts = line.Split(",");
	cubes.Add((x: int.Parse(parts[0]), y: int.Parse(parts[1]), z: int.Parse(parts[2])));
}

var neighbors = 0;
for (var i = 0; i < cubes.Count; i++)
{
	var cube1 = cubes[i];
	for (var j = i + 1; j < cubes.Count; j++)
	{
		var cube2 = cubes[j];
		if ((cube1.x == cube2.x && cube1.y == cube2.y && Math.Abs(cube1.z - cube2.z) == 1) ||
			(cube1.x == cube2.x && Math.Abs(cube1.y - cube2.y) == 1 && cube1.z == cube2.z) ||
			(Math.Abs(cube1.x - cube2.x) == 1 && cube1.y == cube2.y && cube1.z == cube2.z))
		{
			neighbors += 2;
		}
	}
}

Console.WriteLine(cubes.Count * 6 - neighbors);
