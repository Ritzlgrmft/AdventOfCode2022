// parse input
var valves = new List<Valve>();
foreach (string line in File.ReadLines(@"../../../Input.txt"))
{
	var parts = line.Split(new string[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve " }, StringSplitOptions.RemoveEmptyEntries);
	valves.Add(new Valve { Index = valves.Count, Name = parts[0], FlowRate = int.Parse(parts[1]), IsOpen = false, Tunnels = parts[2].Split(", ") });
}

// calculate distances
var adjacencyMatrix = new int[valves.Count, valves.Count];
for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
{
	for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
	{
		if (i == j)
		{
			adjacencyMatrix[i, j] = 0;
		}
		else if (valves[i].Tunnels.Contains(valves[j].Name))
		{
			adjacencyMatrix[i, j] = 1;
		}
		else
		{
			adjacencyMatrix[i, j] = valves.Count + 1;
		}
	}
}
var distances = FloydWarshall(adjacencyMatrix, valves.Count);

var states = new List<State>() { new State { CurrentValve = "AA", OpenValves = valves.Where(v => v.FlowRate == 0).Select(v => v.Name).ToArray(), RemainingMinutes = 30, ReleasedPressure = 0 } };
var endStates = new List<State>();
while (states.Count > 0)
{
	var state = states[0];
	var statesCount = states.Count;
	var currentValve = valves.First(v => v.Name == state.CurrentValve);
	var nextValves = valves.Where(v => !state.OpenValves.Contains(v.Name));
	foreach (var nextValve in nextValves)
	{
		var distance = distances[currentValve.Index, nextValve.Index];
		var availableMinutes = state.RemainingMinutes - distance - 1;
		if (availableMinutes > 0)
		{
			states.Add(new State
			{
				CurrentValve = nextValve.Name,
				OpenValves = state.OpenValves.Concat(new[] { nextValve.Name }).ToArray(),
				RemainingMinutes = availableMinutes,
				ReleasedPressure = state.ReleasedPressure + availableMinutes * nextValve.FlowRate
			});
		}
	}

	if (states.Count == statesCount)
	{
		endStates.Add(state);
	}
	states.RemoveAt(0);
}

Console.WriteLine(endStates.Select(s => s.ReleasedPressure).Max());

int[,] FloydWarshall(int[,] adjacencyMatrix, int numberOfNodes)
{
	int[,] distances = new int[numberOfNodes, numberOfNodes];
	for (int i = 0; i < numberOfNodes; i++)
	{
		for (int j = 0; j < numberOfNodes; j++)
		{
			distances[i, j] = adjacencyMatrix[i, j];
		}
	}

	for (int k = 0; k < numberOfNodes; k++)
	{
		for (int i = 0; i < numberOfNodes; i++)
		{
			for (int j = 0; j < numberOfNodes; j++)
			{
				if (distances[i, k] + distances[k, j] < distances[i, j])
				{
					distances[i, j] = distances[i, k] + distances[k, j];
				}
			}
		}
	}
	return distances;
}

struct State
{
	public string CurrentValve { get; set; }
	public string[] OpenValves { get; set; }
	public int RemainingMinutes { get; set; }
	public int ReleasedPressure { get; set; }
}

struct Valve
{
	public int Index { get; set; }
	public string Name { get; set; }
	public int FlowRate { get; set; }
	public bool IsOpen { get; set; }
	public string[] Tunnels { get; set; }
}