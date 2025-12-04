using AdventOfCodeTools;

namespace AdventOfCode2025.Day4;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        string fileName = sample ? "./2025/Day4/sample.txt" : "./2025/Day4/input.txt";
        var logger = new ConsoleWriter(logLevel);
        var lines = File.ReadAllLines(fileName);
        int rows = lines.Length;
        if (rows == 0) throw new Exception("Input file is empty");
        int cols = lines[0].Length;

        (int r, int c)[] directions =
        [
            (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)
        ];

        for (int r = 0; r < rows; r++)
        {
            var line = lines[r];
            for (int c = 0; c < line.Length; c++)
            {
                if (line[c] != '@') continue;
                int neighbors = 0;
                for (int k = 0; k < 8; k++)
                {
                    (var dr, var dc) = directions[k];
                    int nr = r + dr;
                    int nc = c + dc;
                    if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                    if (lines[nr][nc] == '@') neighbors++;
                }
                if (neighbors < 4) sum++;
            }
        }

        return @$"
Amount of paper rolls that can be accessed:
{sum}
";
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
        long removedTotal = 0;
        string fileName = sample ? "./2025/Day4/sample.txt" : "./2025/Day4/input.txt";
        var logger = new ConsoleWriter(logLevel);
        var lines = File.ReadAllLines(fileName);
        int rows = lines.Length;
        if (rows == 0) throw new Exception("Input file is empty");
        int cols = lines[0].Length;

        var grid = lines.Select(l => l.ToCharArray()).ToArray();

        var directions = new (int dr, int dc)[]
        {
            (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)
        };

        while (true)
        {
            var toRemove = new List<(int r, int c)>();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r][c] != '@') continue;
                    int neighbors = 0;
                    for (int k = 0; k < directions.Length; k++)
                    {
                        var (dr, dc) = directions[k];
                        int nr = r + dr;
                        int nc = c + dc;
                        if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                        if (grid[nr][nc] == '@') neighbors++;
                    }
                    if (neighbors < 4) toRemove.Add((r, c));
                }
            }

            if (toRemove.Count == 0) break;

            foreach (var (r, c) in toRemove)
            {
                grid[r][c] = '.';
            }

            removedTotal += toRemove.Count;
        }

        return @$"
Amount of paper rolls that can be removed:
{removedTotal}
";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}