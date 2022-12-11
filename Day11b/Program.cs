using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

var monkeys = new Monkey[]
{
	//new Monkey(new long[] { 79, 98 }, old => old * 19, 23, 2, 3),
	//new Monkey(new long[] { 54, 65, 75, 74 }, old => old + 6, 19, 2, 0),
	//new Monkey(new long[] { 79, 60, 97 }, old => old * old, 13, 1, 3),
	//new Monkey(new long[] { 74 }, old => old + 3, 17, 0, 1)
	new Monkey(new long[] { 74, 73, 57, 77, 74 }, old => old * 11, 19, 6, 7),
	new Monkey(new long[] { 99, 77, 79 }, old => old + 8, 2, 6, 0),
	new Monkey(new long[] { 64, 67, 50, 96, 89, 82, 82 }, old => old + 1, 3, 5, 3),
	new Monkey(new long[] { 88 }, old => old * 7, 17, 5, 4),
	new Monkey(new long[] { 80, 66, 98, 83, 70, 63, 57, 66 }, old => old + 4, 13, 0, 1),
	new Monkey(new long[] { 81, 93, 90, 61, 62, 64 }, old => old + 7, 7, 1, 4),
	new Monkey(new long[] { 69, 97, 88, 93 }, old => old * old, 5, 7, 2),
	new Monkey(new long[] { 59, 80 }, old => old + 6, 11, 2, 3)
};

for (var round = 0; round < 10000; round++)
{
	foreach (var monkey in monkeys)
	{
		var mod = monkeys.Aggregate(1L, (mod, m) => mod * m.Test);
		while (monkey.Items.Count > 0)
		{
			monkey.NumberOfInspections++;
			var oldValue = monkey.Items[0];
			monkey.Items.RemoveAt(0);

			// The crucial thing is to not modify the test result. Therefore we cannot use an arbitrary number. But the product of the tests works.
			// We have to ensure that we do not modify the results of the tests (newValue % test == 0).
			// However, it doesn't matter, if we do another modulo operation before, as long as we do it with a multiple of test.
			// That means these two operations have the same result:
			// newValue % test
			// newValue % (test * x) % test
			// Since this should work for all monkeys, the save way is to use the product of all Monkeys' tests.
			var newValue = (long)monkey.Operation(oldValue) % mod;
			monkeys[newValue % monkey.Test == 0 ? monkey.TrueMonkey : monkey.FalseMonkey].Items.Add(newValue);
		}
	}

	for (var monkey = 0; monkey < monkeys.Length; monkey++)
	{
		Console.WriteLine($"Monkey {monkey}: {string.Join(" ", monkeys[monkey].Items.Select(i => i.ToString()))}");
	}
	Console.WriteLine($"Number of Inspections: {string.Join(" ", monkeys.Select(m => m.NumberOfInspections.ToString()))}");
}

var monkeyBusiness = monkeys.Select(m => m.NumberOfInspections).OrderByDescending(n => n).Take(2).Aggregate(1L, (x, y) => x * y);
Console.WriteLine(monkeyBusiness);

class Monkey
{
	public Monkey(long[] items, Func<long, long> operation, long test, long trueMonkey, long falseMonkey)
	{
		Items = new List<long>(items);
		Operation = operation;
		Test = test;
		TrueMonkey = trueMonkey;
		FalseMonkey = falseMonkey;
		NumberOfInspections = 0;
	}

	public List<long> Items { get; set; }
	public Func<long, long> Operation { get; set; }
	public long Test { get; set; }
	public long TrueMonkey { get; set; }
	public long FalseMonkey { get; set; }
	public long NumberOfInspections { get; set; }
}