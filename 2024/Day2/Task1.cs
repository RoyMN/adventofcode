using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day2;

public class Task1 : IDailyRunner
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
            var levels = line.ToInts(' ');
            var (isValid, _, _) = levels.CheckLevels(logger);
            if (isValid)
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
        ConsoleWriter logger = new(logLevel);
        var lines = example.Split('\n');
        var safe = 0;
        foreach (var line in lines)
        {
            var levels = line.ToInts(' ');
            var (isValid, _, _) = levels.CheckLevels(logger);
            if (isValid)
            {
                logger.Line($"Safe: {line}");
                safe++;
            }
        }
        return $"Safe: {safe}";
    }
}