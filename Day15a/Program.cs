const int rowOfInterest = 2000000;
//const int rowOfInterest = 10;
var notPossiblePositionRanges = new List<(int, int)>();
var knownBeaconsInRowOfInterest = new List<int>();

foreach (var line in File.ReadLines(@"../../../Input.txt"))
{
	Console.WriteLine(line);

	var lineParts = line.Split(new string[] { "Sensor at x=", ", y=", ": closest beacon is at x=" }, StringSplitOptions.RemoveEmptyEntries);
	var sensor = (int.Parse(lineParts[0]), int.Parse(lineParts[1]));
	var beacon = (int.Parse(lineParts[2]), int.Parse(lineParts[3]));
	if (beacon.Item2 == rowOfInterest && !knownBeaconsInRowOfInterest.Contains(beacon.Item1))
	{
		knownBeaconsInRowOfInterest.Add(beacon.Item1);
	}

	var distance = Math.Abs(sensor.Item1 - beacon.Item1) + Math.Abs(sensor.Item2 - beacon.Item2);
	var distanceToRowOfInterest = Math.Abs(sensor.Item2 - rowOfInterest);
	var distanceDelta = distance - distanceToRowOfInterest;

	if (distanceDelta >= 0)
	{
		notPossiblePositionRanges.Add((sensor.Item1 - distanceDelta, sensor.Item1 + distanceDelta));
		Console.WriteLine($"Add range from {sensor.Item1 - distanceDelta} to {sensor.Item1 + distanceDelta}");
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

var count = notPossiblePositionRanges.Select(r => r.Item2 - r.Item1 + 1).Sum();

Console.WriteLine(count - knownBeaconsInRowOfInterest.Count);
