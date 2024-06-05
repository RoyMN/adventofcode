namespace Extensions;

using System.Text.RegularExpressions;
using Dir = (int dx, int dy);
using Point = (int x, int y);
using static System.Environment;
using MoreLinq;

public static class MappingExtensions
{
    public static List<string> SplitToLines(this string input) => Regex.Split(input, NewLine).Where(ln => ln != "").ToList();
    public static List<string> Tokenize(this string line, IEnumerable<char> splitChars) => [.. line.Split(splitChars.ToArray(), StringSplitOptions.RemoveEmptyEntries)];
    public static Dir East => (1, 0);
    public static Dir West => (-1, 0);
    public static Dir North => (0, -1);
    public static Dir South => (0, 1);
    public static List<Dir> PossibleDirections(this char ch)
    {
        return ch switch
        {
            '|' => [North, South],
            '-' => [East, West],
            'L' => [North, East],
            'J' => [North, West],
            '7' => [South, West],
            'F' => [South, East],
            '.' => [],
            _ => throw new Exception($"Unknown direction {ch}")
        };
    }
    public static Dictionary<Point, char> ToMap(this string input, bool filterEmpty=true)
    {
        return input.SplitToLines()
            .Index()
            .SelectMany(pair => pair is not (int y, string ln) ? throw new() : ln.ToCharArray().Index().Select(pairX => ((pairX.Key, y), pairX.Value)))
            .Where(pair => !filterEmpty || pair.Value != '.')
            .ToDictionary(x => x.Item1, x => x.Value);
    }

    public static Dictionary<Point, char> Expand(this Dictionary<Point, char> map)
    {
        Dictionary<Point, char> expandedMap = map.ToDictionary(kvp => (kvp.Key.x * 2, kvp.Key.y * 2), kvp => kvp.Value);
        return expandedMap;
    }

    public static List<Point> FindEnclosedPoints(Point startPoint, Dictionary<Point, char> expandedMap, List<Point> expandedMainLoopPts)
    {
        foreach (Point pt in expandedMap.Keys.ToList())
        {
            if (!expandedMainLoopPts.Contains(pt) && expandedMap[pt] != '.') { expandedMap[pt] = '.'; }
        }

        HashSet<Point> escapablePoints = GetEscapablePoints(expandedMap);

        var walkablePoints = expandedMap
            .Where(kvp => kvp.Key.x % 2 == 0 && kvp.Key.y % 2 == 0) // original (not expanded) points only
            .Select(kvp => kvp.Key)
            .Except(expandedMainLoopPts) // exclude main loop
            .ToList();

        var enclosedPoints = walkablePoints.Except(escapablePoints).ToList();
        return enclosedPoints;
    }

    private static HashSet<(int x, int y)> GetEscapablePoints(Dictionary<Point, char> expandedMap)
    {
        HashSet<Point> expandedWalls = MapToWalls(expandedMap).ToHashSet();

        int maxX = expandedMap.Keys.Max(pt => pt.x);
        int maxY = expandedMap.Keys.Max(pt => pt.y);

        HashSet<Point> escapablePoints = new();

        for (int x = -1; x <= maxX + 1; x++) { escapablePoints.Add((x, -1)); escapablePoints.Add((x, maxY + 1)); }
        for (int y = -1; y <= maxY + 1; y++) { escapablePoints.Add((-1, y)); escapablePoints.Add((maxX + 1, y)); }
        List<Point> nextEdges = escapablePoints.SelectMany(pt => AllDirections.Select(d => Move(pt, d))).Distinct().ToList();
        while (true)
        {
            nextEdges = nextEdges
                .Where(pt => !expandedWalls.Contains(pt))
                .Where(pt => pt.x >= 0 && pt.x <= maxX && pt.y >= 0 && pt.y <= maxY)
                .ToList();

            if (!nextEdges.Any()) { break; }
            foreach (var edge in nextEdges) { escapablePoints.Add(edge); }

            List<Point> candidatesAfter = nextEdges.SelectMany(edge =>
                new[] { Move(edge, South), Move(edge, North), Move(edge, East), Move(edge, West) }
                ).ToList();
            List<Point> edgesAfter = candidatesAfter.Where(pt => !escapablePoints.Contains(pt)).Distinct().ToList();
            nextEdges = edgesAfter.Distinct().ToList();
        }

        return escapablePoints;
    }

    private static List<Point> MapToWalls(Dictionary<Point, char> expandedMap)
    {
        HashSet<Point> existingExpanded = new(expandedMap.Keys);

        int maxX = existingExpanded.Max(pt => pt.x);
        int maxY = existingExpanded.Max(pt => pt.y);

        List<Point> walls = expandedMap.Where(kvp => kvp.Value != '.').Select(kvp => kvp.Key).ToList();

        for (int x = 0; x <= maxX; x++)
        {
            for (int y = 0; y <= maxY; y++)
            {
                Point pt = (x, y);
                if (existingExpanded.Contains(pt)) { continue; }

                List<Dir> allDirs = [North, East, South, West];
                List<Point> movablePointsFromNeighbours =
                    allDirs
                    .Select(dir => Move(pt, dir))
                    .Where(expandedMap.ContainsKey)
                    .SelectMany(newPt => PossibleDirections(expandedMap[newPt]).Select(expDir => Move(newPt, expDir)))
                    .ToList();

                if (movablePointsFromNeighbours.Where(pti => pti == pt).Count() >= 2)
                {
                    walls.Add(pt);
                }
            }
        }
        return walls;
    }

    public static List<Dir> AllDirections => [North, South, East, West];

    public static Point Move(this Point point, Dir dir) => (point.x + dir.dx, point.y + dir.dy);

    public static char GetStartChar(this Point startPoint, Dictionary<Point, char> map)
    {
        List<Dir> startDirs =
            new[] {North, South, East, West}
            .Where(dir =>
                Move(startPoint, dir) is Point nextPos
                && map.ContainsKey(nextPos)
                && map[nextPos] is char nextCh
                && nextCh.PossibleDirections().Select(d => nextPos.Move(d)).Contains(startPoint))
            .ToList();
        
        return startDirs.OrderBy(d => d.dx).ThenBy(d => d.dy).ToList() switch
        {
            [Dir a, Dir b] when a == North && b == South => '|',
            [Dir a, Dir b] when a == West && b == East => '-',
            [Dir a, Dir b] when a == North && b == East => 'L',
            [Dir a, Dir b] when a == West && b == North => 'J',
            [Dir a, Dir b] when a == West && b == South => '7',
            [Dir a, Dir b] when a == South && b == East => 'F',
            _ => throw new Exception($"Unknown direction for start point {startPoint}")
        };
    }

    public static (int steps, List<Point> mainLoopPoints) TraverseMainLoop(Point startPoint, Dictionary<Point, char> map)
    {
        int steps = 0;
        HashSet<Point> alreadyTaken = new([startPoint]);
        List<Point> nextEdges = PossibleDirections(map[startPoint]).Select(d => startPoint.Move(d)).ToList();
        while (true)
        {
            if (!nextEdges.Any()) { break; }
            foreach (var edge in nextEdges) { alreadyTaken.Add(edge); }

            List<Point> candidatesAfter = nextEdges.SelectMany(edge =>
                map.ContainsKey(edge)
                ? PossibleDirections(map[edge]).Select(dir => edge.Move(dir))
                : []
                ).ToList();
            List<Point> edgesAfter = candidatesAfter.Where(pt => !alreadyTaken.Contains(pt)).Distinct().ToList();
            nextEdges = edgesAfter;

            steps++;
        }

        return (steps, alreadyTaken.ToList());
    }
}