namespace Extensions;

public static class MappingExtensions
{
    public static void Deconstruct(this long[] value, out long v1, out long v2, out long v3)
    {
        if (value.Length < 3)
            throw new ArgumentException("Array must have at least 3 elements", nameof(value));
        v1 = value[0];
        v2 = value[1];
        v3 = value[2];
    }

    public static List<RangeNode> ParseSeeds(this string[] input)
    {
        // Extract and parse seed numbers
        var seedLine = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return seedLine.Skip(1).Select(long.Parse).ToList().Select(s => new RangeNode(s, s + 1)).ToList();
    }

    public static List<RangeNode> ParseSeedRanges(this string[] input)
    {
        var seedLine = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var seedRanges = new List<RangeNode>();
        for (int i = 1; i < seedLine.Length; i += 2)
        {
            long start = long.Parse(seedLine[i]);
            long length = long.Parse(seedLine[i + 1]);
            seedRanges.Add(new RangeNode(start, start+length));
        }
        return seedRanges;
    }

    public static Dictionary<string, List<MappingEdge>> ParseMappings(this string[] input)
    {
        var mappings = new Dictionary<string, List<MappingEdge>>();
        string currentCategory = "";

        foreach (var line in input.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.Contains("map:"))
            {
                currentCategory = line.Split(':')[0];
                mappings[currentCategory] = [];
            }
            else
            {
                (long destStart, long sourceStart, long length) = line.Split(' ').Select(long.Parse).ToArray();

                mappings[currentCategory].Add(new MappingEdge(sourceStart, destStart, length));
            }
        }
        return mappings;
    }
}
