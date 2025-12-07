using AdventOfCodeTools;

namespace AdventOfCode2025.Day7;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = sample ? "./2025/Day7/sample.txt" : "./2025/Day7/input.txt";
        var logger = new ConsoleWriter(logLevel);

        char[][] grid = [.. File.ReadAllLines(fileName).Select(l => l.ToCharArray())];

        // S should be on first line, first split directly below
        int startCol = Array.IndexOf(grid[0], 'S');
        if (startCol < 0) return "Total: 0";
        int startRow = 0;
        while (startRow < grid.Length)
        {
            if (grid[startRow][startCol] == '^') break;
            startRow++;
        }

        // visited: avoid walking same paths multiple times
        // splitPositions: store unique split positions
        var visited = new HashSet<(int r, int c)>();
        var splitPositions = new HashSet<(int r, int c)>();

        Helpers.FinSplitsFrom(startRow, startCol, grid, visited, splitPositions);
        return $"Total: {splitPositions.Count}";
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
        string fileName = sample ? "./2025/Day7/sample.txt" : "./2025/Day7/input.txt";
        var logger = new ConsoleWriter(logLevel);

        char[][] grid = [.. File.ReadAllLines(fileName).Select(l => l.ToCharArray())];

        int startCol = Array.IndexOf(grid[0], 'S');
        if (startCol < 0) return "Total: 0";

        var memo = new Dictionary<(int r, int c), long>();

        long totalPaths = Helpers.CountPathsFrom(0, startCol, grid, memo);
        return $"Total: {totalPaths}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}

public static class Helpers
{
    // walk down from (r, c), adding splits to splitPositions
    public static void FinSplitsFrom(int r, int c, char[][] grid, HashSet<(int r, int c)> visited, HashSet<(int r, int c)> splitPositions)
    {
        if (c < 0 || c >= grid[r].Length) return;
        if (!visited.Add((r, c))) return;

        int row = r;
        while (row < grid.Length)
        {
            // If we hit a splitter, split and recurse
            if (grid[row][c] == '^')
            {
                splitPositions.Add((row, c));
                int leftCol = c - 1;
                int rightCol = c + 1;

                FinSplitsFrom(row, leftCol, grid, visited, splitPositions);
                FinSplitsFrom(row, rightCol, grid, visited, splitPositions);
                return;
            }
            row++;
        }
        return;
    }

    public static long CountPathsFrom(int r, int c, char[][] grid, Dictionary<(int r, int c), long> memo)
    {
        if (r < 0 || r >= grid.Length) return 0;
        if (c < 0 || c >= grid[r].Length) return 0;

        if (memo.TryGetValue((r, c), out var cached)) return cached;

        int row = r;
        while (row < grid.Length)
        {
            if (grid[row][c] == '^')
            {
                int leftCol = c - 1;
                int rightCol = c + 1;

                long left = CountPathsFrom(row, leftCol, grid, memo);
                long right = CountPathsFrom(row, rightCol, grid, memo);

                long sum = left + right;
                memo[(r, c)] = sum;
                return sum;
            }
            row++;
        }
        memo[(r, c)] = 1;
        return 1;
    }
}