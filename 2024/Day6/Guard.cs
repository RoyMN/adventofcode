namespace AdventOfCode2024.Day6;

public class Guard((int r, int c) position, (int h, int v) direction)
{
    public List<(int r, int c)> Path = [position];
    public (int h, int v)[] Directions =
    [
        (-1, 0), (0, 1), (1, 0), (0, -1)
    ];

    public (int r, int c) Position => position;
    public (int h, int v) Direction => direction;

    public (int h, int v) Turn()
    {
        var newdir = Directions[(Array.IndexOf(Directions, direction) + 1) % 4];
        direction = newdir;
        return direction;
    }

    public (int r, int c) Move()
    {
        position = (position.r + direction.h, position.c + direction.v);
        Path.Add(position);
        return position;
    }

    public static bool HasBlockInPath((int r, int c) position, (int h, int v) direction, List<(int r, int c)> blocks)
    {
        var nextpos = (position.r + direction.h, position.c + direction.v);
        var blocked = blocks.Contains(nextpos);
        return blocked;
    }

    public static bool TryGetDirection(char? c, out (int h, int c) direction)
    {
        direction = c switch
        {
            '^' => (-1, 0),
            '<' => (0, -1),
            '>' => (0, 1),
            'v' => (1, 0),
            _ => (0, 0)
        };
        return direction != (0, 0);
    }

    public Guard Copy()
    {
        return new Guard(position, direction);
    }
}

public static class GuardExtensions
{
    public static bool IsBlocked(this Guard guard, List<(int r, int c)> blocks)
    {
        var nextpos = guard.NextPosition();
        var blocked = blocks.Contains(nextpos);
        return blocked;
    }

    public static (int r, int c) NextPosition(this Guard guard)
    {
        return (guard.Position.r + guard.Direction.h, guard.Position.c + guard.Direction.v);
    }
}