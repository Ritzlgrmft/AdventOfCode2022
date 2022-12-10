var cycle = 0;
var x = 1;

var drawing = "";
foreach (string line in File.ReadLines(@"../../../Input.txt"))
{
	var lineParts = line.Split(' ');
	int oldCycle = cycle;
	var oldX = x;
	if (lineParts[0] == "noop")
	{
		cycle++;
		drawing = DrawSprite(drawing, cycle, x);
	}
	else if (lineParts[0] == "addx")
	{
		cycle++;
		drawing = DrawSprite(drawing, cycle, x);
		cycle++;
		drawing = DrawSprite(drawing, cycle, x);
		x += int.Parse(lineParts[1]);
	}
	else
	{
		throw new Exception($"illegal command {line}");
	}
}

string DrawSprite(string drawing, int cycle, int x)
{
	var column = (cycle - 1) % 40;
	drawing += Math.Abs(column - x) <= 1 ? "#" : ".";
	if (column == 39)
	{
		drawing += "\n";
	}
	return drawing;
}

Console.WriteLine(drawing);
