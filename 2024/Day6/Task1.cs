using AdventOfCodeTools;

namespace AdventOfCode2024.Day6;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string filename = sample ? "./2024/Day6/sample.txt" : "./2024/Day6/input.txt";
        StreamReader sr = new(filename);
        ConsoleWriter logger = new(logLevel);
        string[] lines = sr.ReadToEnd().Split(Environment.NewLine);
        var rows = lines.Length;
        char[][] grid = lines.Select(x => x.ToCharArray()).ToArray();
        var cols = grid[0].Length;
        List<(int r, int c)> blocks = [];
        Guard? guard = null;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[r].Length; c++)
            {
                if (Guard.TryGetDirection(grid[r][c], out var direction))
                {
                    guard = new((r, c), direction);
                }
                else if (grid[r][c] == '#')
                {
                    blocks.Add((r, c));
                }
            }
        }

        if (guard == null)
        {
            throw new NullReferenceException("Guard not found.");
        }

        logger.Info($"Bounds: {rows}x{cols}");
        logger.Info($"Guards direction {guard.Direction}");

        while (guard.Position.r >= 0 && guard.Position.r < rows && guard.Position.c >= 0 && guard.Position.c < cols)
        {
            logger.Info($"Guard at [{guard.Position.r},{guard.Position.c}]");
            while (guard.IsBlocked(blocks))
            {
                guard.Turn();
                logger.Info($"Guard turned to {guard.Direction}");
            }
            guard.Move();
            logger.Info($"Guard moved.");
        }

        logger.Info($"Guard exited bounds at [{guard.Position.r},{guard.Position.c}]");

        // Removing the one out of bound cell
        return $"Guard passed through {guard.Path.Distinct().Count() - 1} unique cells.";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}