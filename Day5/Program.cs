using System.Diagnostics;

var example = false;

//Part1();
Part2();

void Part1()
{
    var input = example ? File.ReadAllLines("./Day5.example-1") : File.ReadAllLines("./Day5.input-1");
    var res = SolvePart1(input);
    Console.WriteLine(res);
}

void Part2()
{
    var input = example ? File.ReadAllLines("./Day5.example-2") : File.ReadAllLines("./Day5.input-2");
    var sw = Stopwatch.StartNew();
    var res = SolvePart2(input);
    sw.Stop();
    Console.WriteLine($"{res} found in {sw.ElapsedMilliseconds}ms");
}

int SolvePart1(string[] input)
{
    var seeds = ParseSeeds(input);
    var mappings = ParseMappings(input[1..]);
    var lowestLocation = GetLowestLocation(seeds, mappings);
    return lowestLocation;
}

long SolvePart2(string[] input)
{
    var seeds = ParseSeedRanges(input);
    var mappings = ParseMappingsMulti(input[1..]); // Skipping the first line (seeds line)
    var lowestLocation = GetLowestLocationMulti(seeds, mappings);

    // Only for validation. Takes a long time to run, bruteforce checking all numbers up to lowestLocation (naive checking, assumes traversing is done correctly)
    // bool isValid = TestMappings(mappings, seeds, lowestLocation);
    // var msg = isValid ? $"Mapping to {lowestLocation} is indeed the lowest possible mapping." : "Mapping to a lower number is possible.";
    // Console.WriteLine(msg);
    return lowestLocation;
}

static List<int> ParseSeeds(string[] input)
{
    // Extract and parse seed numbers
    var seedLine = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return seedLine.Skip(1).Select(int.Parse).ToList();
}

static List<Range> ParseSeedRanges(string[] input)
{
    var seedLine = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var ranges = new List<Range>();
    for (int i = 1; i < seedLine.Length; i += 2)
    {
        // Read start and length of each seed range
        ranges.Add(new Range(long.Parse(seedLine[i]),long.Parse(seedLine[i + 1])));
    }
    return ranges;
}

static Dictionary<string, List<RangeMapping>> ParseMappingsMulti(string[] input)
{
    var mappings = new Dictionary<string, List<RangeMapping>>();
    string currentMap = "";
    foreach (var line in input)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        if (line.Contains("map:"))
        {
            currentMap = line.Split(':')[0];
            mappings[currentMap] = new List<RangeMapping>();
        }
        else
        {
            var parts = line.Split(' ').Select(long.Parse).ToArray();
            mappings[currentMap].Add(new RangeMapping(parts[1], parts[0], parts[2]));
        }
    }

    return mappings;
}

static Dictionary<string, Dictionary<int, int>> ParseMappings(string[] input)
{
    var mappings = new Dictionary<string, Dictionary<int, int>>();
    string currentMap = "";
    foreach (var line in input)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        if (line.Contains("map:"))
        {
            currentMap = line.Split(':')[0];
            mappings[currentMap] = [];
        }
        else
        {
            var parts = line.Split(' ').Select(int.Parse).ToArray();
            int destStart = parts[0];
            int srcStart = parts[1];
            int range = parts[2];

            for (int i = 0; i < range; i++)
            {
                mappings[currentMap][srcStart + i] = destStart + i;
            }
        }
    }

    return mappings;
}

static int GetLowestLocation(List<int> seeds, Dictionary<string, Dictionary<int, int>> mappings)
{
    int lowestLocation = int.MaxValue;
    foreach (var seed in seeds)
    {
        int current = seed;
        foreach (var map in mappings)
        {
            current = map.Value.ContainsKey(current) ? map.Value[current] : current;
        }
        lowestLocation = Math.Min(lowestLocation, current);
    }
    return lowestLocation;
}

/**
* Gets the lowest location that maps to a seed number.
* This is done by transforming each seed number through the mappings, and then finding the lowest location among the transformed numbers.
* It's done inner-out, so that the lowest location is found first.
*/
static long GetLowestLocationMulti(List<Range> seedRanges, Dictionary<string, List<RangeMapping>> mappings)
{
    var allTransformedRanges = new List<Range>();

    foreach (var seedRange in seedRanges)
    {
        var transformedRange = new List<Range> { seedRange };

        foreach (var map in mappings)
        {
            var nextTransformedRange = new List<Range>();

            foreach (var range in transformedRange)
            {
                nextTransformedRange.AddRange(TransformRange(range, map.Value));
            }

            transformedRange = MergeRanges(nextTransformedRange);
        }

        allTransformedRanges.AddRange(transformedRange);
    }

    // Find the lowest starting value among all ranges
    return allTransformedRanges.Min(r => r.Start);
}

