using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day2;

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = sample ? "./Day2/sample.txt" : "./Day2/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);
        string? line = sr.ReadLine();
        var safe = 0;
        while (line != null)
        {
            var lines = line.ToInts(' ').ToCombinationsWithoutOne();
            if (lines.Any(l => l.CheckLevels(logger).isValid))
            {
                logger.Line($"Safe: {line}");
                safe++;
            }
            line = sr.ReadLine();
        }

        return $"Safe: {safe}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}