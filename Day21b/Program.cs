using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

var monkeys = new List<Monkey>();
foreach (var line in File.ReadLines("../../../Input.txt"))
{
	var parts = line.Split(new[] { ": ", " " }, StringSplitOptions.RemoveEmptyEntries);
	var monkey = new Monkey { Name = parts[0] };
	if (parts.Length == 2)
	{
		monkey.Value = long.Parse(parts[1]);
	}
	else
	{
		monkey.Monkey1Name = parts[1];
		monkey.Operation = monkey.Name == "root" ? "=" : parts[2];
		monkey.Monkey2Name = parts[3];
	}
	monkeys.Add(monkey);
}

// reduce tree as far as possible without a value of humn
monkeys.First(m => m.Name == "humn").Value = null;

var hasSomethingDone = true;
while (monkeys.Count > 1 && hasSomethingDone)
{
	hasSomethingDone = false;
	var monkeysWithValue = monkeys.Where(m => m.Value != null);
	foreach (var monkeyWithValue in monkeysWithValue)
	{
		foreach (var parentMonkey in monkeys.Where(m => m.Monkey1Name == monkeyWithValue.Name))
		{
			parentMonkey.Monkey1Value = monkeyWithValue.Value;
			hasSomethingDone = true;
		}
		foreach (var parentMonkey in monkeys.Where(m => m.Monkey2Name == monkeyWithValue.Name))
		{
			parentMonkey.Monkey2Value = monkeyWithValue.Value;
			hasSomethingDone = true;
		}
	}
	monkeys.RemoveAll(m => monkeysWithValue.Contains(m));

	var monkeysWithPossibleOperation = monkeys.Where(m => m.Value == null && m.Monkey1Value != null && m.Monkey2Value != null);
	foreach (var monkeyWithPossibleOperation in monkeysWithPossibleOperation)
	{
		switch (monkeyWithPossibleOperation.Operation)
		{
			case "+":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value + monkeyWithPossibleOperation.Monkey2Value;
				hasSomethingDone = true;
				break;
			case "-":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value - monkeyWithPossibleOperation.Monkey2Value;
				hasSomethingDone = true;
				break;
			case "*":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value * monkeyWithPossibleOperation.Monkey2Value;
				hasSomethingDone = true;
				break;
			case "/":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value / monkeyWithPossibleOperation.Monkey2Value;
				hasSomethingDone = true;
				break;
			case "=":
				break;
			default:
				throw new Exception("invalid operation " + monkeyWithPossibleOperation.Operation);
		}
	}
}

var root = monkeys.First(m => m.Name == "root");
long targetValue;
string nextMonkeyName;
if (root.Monkey1Value != null)
{
	targetValue = (long)root.Monkey1Value;
	nextMonkeyName = root.Monkey2Name;
}
else
{
	targetValue = (long)root.Monkey2Value;
	nextMonkeyName = root.Monkey1Name;
}
while (nextMonkeyName != "humn")
{
	var nextMonkey = monkeys.First(m => m.Name == nextMonkeyName);
	switch (nextMonkey.Operation)
	{
		case "*":
			if (nextMonkey.Monkey1Value != null)
			{
				targetValue = targetValue / (long)nextMonkey.Monkey1Value;
				nextMonkeyName = nextMonkey.Monkey2Name;
			}
			else
			{
				targetValue = targetValue / (long)nextMonkey.Monkey2Value;
				nextMonkeyName = nextMonkey.Monkey1Name;
			}
			break;
		case "+":
			if (nextMonkey.Monkey1Value != null)
			{
				targetValue = targetValue - (long)nextMonkey.Monkey1Value;
				nextMonkeyName = nextMonkey.Monkey2Name;
			}
			else
			{
				targetValue = targetValue - (long)nextMonkey.Monkey2Value;
				nextMonkeyName = nextMonkey.Monkey1Name;
			}
			break;
		case "/":
			if (nextMonkey.Monkey1Value != null)
			{
				throw new NotImplementedException();
			}
			else
			{
				targetValue = targetValue * (long)nextMonkey.Monkey2Value;
				nextMonkeyName = nextMonkey.Monkey1Name;
			}
			break;
		case "-":
			if (nextMonkey.Monkey1Value != null)
			{
				targetValue = (long)nextMonkey.Monkey1Value - targetValue;
				nextMonkeyName = nextMonkey.Monkey2Name;
			}
			else
			{
				targetValue = targetValue + (long)nextMonkey.Monkey2Value;
				nextMonkeyName = nextMonkey.Monkey1Name;
			}
			break;
	}
}
Console.WriteLine(targetValue);

class Monkey
{
	public string Name { get; set; }
	public long? Value { get; set; }
	public string? Operation { get; set; }
	public long? Monkey1Value { get; set; }
	public string? Monkey1Name { get; set; }
	public long? Monkey2Value { get; set; }
	public string? Monkey2Name { get; set; }

	public Monkey Clone()
	{
		return new Monkey { Name = Name, Value = Value, Operation = Operation, Monkey1Value = Monkey1Value, Monkey1Name = Monkey1Name, Monkey2Value = Monkey2Value, Monkey2Name = Monkey2Name };
	}

	public override string ToString()
	{
		var result = new StringBuilder();
		result.Append(Name);
		result.Append(": ");
		if (Value != null)
		{
			result.Append(Value);
		}
		else
		{
			if (Monkey1Value != null)
			{
				result.Append(Monkey1Value);
			}
			else
			{
				result.Append(Monkey1Name);
			}
			result.Append(" ");
			result.Append(Operation);
			result.Append(" ");
			if (Monkey2Value != null)
			{
				result.Append(Monkey2Value);
			}
			else
			{
				result.Append(Monkey2Name);
			}
		}
		return result.ToString();
	}
}