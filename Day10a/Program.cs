var cycle = 0;
var x = 1;
var relevantCycles = new[] { 20, 60, 100, 140, 180, 220 };

var sum = 0;
foreach (string line in File.ReadLines(@"../../../Input.txt"))
{
	var lineParts = line.Split(' ');
	int oldCycle = cycle;
	var oldX = x;
	if (lineParts[0] == "noop")
	{
		cycle++;
		sum = CheckForRelevantCycle(sum, cycle, x);
	}
	else if (lineParts[0] == "addx")
	{
		cycle++;
		sum = CheckForRelevantCycle(sum, cycle, x);
		cycle++;
		sum = CheckForRelevantCycle(sum, cycle, x);
		x += int.Parse(lineParts[1]);
	}
	else
	{
		throw new Exception($"illegal command {line}");
	}

	Console.WriteLine($"{line}: from {oldCycle}/{oldX} to {cycle}/{x}");
}

int CheckForRelevantCycle(int sum, int cycle, int x)
{
	if (relevantCycles.Contains(cycle))
	{
		Console.WriteLine($"{cycle}/{x} ==> {(cycle) * x}");
		sum += cycle * x;
	}
	else
	{
		Console.WriteLine($"{cycle}/{x}");
	}
	return sum;
}

Console.WriteLine(sum);
