using AdventOfCodeTools;

namespace AdventOfCode2025.Day6;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = sample ? "./2025/Day6/sample.txt" : "./2025/Day6/input.txt";
        var logger = new ConsoleWriter(logLevel);

        using var sr = new StreamReader(fileName);
        string? line = sr.ReadLine() ?? throw new InvalidOperationException("Input file is empty.");
        long[] numbers = [.. line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)];
        var n = numbers.Length;
        bool[] operations = new bool[n];
        List<long>[] problems = new List<long>[n];
        for (int i = 0; i < n; i++)
        {
            problems[i] = [numbers[i]];
        }
        long sum = 0;
        while ((line = sr.ReadLine()) != null)
        {
            if (line[0] == '*' || line[0] == '+')
            {
                // Operation line: Do something with the problems
                var ops = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < n; i++)
                {
                    var op = ops[i];
                    if (op == "*")
                    {
                        operations[i] = true;
                    }
                }
                // walk through problems and add to sum (false == +, true == *)
                for (int i = 0; i < n; i++)
                {
                    sum += problems[i].Aggregate
                    (
                        operations[i] ? 1L : 0L,
                        (acc, val) => operations[i] ? acc * val : acc + val
                    );
                }
                break;
            }
            numbers = [.. line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)];
            for (int i = 0; i < n; i++)
            {
                problems[i].Add(numbers[i]);
            }
        }
        return $"Grand total: {sum}";
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
        string fileName = sample ? "./2025/Day6/sample.txt" : "./2025/Day6/input.txt";
        var logger = new ConsoleWriter(logLevel);

        // Read all lines preserving spaces
        var lines = File.ReadAllLines(fileName);
        if (lines.Length == 0) throw new InvalidOperationException("Input file is empty.");

        int rowCount = lines.Length;
        int opRowIndex = rowCount - 1;
        int width = lines[0].Length;

        string[] grid = new string[rowCount];
        for (int r = 0; r < rowCount; r++)
            grid[r] = lines[r];

        // Columns are separated by a column with only spaces
        bool[] isSeparator = new bool[width];
        for (int c = 0; c < width; c++)
        {
            if (grid.All(r => char.IsWhiteSpace(r[c])))
            {
                isSeparator[c] = true;
                continue;
            }
        }

        // Find segments of numbers
        var segments = new List<(int start, int end)>();
        int cIdx = 0;
        while (cIdx < width)
        {
            if (isSeparator[cIdx]) cIdx++;
            if (cIdx >= width) break;
            int start = cIdx;
            // Take until next separator
            while (cIdx < width && !isSeparator[cIdx]) cIdx++;
            int end = cIdx - 1;
            segments.Add((start, end));
        }

        if (segments.Count == 0)
            throw new InvalidOperationException("No problem segments found.");

        long total = 0;

        for (int s = 0; s < segments.Count; s++)
        {
            var (start, end) = segments[s];

            // Operator is in the operation row at the start of the segment
            char opChar = grid[opRowIndex][start];

            // Build numbers from top to bottom per column
            var values = new List<long>();
            for (int c = start; c <= end; c++)
            {
                var sb = new System.Text.StringBuilder();
                for (int r = 0; r < opRowIndex; r++)
                {
                    char ch = grid[r][c];
                    if (!char.IsWhiteSpace(ch))
                        sb.Append(ch);
                }
                long val = long.Parse(sb.ToString());
                values.Add(val);
            }

            var result = values.Aggregate
            (
                opChar == '*' ? 1L : 0L,
                (acc, v) => opChar == '*' ? acc * v : acc + v
            );

            total += result;
        }

        return $"Grand total: {total}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}