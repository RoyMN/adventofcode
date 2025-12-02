using AdventOfCodeTools;

namespace AdventOfCode2025.Day2;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        string fileName = sample ? "./2025/Day2/sample.txt" : "./2025/Day2/input.txt";
        var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        var line = sr.ReadLine() ?? throw new Exception("Input file is empty");
        var visited = new HashSet<long>();

        var ranges = line.Split(',').Select(r =>
        {
            var parts = r.Split('-');
            return (long.Parse(parts[0]), long.Parse(parts[1]));
        });
        foreach (var (start, end) in ranges)
        {
            for (long i = start; i <= end; i++)
            {
                if (visited.Contains(i))
                {
                    continue;
                }
                visited.Add(i);
                var asString = i.ToString();
                if (asString.Length % 2 != 0)
                {
                    continue;
                }
                var left = asString[..(asString.Length / 2)];
                var right = asString[(asString.Length / 2)..];
                if (left.Equals(right))
                {
                    sum += i;
                }
            }
        }
        return $"Sum of all matching numbers: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        // Implementation for running examples goes here
        throw new NotImplementedException();
    }
}

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        string fileName = sample ? "./2025/Day2/sample.txt" : "./2025/Day2/input.txt";
        var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        var line = sr.ReadLine() ?? throw new Exception("Input file is empty");
        var visited = new HashSet<long>();

        var ranges = line.Split(',').Select(r =>
        {
            var parts = r.Split('-');
            return (long.Parse(parts[0]), long.Parse(parts[1]));
        });
        foreach (var (start, end) in ranges)
        {
            for (long i = start; i <= end; i++)
            {
                if (visited.Contains(i))
                {
                    continue;
                }
                visited.Add(i);
                var asString = i.ToString();
                if (asString.Length < 2)
                {
                    continue;
                }

                if (Repeats(asString))
                {
                    sum += i;
                }
            }
        }
        return $"Sum of all matching numbers: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        // Implementation for running examples goes here
        throw new NotImplementedException();
    }

    private static bool Repeats(string s)
    {
        int n = s.Length;
        for (int unit = 1; unit <= n / 2; unit++)
        {
            // skip if not evenly divisible
            if (n % unit != 0) continue;
            var pattern = s[..unit];
            bool ok = true;
            for (int pos = unit; pos < n; pos += unit)
            {
                // does not repeat if any block does not match
                if (!s.Substring(pos, unit).Equals(pattern))
                {
                    ok = false;
                    break;
                }
            }
            if (ok) return true;
        }
        return false;
    }
}