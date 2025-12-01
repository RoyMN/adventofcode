namespace AdventOfCode2024.Day6;

public static class BlockPlacement
{
    public static List<(int r, int c)> FindCircularBlockPlacements(List<((int r, int c) block, (int r, int c) character)> blocksInPath)
    {
        var validPlacements = new List<(int r, int c)>();

        // Extract only the block positions for combinations
        var blockPositions = blocksInPath.Select(bp => bp.block).ToList();

        // Generate all combinations of 3 block positions
        var combinations = GetCombinations(blockPositions, 3);

        foreach (var combo in combinations)
        {
            // Find the character positions corresponding to the selected blocks
            var characterPositions = blocksInPath
                .Where(bp => combo.Contains(bp.block))
                .Select(bp => bp.character)
                .ToList();

            // Calculate the potential fourth corner of the rectangle
            var fourthCorner = CalculateFourthCorner(characterPositions[0], characterPositions[1], characterPositions[2]);

            if (fourthCorner.HasValue)
            {
                validPlacements.Add(fourthCorner.Value);
            }
        }

        return validPlacements;
    }

    private static List<List<T>> GetCombinations<T>(List<T> list, int length)
    {
        // Recursive combinations generator
        if (length == 0) return [[]];
        if (list.Count == 0) return [];

        var result = new List<List<T>>();

        var head = list[0];
        var tail = list.Skip(1).ToList();

        foreach (var combination in GetCombinations(tail, length - 1))
        {
            var newCombination = new List<T> { head };
            newCombination.AddRange(combination);
            result.Add(newCombination);
        }

        result.AddRange(GetCombinations(tail, length));
        return result;
    }

    private static List<(int r, int c)> GetInnerPoints(List<(int r, int c)> blocks)
    {
        List<(int r, int c)> innerPoints = [];

        return innerPoints;
    }

    private static (int r, int c)? CalculateFourthCorner((int r, int c) b1, (int r, int c) b2, (int r, int c) b3)
    {
        // Identify the fourth corner using rectangle logic:
        // b4 = (b1.r ^ b2.r ^ b3.r, b1.c ^ b2.c ^ b3.c)
        var possibleR = b1.r ^ b2.r ^ b3.r;
        var possibleC = b1.c ^ b2.c ^ b3.c;

        // Return the calculated position
        return (possibleR, possibleC);
    }
}