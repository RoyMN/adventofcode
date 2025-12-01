namespace AdventOfCodeTools;

public static class Helpers
{
    public static (bool isValid, int direction, int? currentLevel) CheckLevels(this IEnumerable<int> levels, ConsoleWriter cw)
    {
        return levels.Aggregate(
            (isValid: true, direction: 0, prev: (int?)null),
            (state, current) =>
            {
                var direction = state.direction;
                if (!state.isValid) return state;
                if (state.prev.HasValue)
                {
                    int prev = state.prev.Value;
                    if (direction == 0)
                    {
                        direction = prev < current ? 1 : -1;
                    }
                    if (direction == 1 && prev >= current)
                    {
                        return (false, state.direction, current);
                    }
                    else if (direction == -1 && prev <= current)
                    {
                        return (false, state.direction, current);
                    }
                    else if (Math.Abs(prev - current) > 3)
                    {
                        return (false, state.direction, current);
                    }
                }

                return (true, direction, current);
            }
        );
    }
}