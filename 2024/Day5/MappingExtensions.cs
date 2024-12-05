namespace AdventOfCode2024.Day5;

public static class MappingExtensions
{
    public static (bool valid, List<int> previous) GetResult(this IEnumerable<int> updates, Dictionary<int, List<int>> rules, bool earlyExitOnInvalid = true) {
        return updates.Aggregate(
            (valid: true, previous: new List<int>()),
            (state, curr) =>
            {
                if (!state.valid && earlyExitOnInvalid) return state;
                if (rules.TryGetValue(curr, out var rs))
                {
                    if (state.valid) state.valid = !state.previous.Any(l => rs.Contains(l));
                }
                state.previous.Add(curr);
                return state;
            }
        );
    }

    public static void Reorder(this List<int> updates, Dictionary<int, List<int>> rules)
    {
        for (int i = 0; i < updates.Count; i++)
        {
            if (rules.TryGetValue(updates[i], out var rs))
            {
                // get all elements from updates before i
                var before = updates.GetRange(0, i);
                for (int j = 0; j < before.Count; j++)
                {
                    if (rs.Contains(before[j]))
                    {
                        // swap
                        var temp = updates[i];
                        updates[i] = updates[j];
                        updates[j] = temp;
                        break;
                    }
                }
            }
        }
    }
}