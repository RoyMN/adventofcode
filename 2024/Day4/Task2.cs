using AdventOfCodeTools;

namespace AdventOfCode2024.Day4;

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string file = sample ? "./2024/Day4/sample.txt" : "./2024/Day4/input.txt";
        StreamReader sr = new(file);
        string[] grid = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        int totalMatches = CountXPatterns(grid);
        return $"Total '`X`-MAS' occurrences: {totalMatches}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        var grid = example.Split(Environment.NewLine);
        int totalMatches = CountXPatterns(grid);
        return $"Total '`X`-MAS' occurrences: {totalMatches}";
    }

    private static int CountXPatterns(string[] grid)
    {
        var rows = grid.Length;
        var cols = grid[0].Length;

        var count = 0;
        for (var row = 0; row < rows - 2; row++)
        {
            for (int col = 0; col < cols - 2; col++)
            {
                if (grid[row + 1][col + 1] != 'A') continue;
                var diag1 = $"{grid[row][col]}{grid[row + 1][col + 1]}{grid[row + 2][col + 2]}";
                var diag2 = $"{grid[row + 2][col]}{grid[row + 1][col + 1]}{grid[row][col + 2]}";

                if (IsMas(diag1) && IsMas(diag2)) count++;
            }
        }

        return count;
    }

    private static bool IsMas(string test)
    {
        return test == "MAS" || test == "SAM";
    }
}

