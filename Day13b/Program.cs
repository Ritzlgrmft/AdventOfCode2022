using Newtonsoft.Json.Linq;

var items = new List<JArray>();
foreach (string line in File.ReadLines(@"../../../Input.txt"))
{
	if (!string.IsNullOrEmpty(line))
	{
		items.Add(JArray.Parse(line));
	}
}

var divider1 = JArray.Parse("[[2]]");
items.Add(divider1);
var divider2 = JArray.Parse("[[6]]");
items.Add(divider2);

items.Sort((a, b) =>
{
	var result = CompareItems(a, b);
	if (result == CompareResult.Lower)
	{
		return -1;
	}
	else if (result == CompareResult.Higher)
	{
		return 1;
	}
	else
	{
		return 0;
	}
});

var dividerPos1 = items.FindIndex(i => i == divider1) + 1;
var dividerPos2 = items.FindIndex(i => i == divider2) + 1;
Console.WriteLine(dividerPos1 * dividerPos2);

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


string ItemToString(List<object> item)
{
	var parts = new List<string>();
	foreach (var part in item)
	{
		if (part is int)
		{
			parts.Add(((int)part).ToString());
		}
		else
		{
			parts.Add(ItemToString((List<object>)part));
		}
	}
	return "[" + string.Join(",", parts) + "]";
}

enum CompareResult
{
	Lower,
	Equal,
	Higher
}
