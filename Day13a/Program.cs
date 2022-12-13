using Newtonsoft.Json.Linq;

var lines = File.ReadLines(@"../../../Input.txt").ToArray();
var sum = 0;
for (var pair = 1; (pair - 1) * 3 + 2 <= lines.Length; pair++)
{
	var item1 = JArray.Parse(lines[(pair - 1) * 3]);
	var item2 = JArray.Parse(lines[(pair - 1) * 3 + 1]);

	var result = CompareItems(item1, item2);
	if (result == CompareResult.Lower)
	{
		sum += pair;
	}
}
Console.WriteLine(sum);

CompareResult CompareItems(JArray item1, JArray item2)
{
	for (var i = 0; i < Math.Min(item1.Count, item2.Count); i++)
	{
		if (item1[i] is JValue && item2[i] is JValue)
		{
			var value1 = (int)item1[i];
			var value2 = (int)item2[i];
			if (value1 < value2)
			{
				return CompareResult.Lower;
			}
			else if (value1 > value2)
			{
				return CompareResult.Higher;
			}
		}
		else if (item1[i] is JArray && item2[i] is JArray)
		{
			var result = CompareItems((JArray)item1[i], (JArray)item2[i]);
			if (result != CompareResult.Equal)
			{
				return result;
			}
		}
		else if (item1[i] is JValue)
		{
			var result = CompareItems(new JArray(item1[i]), (JArray)item2[i]);
			if (result != CompareResult.Equal)
			{
				return result;
			}
		}
		else
		{
			var result = CompareItems((JArray)item1[i], new JArray(item2[i]));
			if (result != CompareResult.Equal)
			{
				return result;
			}
		}
	}
	if (item1.Count < item2.Count)
	{
		return CompareResult.Lower;
	}
	else if (item1.Count > item2.Count)
	{
		return CompareResult.Higher;
	}
	return CompareResult.Equal;
}

enum CompareResult
{
	Lower,
	Equal,
	Higher
}
