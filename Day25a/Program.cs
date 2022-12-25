var map = new Dictionary<char, int>()
{
	{ '0', 0 },
	{ '1', 1 },
	{ '2', 2 },
	{ '-', -1 },
	{ '=', -2 },
};

var numbers = new List<long>();
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	numbers.Add(ParseNumber(line));
}
Console.WriteLine(FormatNumber(numbers.Sum()));

long ParseNumber(string text)
{
	var multiplier = 1L;
	var result = 0L;
	foreach (var c in text.Reverse())
	{
		result += map[c] * multiplier;
		multiplier *= 5;
	}
	return result;
}

string FormatNumber(long number)
{
	var result = "";
	while (number > 0)
	{
		var digit = (number + 2) % 5 - 2;
		result = map.FirstOrDefault(kvp => kvp.Value == digit).Key + result;
		number = (number + 2) / 5;
	}
	return result;
}