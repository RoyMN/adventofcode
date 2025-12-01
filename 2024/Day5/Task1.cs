using AdventOfCodeTools;

namespace AdventOfCode2024.Day5;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string filename = sample ? "./2024/Day5/sample.txt" : "./2024/Day5/input.txt";
        StreamReader sr = new(filename);
        var readingRules = true;
        ConsoleWriter logger = new(logLevel);
        Dictionary<int, List<int>> rules = [];
        var sum = 0;
        while (StringExtensions.TryReadLine(sr, out string line))
        {
            line = line.Trim();
            if (readingRules)
            {
                if (line == "")
                {
                    readingRules = false;
                    continue;
                }
                else
                {
                    (int l, int r) = line.TakeTwo('|');
                    if (!rules.TryGetValue(l, out var values))
                    {
                        rules[l] = [r];
                    }
                    else
                    {
                        values.Add(r);
                    }
                }
            }
            else
            {
                var updates = line.Split(',').Select(int.Parse);
                var (valid, result) = updates.GetResult(rules, true);

                logger.Debug($"{(valid ? "Valid" : "Invalid")} updates: {string.Join(',', result)}");

                if (valid && result.Count > 0)
                {
                    var middle = result.Count / 2;
                    sum += result[middle];
                }
            }
        }
        return $"The total sum of middle number of valid updates is: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}