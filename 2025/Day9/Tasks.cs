using AdventOfCodeTools;

namespace AdventOfCode2025.Day9;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        var fileName = sample ? "./2025/Day9/sample.txt" : "./2025/Day9/input.txt";
        using var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        List<(long x, long y)> corners = [];
        long largestArea = 0;

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            (long x, long y) = line.ToLongTouple(',');
            corners.Add((x, y));
        }

        // compute all pairwise areas and find the largest one
        for (int i = 0; i < corners.Count; i++)
        {
            var (ax, ay) = corners[i];
            for (int j = i + 1; j < corners.Count; j++)
            {
                var (bx, by) = corners[j];
                // not zero-based
                long area = Math.Abs(ax - bx + 1) * Math.Abs(ay - by + 1);
                if (area > largestArea)
                {
                    largestArea = area;
                }
            }
        }

        return $"The largest rectange has an area of {largestArea}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        var fileName = sample ? "./2025/Day9/sample.txt" : "./2025/Day9/input.txt";
        using var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        List<(long x, long y)> corners = [];
        long largestArea = 0;

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            (long x, long y) = line.ToLongTouple(',');
            corners.Add((x, y));
        }

        throw new NotImplementedException();
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}