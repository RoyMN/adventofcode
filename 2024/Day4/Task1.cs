using System;
using System.Linq;
using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day4;

class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        StreamReader sr = sample ? new("./Day4/sample.txt") : new("./Day4/input.txt");
        string[] grid = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        int totalMatches = CountTotalOccurrences(grid, "XMAS");
        return $"Total 'XMAS' occurrences: {totalMatches}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        var grid = example.Split(Environment.NewLine);
        int totalMatches = CountTotalOccurrences(grid, "XMAS");
        return $"Total 'XMAS' occurrences: {totalMatches}";
    }

    private static int CountTotalOccurrences(string[] grid, string target)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        int targetLength = target.Length;

        // Directions as row and column deltas
        var directions = new (int dRow, int dCol)[]
        {
            (0, 1),    // Right
            (0, -1),   // Left
            (1, 0),    // Down
            (-1, 0),   // Up
            (1, 1),    // Diagonal down-right
            (1, -1),   // Diagonal down-left
            (-1, 1),   // Diagonal up-right
            (-1, -1)   // Diagonal up-left
        };

        // Use LINQ to count matches
        return (
            from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            from dir in directions
            let match = CheckDirection(grid, row, col, dir.dRow, dir.dCol, target)
            where match
            select match
        ).Count();
    }

    private static bool CheckDirection(string[] grid, int startRow, int startCol, int dRow, int dCol, string target)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        int targetLength = target.Length;

        // Traverse the target string along the given direction
        for (int i = 0; i < targetLength; i++)
        {
            int newRow = startRow + i * dRow;
            int newCol = startCol + i * dCol;

            // Check bounds
            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                return false;

            // Check character match
            if (grid[newRow][newCol] != target[i])
                return false;
        }

        return true;
    }
}