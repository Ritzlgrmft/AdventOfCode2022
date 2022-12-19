
var blueprints = new List<Blueprint>();
foreach (var line in File.ReadLines("../../../Input.txt").Take(3))
{
	var parts = line.Split(new[] { "Blueprint ", ": Each ore robot costs ", " ore. Each clay robot costs ", " ore. Each obsidian robot costs ", " ore and ", " clay. Each geode robot costs ", " obsidian." }, StringSplitOptions.RemoveEmptyEntries);
	blueprints.Add(new Blueprint
	{
		Index = int.Parse(parts[0]),
		OreRobotCosts = (int.Parse(parts[1]), 0, 0),
		ClayRobotCosts = (int.Parse(parts[2]), 0, 0),
		ObsidianRobotCosts = (int.Parse(parts[3]), int.Parse(parts[4]), 0),
		GeodeRobotCosts = (int.Parse(parts[5]), 0, int.Parse(parts[6]))
	});
}

var results = new List<int>();
foreach (var blueprint in blueprints)
{
	var qualityLevel = CalculateQualityLevel(blueprint, 0, 1, 0, 0, 0, 0, 0, 0, 0);
	Console.WriteLine($"Blueprint {blueprint.Index}: {qualityLevel}");
	results.Add(qualityLevel);
}

Console.WriteLine(results.Aggregate((a, b) => a * b));

int CalculateQualityLevel(Blueprint blueprint, int minute, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int ore, int clay, int obsidian, int geode,
	bool skipBuildOreRobot = false, bool skipBuildClayRobot = false, bool skipBuildObsidianRobot = false, bool skipBuildGeodeRobot = false)
{
	var maxOreRobotsNeeded = new[] { blueprint.ClayRobotCosts.ore, blueprint.ObsidianRobotCosts.ore, blueprint.GeodeRobotCosts.ore }.Max();
	var maxClayRobotsNeeded = blueprint.ObsidianRobotCosts.clay;
	var maxObsidianRobotsNeeded = blueprint.GeodeRobotCosts.obsidian;

	var canBuildOreRobot = !skipBuildOreRobot && ore >= blueprint.OreRobotCosts.ore && oreRobots < maxOreRobotsNeeded;
	var canBuildClayRobot = !skipBuildClayRobot && ore >= blueprint.ClayRobotCosts.ore && clayRobots < maxClayRobotsNeeded;
	var canBuildObsidianRobot = !skipBuildObsidianRobot && ore >= blueprint.ObsidianRobotCosts.ore && clay >= blueprint.ObsidianRobotCosts.clay && obsidianRobots < maxObsidianRobotsNeeded;
	var canBuildGeodeRobot = !skipBuildGeodeRobot && ore >= blueprint.GeodeRobotCosts.ore && obsidian >= blueprint.GeodeRobotCosts.obsidian;

	minute++;
	ore += oreRobots;
	clay += clayRobots;
	obsidian += obsidianRobots;
	geode += geodeRobots;

	if (minute == 32)
	{
		return geode;
	}

	var results = new List<int>();
	if (canBuildGeodeRobot)
	{
		results.Add(CalculateQualityLevel(blueprint, minute, oreRobots, clayRobots, obsidianRobots, geodeRobots + 1, ore - blueprint.GeodeRobotCosts.ore, clay, obsidian - blueprint.GeodeRobotCosts.obsidian, geode));
	}
	else
	{
		if (canBuildObsidianRobot)
		{
			results.Add(CalculateQualityLevel(blueprint, minute, oreRobots, clayRobots, obsidianRobots + 1, geodeRobots, ore - blueprint.ObsidianRobotCosts.ore, clay - blueprint.ObsidianRobotCosts.clay, obsidian, geode));
		}
		if (canBuildClayRobot)
		{
			results.Add(CalculateQualityLevel(blueprint, minute, oreRobots, clayRobots + 1, obsidianRobots, geodeRobots, ore - blueprint.ClayRobotCosts.ore, clay, obsidian, geode));
		}
		if (canBuildOreRobot)
		{
			results.Add(CalculateQualityLevel(blueprint, minute, oreRobots + 1, clayRobots, obsidianRobots, geodeRobots, ore - blueprint.OreRobotCosts.ore, clay, obsidian, geode));
		}
		results.Add(CalculateQualityLevel(blueprint, minute, oreRobots, clayRobots, obsidianRobots, geodeRobots, ore, clay, obsidian, geode, canBuildOreRobot, canBuildClayRobot, canBuildObsidianRobot, canBuildObsidianRobot));
	}
	return results.Max();
}

struct Blueprint
{
	public int Index;
	public (int ore, int clay, int obsidian) OreRobotCosts;
	public (int ore, int clay, int obsidian) ClayRobotCosts;
	public (int ore, int clay, int obsidian) ObsidianRobotCosts;
	public (int ore, int clay, int obsidian) GeodeRobotCosts;
}

struct State
{
	public int OreRobots;
	public int ClayRobots;
	public int ObsidianRobots;
	public int GeodeRobots;
	public int RemainingMinutes;
	public int Ore;
	public int Clay;
	public int Obsidian;
	public int Geode;
}