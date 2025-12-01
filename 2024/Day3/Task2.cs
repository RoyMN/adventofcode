using System.Text.RegularExpressions;
using AdventOfCodeTools;

namespace AdventOfCode2024.Day3;

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = "./2024/Day3/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);

        string input = sr.ReadToEnd();
        sr.Close();

        List<(int start, bool enabled)> ranges = [];
        ranges.Add((0, true)); // First range is always enabled

        foreach (Match match in RegexHelpers.controlRegex().Matches(input))
        {
            if (match.Value == "do()")
            {
                ranges.Add((match.Index, true));
            }
            else if (match.Value == "don't()")
            {
                ranges.Add((match.Index, false));
            }
        }

        logger.Debug($"Ranges: {string.Join(", ", ranges.Select(r => $"[{r.start}: {r.enabled})"))}");

        int sum = 0;
        foreach (Match match in RegexHelpers.multRegex().Matches(input))
        {
            int mulStart = match.Index;
            int mulEnd = match.Index + match.Length;

            (int start, bool enabled)? leftRange = ranges.Where(r => r.start <= mulStart).LastOrDefault();

            var invalidRangeMsg = $"No range found to the left of the match-index: {match.Index}";
            (int start, bool enabled) = leftRange ?? throw new ArgumentOutOfRangeException(invalidRangeMsg);

            if (!enabled)
            {
                logger.Debug($"Skipping {match.Value} as mul instructions are disabled");
                continue;
            }

            if (match.Groups.Count == 3 &&
                int.TryParse(match.Groups[1].Value, out int left) &&
                int.TryParse(match.Groups[2].Value, out int right))
            {
                logger.Debug($"Match found: {match.Value}: {left} * {right} = {left * right}");
                sum += left * right;
            }
        }

        return $"Sum of all multiplications: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}