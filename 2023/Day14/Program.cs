using Extensions;
using static System.Console;
using Point = (int col, int row);

Part1();
Part2();

void Part1()
{
    var input = File.ReadAllLines("./input");

    List<Point> cubeRocks = [];
    List<Point> roundRocks = [];

    for (int i = 0; i < input.Length; i++)
    {
        var row = input[i];
        for (int j = 0; j < row.Length; j++)
        {
            var ch = row[j];
            if (ch == '#') cubeRocks.Add((j, i));
            if (ch == 'O') roundRocks.Add((j, i));
        }
    }

    var totalWeight = roundRocks.Sum(r => r.Roll(cubeRocks, roundRocks, input.Length));

    WriteLine(totalWeight);
}

void Part2()
{
    var input = File.ReadAllLines("./input");

    List<Point> cubeRocks = [];
    List<Point> roundRocks = [];

    for (int i = 0; i < input.Length; i++)
    {
        var row = input[i];
        for (int j = 0; j < row.Length; j++)
        {
            var ch = row[j];
            if (ch == '#') cubeRocks.Add((j, i));
            if (ch == 'O') roundRocks.Add((j, i));
        }
    }

    var southIdx = input.Length - 1;
    var eastIdx = input[0].Length - 1;

    var currentRoundRocks = roundRocks;
    var seenStates = new Dictionary<string, int>();
    var totalCycles = 1000000000;
    var cycle = 0;

    while (cycle < totalCycles)
    {
        currentRoundRocks = currentRoundRocks.Cycle(cubeRocks, southIdx, eastIdx);
        var hash = currentRoundRocks.GetHash();

        if (seenStates.TryGetValue(hash, out int value))
        {
            var previousCycle = value;
            var cycleLength = cycle - previousCycle;

            cycle += ((totalCycles - cycle) / cycleLength) * cycleLength;
        }
        else
        {
            seenStates[hash] = cycle;
        }

        cycle++;
    }

    var totalWeight = currentRoundRocks.NorthLoad(input.Length);
    WriteLine(totalWeight);
}