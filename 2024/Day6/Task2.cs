using AdventOfCodeTools;

namespace AdventOfCode2024.Day6;

public class Task2 : IDailyRunner
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
        List<((int r, int c) block, (int c, int r) character)> blocksInPath = [];
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

        var possibleLoopBlocks = new List<(int r, int c)>();

        while (guard.Position.r >= 0 && guard.Position.r < rows && guard.Position.c >= 0 && guard.Position.c < cols)
        {
            var blockedAmount = 0;
            while (guard.IsBlocked(blocks))
            {
                blockedAmount++;
                blocksInPath.Add((guard.NextPosition(), guard.Position));
                guard.Turn();
                if (blockedAmount > 2)
                {
                    var free = true;
                    var r = guard.Position.r; var c = guard.Position.c;
                    var threeBlocksAgo = blocksInPath[^3].block;
                    // Look in the direction of the guard, see if you can place a block
                    if (guard.Direction == (-1, 0)) // walking north
                    {
                        // Check if all spots north up until the row of the block from three times ago are free
                        for (int i = r; i >= threeBlocksAgo.r; i--)
                        {
                            if (blocks.Contains((i, c)))
                            {
                                free = false;
                                break;
                            }
                        }
                        if (free)
                        {
                            possibleLoopBlocks.Add((threeBlocksAgo.r - 1, guard.Position.c));
                        }
                    }
                    else if (guard.Direction == (1, 0)) // walking south
                    {
                        for (int i = r; i <= threeBlocksAgo.r; i++)
                        {
                            if (blocks.Contains((i, c)))
                            {
                                free = false;
                                break;
                            }
                        }
                        if (free)
                        {
                            possibleLoopBlocks.Add((threeBlocksAgo.r + 1, guard.Position.c));
                        }
                    }
                    else if (guard.Direction == (0, -1)) // walking west
                    {
                        for (int i = c; i >= threeBlocksAgo.c; i--)
                        {
                            if (blocks.Contains((r, i)))
                            {
                                free = false;
                                break;
                            }
                        }
                        if (free)
                        {
                            possibleLoopBlocks.Add((guard.Position.r, threeBlocksAgo.c - 1));
                        }
                    }
                    else if (guard.Direction == (0, 1)) // walking east
                    {
                        for (int i = c; i <= threeBlocksAgo.c; i++)
                        {
                            if (blocks.Contains((r, i)))
                            {
                                free = false;
                                break;
                            }
                        }
                        if (free)
                        {
                            possibleLoopBlocks.Add((guard.Position.r, threeBlocksAgo.c + 1));
                        }
                    }
                }
            }
            guard.Move();
        }

        logger.Info($"Possible loop blocks:");
        foreach (var (r, c) in possibleLoopBlocks)
        {
            logger.Info($"[{r},{c}]");
        }

        return "";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}