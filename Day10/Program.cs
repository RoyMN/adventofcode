using Extensions;
using Point = (int x, int y);
using MoreLinq;

var example = false;
var prefix = "day10";

// Part1(example);
Part2(example);

void Part1(bool example)
{
    // https://github.com/blazpeterlin/AdventOfCode2023.CSharp/blob/main/AdventOfCode2023.CSharp/ConsoleApp1/CompletedDays/SolutionDay11.cs
    var input = example ? File.ReadAllText($"./{prefix}-example1") : File.ReadAllText($"./{prefix}-input1");
    Dictionary<Point, char> map = input.ToMap();

    Point startPoint = map.Single(m => m.Value == 'S').Key;
    char startChar = startPoint.GetStartChar(map);
    map[startPoint] = startChar;

    var (steps, _) = MappingExtensions.TraverseMainLoop(startPoint, map);

    Console.WriteLine($"Part 1, number of steps: {steps}");
}

void Part2(bool example)
{
    var input = example ? File.ReadAllText($"./{prefix}-example2") : File.ReadAllText($"./{prefix}-input1");
    Dictionary<Point, char> map = input.ToMap(false);

    Point startPoint = map.Single(m => m.Value == 'S').Key;
    char startChar = MappingExtensions.GetStartChar(startPoint, map);
    map[startPoint] = startChar;

    Dictionary<(int x, int y), char> expandedMap = map.Expand();

    var (_, mainLoopPts) = MappingExtensions.TraverseMainLoop(startPoint, map);
    var expandedMainLoopPts = mainLoopPts.Select(pt => (pt.x * 2, pt.y * 2)).ToList();

    var enclosedPoints = MappingExtensions.FindEnclosedPoints(startPoint, expandedMap, expandedMainLoopPts);

    int res = enclosedPoints.Count;
    
    Console.WriteLine($"Part 2, number of enclosed points: {res}");
}