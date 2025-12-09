using AdventOfCodeTools;

namespace AdventOfCode2025.Day8;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        var fileName = sample ? "./2025/Day8/sample.txt" : "./2025/Day8/input.txt";
        using var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);

        var N = sample ? 10 : 1000;

        var points = new List<(int x, int y, int z)>();
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            var (x, y, z) = line.TakeThree(',');
            points.Add((x, y, z));
        }

        var edges = new List<(int i, int j, long euclidian)>();
        int n = points.Count;

        // Compute all pairwise euclidian distances
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = points[i].x - points[j].x;
                long dy = points[i].y - points[j].y;
                long dz = points[i].z - points[j].z;
                long euclidian = dx * dx + dy * dy + dz * dz;
                edges.Add((i, j, euclidian));
            }
        }

        // Keep only the N shortest edges
        edges.Sort((a, b) => a.euclidian.CompareTo(b.euclidian));
        if (edges.Count > N) edges = edges.GetRange(0, N);

        // Union-Find to form circuits
        var uf = new UnionFind(n);
        foreach (var (i, j, _) in edges)
        {
            uf.Union(i, j);
        }

        // Compute sizes of all circuits
        var circuits = new Dictionary<int, int>();
        for (int i = 0; i < n; i++)
        {
            int root = uf.Find(i);
            if (!circuits.TryAdd(root, 1))
            {
                circuits[root]++;
            }
        }

        var circuitSizes = circuits.Values.OrderByDescending(s => s).ToList();
        if (circuitSizes.Count < 3) throw new ArgumentException("Less than three circuits found");
        int result = circuitSizes[0] * circuitSizes[1] * circuitSizes[2];

        logger.Info($"Largest three circuit sizes: {circuitSizes[0]}, {circuitSizes[1]}, {circuitSizes[2]}");
        return $"Three largest circuit sizes multiplied together: {result}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}

public class Task2 : IDailyRunner
{
    // https://en.wikipedia.org/wiki/Kruskal%27s_algorithm
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        var fileName = sample ? "./2025/Day8/sample.txt" : "./2025/Day8/input.txt";
        using var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);

        var points = new List<(int x, int y, int z)>();
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            var (x, y, z) = line.TakeThree(',');
            points.Add((x, y, z));
        }

        int n = points.Count;
        if (n == 0) throw new InvalidOperationException("No points provided.");

        // Compute all pairwise euclidian distances
        var edges = new List<(int i, int j, long euclidian)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = points[i].x - points[j].x;
                long dy = points[i].y - points[j].y;
                long dz = points[i].z - points[j].z;
                long euclidian = dx * dx + dy * dy + dz * dz;
                edges.Add((i, j, euclidian));
            }
        }

        edges.Sort((a, b) => a.euclidian.CompareTo(b.euclidian));

        var uf = new UnionFind(n);
        int unionsMade = 0;
        (int i, int j) lastUnion = (-1, -1);

        foreach (var (i, j, _) in edges)
        {
            if (uf.Union(i, j))
            {
                unionsMade++;
                lastUnion = (i, j);
                if (unionsMade == n - 1) break; // fully connected
            }
        }

        if (lastUnion.i == -1) throw new InvalidOperationException("Graph did not become connected.");

        long result = (long)points[lastUnion.i].x * points[lastUnion.j].x;
        logger.Info($"Last connection between indices {lastUnion.i} and {lastUnion.j} with Xs {points[lastUnion.i].x} and {points[lastUnion.j].x}");
        return result.ToString();
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}

public class UnionFind
{
    private readonly int[] parent;
    private readonly int[] rank;

    public UnionFind(int n)
    {
        parent = new int[n];
        rank = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
        }
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    public bool Union(int a, int b)
    {
        int ra = Find(a), rb = Find(b);
        if (ra == rb) return false;
        if (rank[ra] < rank[rb]) parent[ra] = rb;
        else if (rank[ra] > rank[rb]) parent[rb] = ra;
        else
        {
            parent[rb] = ra; rank[ra]++;
        }
        return true;
    }
}