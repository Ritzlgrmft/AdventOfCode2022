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
		monkey.Operation = parts[2];
		monkey.Monkey2Name = parts[3];
	}
	monkeys.Add(monkey);
}

while (monkeys.Count > 1)
{
	var monkeysWithValue = monkeys.Where(m => m.Value != null);
	foreach (var monkeyWithValue in monkeysWithValue)
	{
		foreach (var parentMonkey in monkeys.Where(m => m.Monkey1Name == monkeyWithValue.Name))
		{
			parentMonkey.Monkey1Value = monkeyWithValue.Value;
		}
		foreach (var parentMonkey in monkeys.Where(m => m.Monkey2Name == monkeyWithValue.Name))
		{
			parentMonkey.Monkey2Value = monkeyWithValue.Value;
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
				break;
			case "-":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value - monkeyWithPossibleOperation.Monkey2Value;
				break;
			case "*":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value * monkeyWithPossibleOperation.Monkey2Value;
				break;
			case "/":
				monkeyWithPossibleOperation.Value = monkeyWithPossibleOperation.Monkey1Value / monkeyWithPossibleOperation.Monkey2Value;
				break;
			default:
				throw new Exception("invalid operation " + monkeyWithPossibleOperation.Operation);
		}
	}
}
Console.WriteLine(monkeys[0].Value);




class Monkey
{
	public string Name { get; set; }
	public long? Value { get; set; }
	public string? Operation { get; set; }
	public long? Monkey1Value { get; set; }
	public string? Monkey1Name { get; set; }
	public long? Monkey2Value { get; set; }
	public string? Monkey2Name { get; set; }

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