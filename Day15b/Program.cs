const int min = 0;
const int max = 4000000;

int y = min;
var x = min - 1;
while (y <= max && x < min)
{
	if (y % 10000 == 0)
	{
		Console.WriteLine(y);
	}
	var notPossiblePositionRanges = GetNotPossiblePositionRanges(y).Where(r => r.Item2 >= min && r.Item1 <= max);
	if (notPossiblePositionRanges.Count() > 1)
	{
		x = notPossiblePositionRanges.First().Item2 + 1;
	}
	else if (notPossiblePositionRanges.First().Item1 > min)
	{
		x = notPossiblePositionRanges.First().Item1 - 1;
	}
	else if (notPossiblePositionRanges.Last().Item2 < max)
	{
		x = notPossiblePositionRanges.Last().Item2 + 1;
	}
	else
	{
		y++;
	}
}
var frequency = ((long)x) * 4000000L + y;
Console.WriteLine($"x={x}, y={y}, frequency={frequency}");



List<(int, int)> GetNotPossiblePositionRanges(int rowOfInterest)
{
	var notPossiblePositionRanges = new List<(int, int)>();
	foreach (var line in File.ReadLines(@"../../../Input.txt"))
	{
		var lineParts = line.Split(new string[] { "Sensor at x=", ", y=", ": closest beacon is at x=" }, StringSplitOptions.RemoveEmptyEntries);
		var sensor = (int.Parse(lineParts[0]), int.Parse(lineParts[1]));
		var beacon = (int.Parse(lineParts[2]), int.Parse(lineParts[3]));

		var distance = Math.Abs(sensor.Item1 - beacon.Item1) + Math.Abs(sensor.Item2 - beacon.Item2);
		var distanceToRowOfInterest = Math.Abs(sensor.Item2 - rowOfInterest);
		var distanceDelta = distance - distanceToRowOfInterest;

		if (distanceDelta >= 0)
		{
			notPossiblePositionRanges.Add((sensor.Item1 - distanceDelta, sensor.Item1 + distanceDelta));
		}
	}

	var index = 0;
	while (index < notPossiblePositionRanges.Count - 1)
	{
		var rangeToCheck = notPossiblePositionRanges[index];
		var hasJoined = false;
		for (var otherIndex = index + 1; otherIndex < notPossiblePositionRanges.Count && !hasJoined; otherIndex++)
		{
			var otherRangeToCheck = notPossiblePositionRanges[otherIndex];
			if (rangeToCheck.Item1 <= otherRangeToCheck.Item1 && rangeToCheck.Item2 >= otherRangeToCheck.Item1 ||
				rangeToCheck.Item1 >= otherRangeToCheck.Item1 && rangeToCheck.Item1 <= otherRangeToCheck.Item2)
			{
				rangeToCheck.Item1 = Math.Min(rangeToCheck.Item1, otherRangeToCheck.Item1);
				rangeToCheck.Item2 = Math.Max(rangeToCheck.Item2, otherRangeToCheck.Item2);
				notPossiblePositionRanges[index] = rangeToCheck;
				notPossiblePositionRanges.RemoveAt(otherIndex);
				hasJoined = true;
			}
		}
		if (!hasJoined)
		{
			index++;
		}
	}
	return notPossiblePositionRanges;
}
