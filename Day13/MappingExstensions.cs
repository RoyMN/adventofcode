namespace Extensions;

public static class MappingExtensions
{
    public static bool IsMirrored(this string[] input, int rowIdx)
    {
        if (rowIdx == input.Length - 1) return false;
        var shortestPath = Math.Min(rowIdx,input.Length - rowIdx);
        for (int i = 0; i < shortestPath; i++)
        {
            if (input[rowIdx - 1] != input[rowIdx + 1 + i]) return false;
        }
        return true;
    }
}