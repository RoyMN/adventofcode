using AdventOfCodeTools;

namespace AdventOfCode2025.Day5;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = sample ? "./2025/Day5/sample.txt" : "./2025/Day5/input.txt";
        var logger = new ConsoleWriter(logLevel);
        var ranges = new List<(long Start, long End)>();
        var sum = 0;
        var readingRanges = true;

        using var sr = new StreamReader(fileName);
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                logger.Info("Finished reading ranges.");
                readingRanges = false;
                continue;
            }

            if (readingRanges)
            {
                logger.Info($"Adding range: {line}");
                var parts = line.Split('-', 2);
                if (parts.Length == 2 && long.TryParse(parts[0].Trim(), out var s) && long.TryParse(parts[1].Trim(), out var e))
                    ranges.Add((s, e));
                else
                    logger.Warn($"Invalid range line: '{line}'");
            }
            else
            {
                logger.Info($"Checking value: {line}");
                if (!long.TryParse(line.Trim(), out var value))
                {
                    logger.Warn($"Invalid value line: '{line}'");
                    continue;
                }

                if (ranges.Any(r => r.Start <= value && value <= r.End))
                {
                    logger.Info($"Value {value} is within one of the ranges.");
                    sum++;
                }
                else
                {
                    logger.Info($"Value {value} is NOT within any of the ranges.");
                }
            }
        }

        return $"Number of fresh items: {sum}";
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
        string fileName = sample ? "./2025/Day5/sample.txt" : "./2025/Day5/input.txt";
        var logger = new ConsoleWriter(logLevel);
        var merged = new List<(long Start, long End)>();

        using var sr = new StreamReader(fileName);
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                logger.Info("Finished reading ranges. Stopping.");
                break;
            }

            logger.Info($"Processing range: {line}");
            var parts = line.Split('-', 2);
            if (
                parts.Length != 2 ||
                !long.TryParse(parts[0].Trim(), out var s) ||
                !long.TryParse(parts[1].Trim(), out var e)
            )
            {
                logger.Error($"Invalid range line: '{line}'");
                throw new InvalidDataException($"Invalid range line: '{line}'");
            }

            int idx = 0;

            // Find ranges that are before the new range (no overlap) and skip them
            while (idx < merged.Count && merged[idx].End + 1 < s)
                idx++;

            // Merge ranges that overlap/are adjacent
            while (idx < merged.Count && merged[idx].Start <= e + 1)
            {
                s = Math.Min(s, merged[idx].Start);
                e = Math.Max(e, merged[idx].End);
                merged.RemoveAt(idx);
            }
            merged.Insert(idx, (s, e));
        }

        long total = 0;
        foreach (var (Start, End) in merged)
        {
            total += End - Start + 1;
        }

        return $"Total unique integers in ranges: {total}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}