using static System.Runtime.InteropServices.JavaScript.JSType;

var numbers = new List<(int Index, long Value)>();
var index = 0;
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	numbers.Add((Index: index, Value: long.Parse(line) * 811589153));
	index++;
}

for (var j = 0; j < 10; j++)
{
	for (var i = 0; i < numbers.Count(); i++)
	{
		var currentIndex = numbers.FindIndex(n => n.Index == i);
		var number = numbers[currentIndex].Value;

		if (number > 0)
		{
			MoveRight(currentIndex, number);
		}
		else if (number < 0)
		{
			MoveLeft(currentIndex, -number);
		}
	}
}

var posOfZero = numbers.FindIndex(n => n.Value == 0);
var result = new[] { 1000, 2000, 3000 }.Select(i =>
{
	var v = numbers[(posOfZero + i) % numbers.Count()].Value;
	Console.WriteLine(v);
	return v;
}).Sum();
Console.WriteLine(result);


void MoveLeft(int start, long count)
{
	// we should remove the item from the list, then move and finally insert it again (virtually)
	// therefore it doesn't matter if we move x or x + number.Count() - 1
	count = count % (numbers.Count() - 1);
	var number = numbers[start];
	for (var i = 0; i < count; i++)
	{
		var indexTo = (numbers.Count() * 2 + start - i) % numbers.Count();
		var indexFrom = (numbers.Count() * 2 + start - i - 1) % numbers.Count();
		numbers[indexTo] = numbers[indexFrom];
	}
	numbers[(numbers.Count() * 2 + start - (int)count) % numbers.Count()] = number;
}

void MoveRight(int start, long count)
{
	count = count % (numbers.Count() - 1);
	var number = numbers[start];
	for (var i = 0; i < count; i++)
	{
		var indexTo = (numbers.Count() + start + i) % numbers.Count();
		var indexFrom = (numbers.Count() + start + i + 1) % numbers.Count();
		numbers[indexTo] = numbers[indexFrom];
	}
	numbers[(numbers.Count() + start + (int)count) % numbers.Count()] = number;
}