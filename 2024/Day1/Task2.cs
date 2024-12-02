using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day1;

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = sample ? "./Day1/sample.txt" : "./Day1/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);
        string? line = sr.ReadLine();
        int[] left = [];
        Dictionary<int, int> observedInstances = [];
        while (line != null)
        {
            (int l, int right) = line.ToIntTouple("   ");
            left = left.Insert(l);

            if (observedInstances.TryGetValue(right, out int currObserved))
            {
                observedInstances[right] = currObserved + 1;
            }
            else
            {
                observedInstances[right] = 1;
            }

            line = sr.ReadLine();
        }

        var similarityScore = 0;

        foreach (var l in left)
        {
            if (observedInstances.TryGetValue(l, out int observed))
            {
                similarityScore += l * observed;
            }
        }

        return $"Similarity Score: {similarityScore}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}