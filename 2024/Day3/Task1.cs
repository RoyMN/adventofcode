using System.Text.RegularExpressions;
using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day3;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = "./Day3/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);
        
        string input = sr.ReadToEnd();
        sr.Close();

        int sum = 0;

        foreach (Match match in RegexHelpers.multRegex().Matches(input))
        {
            if (match.Groups.Count == 3 &&
                int.TryParse(match.Groups[1].Value, out int left) &&
                int.TryParse(match.Groups[2].Value, out int right))
            {
                logger.Debug($"Match found: {match.Value} -> {left} * {right}");
                sum += left * right;
            }
        }

        return $"Sum of all multiplications: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        ConsoleWriter logger = new(logLevel);
        int sum = 0;

        foreach (Match match in RegexHelpers.multRegex().Matches(example))
        {
            if (match.Groups.Count == 3 &&
                int.TryParse(match.Groups[1].Value, out int left) &&
                int.TryParse(match.Groups[2].Value, out int right))
            {
                logger.Debug($"Match found: {match.Value} -> {left} * {right}");
                sum += left * right;
            }
        }

        return $"Sum of all multiplications: {sum}";
    }
}
