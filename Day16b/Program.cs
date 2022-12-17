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

var states = new List<State>() { new State {
	CurrentValve1="AA",RemainingMinutes1=26, WorkMinutes1=0,CurrentValve2="AA",RemainingMinutes2=26, WorkMinutes2=0 ,
	OpenValves = valves.Where(v => v.FlowRate == 0).Select(v => v.Name).ToArray(),
	ReleasedPressure = 0 } };
var maxReleasedPressure = 0;
while (states.Count > 0)
{
	var state = states[states.Count - 1];
	var newStates = new List<State>();

	if (state.WorkMinutes1 == state.WorkMinutes2)
	{
		// move both
		var currentValve1 = valves.First(v => v.Name == state.CurrentValve1);
		var nextValves1 = valves.Where(v => !state.OpenValves.Contains(v.Name)).ToList();
		foreach (var nextValve1 in nextValves1)
		{
			var distance1 = distances[currentValve1.Index, nextValve1.Index];
			var availableMinutes1 = state.RemainingMinutes1 - distance1 - 1;
			if (availableMinutes1 > 0)
			{
				var currentValve2 = valves.First(v => v.Name == state.CurrentValve2);
				var nextValves2 = valves.Where(v => !state.OpenValves.Contains(v.Name) && v.Name != nextValve1.Name).ToList();
				foreach (var nextValve2 in nextValves2)
				{
					var distance2 = distances[currentValve2.Index, nextValve2.Index];
					var availableMinutes2 = state.RemainingMinutes2 - distance2 - 1;
					if (availableMinutes2 > 0)
					{
						newStates.Add(new State
						{
							CurrentValve1 = nextValve1.Name,
							RemainingMinutes1 = availableMinutes1,
							WorkMinutes1 = distance1 + 1,
							CurrentValve2 = nextValve2.Name,
							RemainingMinutes2 = availableMinutes2,
							WorkMinutes2 = distance2 + 1,
							OpenValves = state.OpenValves.Concat(new[] { nextValve1.Name, nextValve2.Name }).ToArray(),
							ReleasedPressure = state.ReleasedPressure + availableMinutes1 * nextValve1.FlowRate + availableMinutes2 * nextValve2.FlowRate
						});
					}
				}

			}
		}
	}
	else if (state.WorkMinutes1 < state.WorkMinutes2)
	{
		// move only 1
		var currentValve1 = valves.First(v => v.Name == state.CurrentValve1);
		var nextValves1 = valves.Where(v => !state.OpenValves.Contains(v.Name)).ToList();
		foreach (var nextValve1 in nextValves1)
		{
			var distance1 = distances[currentValve1.Index, nextValve1.Index];
			var availableMinutes1 = state.RemainingMinutes1 - distance1 - 1;
			if (availableMinutes1 > 0)
			{
				newStates.Add(new State
				{
					CurrentValve1 = nextValve1.Name,
					RemainingMinutes1 = availableMinutes1,
					WorkMinutes1 = distance1 + 1,
					CurrentValve2 = state.CurrentValve2,
					RemainingMinutes2 = state.RemainingMinutes2,
					WorkMinutes2 = state.WorkMinutes2 - state.WorkMinutes1,
					OpenValves = state.OpenValves.Concat(new[] { nextValve1.Name }).ToArray(),
					ReleasedPressure = state.ReleasedPressure + availableMinutes1 * nextValve1.FlowRate
				});

			}
		}
	}
	else
	{
		// move only 2
		var currentValve2 = valves.First(v => v.Name == state.CurrentValve2);
		var nextValves2 = valves.Where(v => !state.OpenValves.Contains(v.Name)).ToList();
		foreach (var nextValve2 in nextValves2)
		{
			var distance2 = distances[currentValve2.Index, nextValve2.Index];
			var availableMinutes2 = state.RemainingMinutes2 - distance2 - 1;
			if (availableMinutes2 > 0)
			{
				newStates.Add(new State
				{
					CurrentValve1 = state.CurrentValve1,
					RemainingMinutes1 = state.RemainingMinutes1,
					WorkMinutes1 = state.WorkMinutes1 - state.WorkMinutes2,
					CurrentValve2 = nextValve2.Name,
					RemainingMinutes2 = availableMinutes2,
					WorkMinutes2 = distance2 + 1,
					OpenValves = state.OpenValves.Concat(new[] { nextValve2.Name }).ToArray(),
					ReleasedPressure = state.ReleasedPressure + availableMinutes2 * nextValve2.FlowRate
				});

			}
		}
	}

	if (newStates.Count == 0 && state.ReleasedPressure > maxReleasedPressure)
	{
		maxReleasedPressure = state.ReleasedPressure;
		Console.WriteLine($"New maximum: {maxReleasedPressure}, number of states: {states.Count - 1}");
	}

	states.RemoveAt(states.Count - 1);
	states.AddRange(newStates);
}

Console.WriteLine(maxReleasedPressure);

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
	public string CurrentValve1 { get; set; }
	public int RemainingMinutes1 { get; set; }
	public int WorkMinutes1 { get; set; }
	public string CurrentValve2 { get; set; }
	public int RemainingMinutes2 { get; set; }
	public int WorkMinutes2 { get; set; }
	public string[] OpenValves { get; set; }
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