/**
* Transforms a range to its list of transformed ranges, based on a list of mappings.
*/
static List<Range> TransformRange(Range range, List<RangeMapping> mappings)
{
    var transformedRanges = new List<Range>();
    long currentStart = range.Start;

    foreach (var mapping in mappings)
    {
        long mappingEnd = mapping.SourceStart + mapping.Length;
        long rangeEnd = range.Start + range.Length;

        // Check for overlap
        if (currentStart < mappingEnd && rangeEnd > mapping.SourceStart)
        {
            // Handle the part before the overlap
            if (currentStart < mapping.SourceStart)
            {
                transformedRanges.Add(new Range(currentStart, mapping.SourceStart - currentStart));
                currentStart = mapping.SourceStart;
            }

            // Handle the overlapping part
            long overlapEnd = Math.Min(mappingEnd, rangeEnd);
            long offset = currentStart - mapping.SourceStart;
            transformedRanges.Add(new Range(mapping.DestStart + offset, overlapEnd - currentStart));
            currentStart = overlapEnd;
        }

        // Break if the rest of the range is beyond this mapping
        if (currentStart >= rangeEnd) break;
    }

    // Handle any remaining part of the range after the last mapping
    if (currentStart < range.Start + range.Length)
    {
        transformedRanges.Add(new Range(currentStart, range.Start + range.Length - currentStart));
    }

    return transformedRanges;
}

/**
 * Merges a list of ranges into a list of merged ranges.
 * Requires the input ranges to be sorted by start value.
 * Example: [0, 5], [5, 5], [10, 5] -> [0, 10], [10, 5]
 *
 * Should not exist in the dataset, so probably not needed here.
 *
 */
static List<Range> MergeRanges(List<Range> ranges)
{
    if (ranges.Count <= 1) return ranges;

    // Sort ranges by start value
    ranges.Sort((a, b) => a.Start.CompareTo(b.Start));

    var mergedRanges = new List<Range>();
    var currentRange = ranges[0];

    foreach (var range in ranges.Skip(1))
    {
        if (currentRange.Start + currentRange.Length >= range.Start)
        {
            // Extend the current range if it overlaps or is contiguous with the next range
            currentRange = new Range(currentRange.Start, Math.Max(currentRange.Start + currentRange.Length, range.Start + range.Length) - currentRange.Start);
        }
        else
        {
            mergedRanges.Add(currentRange);
            currentRange = range;
        }
    }

    // Add the last range
    mergedRanges.Add(currentRange);

    return mergedRanges;
}

#region Validation code
static bool TestMappings(Dictionary<string, List<RangeMapping>> mappings, List<Range> seedRanges, long testUpTo)
{
    var reverseMappings = CreateReverseMappings(mappings);

    for (long location = 0; location <= testUpTo; location++)
    {
        long seed = ReverseTransform(location, reverseMappings);
        if (IsWithinSeedRanges(seed, seedRanges))
        {
            return false; // Found a lower location that maps back to a valid seed range
        }
    }

    return true; // No lower location maps back to a valid seed range
}

static Dictionary<string, List<RangeMapping>> CreateReverseMappings(Dictionary<string, List<RangeMapping>> mappings)
{
    var reverseMappings = new Dictionary<string, List<RangeMapping>>();
    foreach (var map in mappings)
    {
        reverseMappings[map.Key] = new List<RangeMapping>();
        foreach (var rangeMapping in map.Value)
        {
            reverseMappings[map.Key].Add(new RangeMapping(rangeMapping.DestStart, rangeMapping.SourceStart, rangeMapping.Length));
        }
    }
    return reverseMappings;
}

static long ReverseTransform(long number, Dictionary<string, List<RangeMapping>> reverseMappings)
{
    foreach (var map in reverseMappings)
    {
        foreach (var mapping in map.Value)
        {
            long mappingEnd = mapping.SourceStart + mapping.Length;
            if (number >= mapping.SourceStart && number < mappingEnd)
            {
                long offset = number - mapping.SourceStart;
                number = mapping.DestStart + offset;
                break; // Break the inner loop once a mapping is applied
            }
        }
    }
    return number;
}

static bool IsWithinSeedRanges(long number, List<Range> seedRanges)
{
    foreach (var range in seedRanges)
    {
        if (number >= range.Start && number < range.Start + range.Length)
        {
            return true; // Number is within a seed range
        }
    }
    return false;
}

#endregion

#region Models
record Range(long Start, long Length);
record RangeMapping(long SourceStart, long DestStart, long Length);
#endregion