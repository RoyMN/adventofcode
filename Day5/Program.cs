using System.Diagnostics;
using Extensions;

var example = false;

Part1();
Part2();

void Part1()
{
    // Converted to be align with Part2
    var sw = Stopwatch.StartNew();
    var input = example ? File.ReadAllLines("./Day5.example-1") : File.ReadAllLines("./Day5.input-1");
    var seedRanges = input.ParseSeeds();
    var mappings = input.ParseMappings();
    List<RangeNode> finalRanges = TraverseGraph(seedRanges, mappings);
    var lowestLocation = finalRanges.Min(r => r.Start);
    sw.Stop();
    Console.WriteLine($"Lowest Location found (Part1): {lowestLocation} in {sw.ElapsedMilliseconds}ms");
}

void Part2()
{
    var sw = Stopwatch.StartNew();
    var input = example ? File.ReadAllLines("./Day5.example-1") : File.ReadAllLines("./Day5.input-1");
    var seedRanges = input.ParseSeedRanges();
    var mappings = input.ParseMappings();
    List<RangeNode> finalRanges = TraverseGraph(seedRanges, mappings);
    var lowestLocation = finalRanges.Min(r => r.Start);
    sw.Stop();

    Console.WriteLine($"Lowest Location found (Part2), pre-JITed, yay!: {lowestLocation} in {sw.ElapsedMilliseconds}ms");
}

static List<RangeNode> TraverseGraph(List<RangeNode> seedRanges, Dictionary<string, List<MappingEdge>> mappings)
{
    var currentRanges = seedRanges;

    foreach (var category in mappings.Keys)
    {
        var sortedMappings = mappings[category].OrderBy(e => e.SourceStart).ToArray();

        var nextRanges = new List<RangeNode>();
        foreach (RangeNode range in currentRanges)
        {
            var ranges = TransformRange(range, sortedMappings);
            nextRanges.AddRange(ranges);
        }
        currentRanges = nextRanges;
    }
    return currentRanges;
}

static List<RangeNode> TransformRange(RangeNode range, MappingEdge[] mappingEdges)
{
    var transformedRanges = new List<RangeNode>();
    long currentStart = range.Start;

    for (int i = 0; i < mappingEdges.Length; i++)
    {
        if (currentStart == range.End)
        {
            // Nothing more to map
            break;
        }
        long edgeStart = mappingEdges[i].SourceStart;
        long edgeEnd = mappingEdges[i].SourceEnd;
        long edgeDiff = mappingEdges[i].Diff;
        if (currentStart > edgeEnd)
        {
            // Nothing more to map here (but maybe later)
            continue;
        }
        if (currentStart < edgeStart)
        {
            // Map the non-overlapping segment to itself
            transformedRanges.Add(new RangeNode(currentStart, edgeStart));
            currentStart = edgeStart;
        }
        if (currentStart < edgeEnd)
        {
            // Map the overlapping segment
            var segmentEnd = Math.Min(range.End, edgeEnd);
            if (segmentEnd > currentStart)
            {
                transformedRanges.Add(new RangeNode(currentStart+edgeDiff, segmentEnd+edgeDiff));
            }
            currentStart = segmentEnd;
        }
    }
    // If any part of the range remains after all mappings, map it to itself
    if (currentStart < range.End)
    {
        transformedRanges.Add(new RangeNode(currentStart, range.End));
    }

    return transformedRanges;
}

#region Models

public class RangeNode
{
    public long Start { get; set; }
    public long End { get; set; }

    public RangeNode(long start, long end)
    {
        Start = start;
        End = end;
    }
    public override string ToString()
    {
        return $"Start: {Start}, End: {End}";
    }
}

public class MappingEdge(long sourceStart, long destinationStart, long length) : IComparable<MappingEdge>
{
    public long SourceStart { get; set; } = sourceStart;
    public long DestinationStart { get; set; } = destinationStart;
    public long Length { get; set; } = length;
    public long SourceEnd => SourceStart + Length;
    public long Diff => DestinationStart - SourceStart;
    public int CompareTo(MappingEdge? other)
    {
        if (other == null) return 1;
        return SourceStart.CompareTo(other.SourceStart);
    }
    public override string ToString()
    {
        return $"SourceStart: {SourceStart}, DestinationStart: {DestinationStart}, Length: {Length}";
    }
}
#endregion