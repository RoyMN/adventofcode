global using Point = (int x, int y);
global using PointedSymbol = (char symbol, (int x, int y));
global using PointCombination = ((int x, int y) p1, (int x, int y) p2);

using static System.Console;

namespace Extensions;

public static class Extensions
{
    public static void DisplayLine(object line)
    {
        WriteLine($"{Environment.NewLine}{line}{Environment.NewLine}");
    }
    public static int Distance(this PointCombination pc)
    {
        return Math.Abs(pc.p1.x - pc.p2.x) + Math.Abs(pc.p1.y - pc.p2.y);
    }
    public static long ExpandedDistance(this PointCombination pc, int[] expandedColumns, int[] expandedRows, int expansionRate)
    {
        var normalDistance = pc.Distance();
        var expansions = pc.ExpansionsInPath(expandedColumns, expandedRows);
        var expansionLessDistance = normalDistance - expansions;
        return expansionLessDistance + (expansions * expansionRate);
    }
    public static int ExpansionsInPath(this PointCombination pc, int[] expandedColumns, int[] expandedRows)
    {
        var (x1, y1) = pc.p1;
        var (x2, y2) = pc.p2;
        var xDiff = Math.Abs(x1 - x2);
        var yDiff = Math.Abs(y1 - y2);
        var rowExpansions = expandedRows.Count(row => row >= Math.Min(y1, y2) && row <= Math.Max(y1, y2));
        var colExpansions = expandedColumns.Count(col => col >= Math.Min(x1, x2) && col <= Math.Max(x1, x2));
        return rowExpansions + colExpansions;
    }
    public static IEnumerable<Point> ListSymbols(this string[] input, char symbol)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == symbol)
                {
                    yield return (j, i);
                }
            }
        }
    }
    public static List<PointCombination> ListSymbolCombinations(this IEnumerable<Point> input)
    {
        var output = new List<PointCombination>();
        var points = input.ToArray();
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                if (!output.HasCombination((points[i], points[j])))
                {
                    output.Add((points[i], points[j]));
                }
            }
        }
        return output;
    }

    public static bool HasCombination(this List<PointCombination> values, PointCombination pc)
    {
        return values.Any(comb => comb.p1 == pc.p1 && comb.p2 == pc.p2 || comb.p1 == pc.p2 && comb.p2 == pc.p1);
    }

    public static void DisplayLines(object[] input)
    {
        WriteLine();
        for (int i = 0; i < input.Length; i++)
        {
            WriteLine(input[i]);
        }
        WriteLine();
    }

    public static void DisplayLines(List<object> input)
    {
        WriteLine();
        foreach(var obj in input)
        {
            WriteLine(obj);
        }
        WriteLine();
    }

    public static void DisplayLines(List<PointCombination> combinations)
    {
        WriteLine();
        foreach(var (p1, p2) in combinations)
        {
            WriteLine($"({p1}, {p2})");
        }
        WriteLine();
    }

    public static string[] ExpandLines(this string[] input)
    {
        var output = new List<string>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].HasGalaxy())
            {
                output.Add(input[i]);
            }
            else
            {
                output.Add(input[i]);
                output.Add(input[i]);
            }
        }
        return [.. output];
    }

    public static int[] ExpandedRows(this string[] input)
    {
        var output = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            if (!input[i].HasGalaxy())
            {
                output.Add(i);
            }
        }
        return [.. output];
    }

    public static string[] ExpandColumns(this string[] input)
    {
        List<string> output = [];
        input = input.Transpose();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].HasGalaxy())
            {
                output.Add(input[i]);
            }
            else
            {
                output.Add(input[i]);
                output.Add(input[i]);
            }
        }
        return output.ToArray().Transpose();
    }

    public static int[] ExpandedColumns(this string[] input)
    {
        var output = new List<int>();
        var lineLength = input[0].Length;
        for(int i = 0; i < lineLength; i++)
        {
            var col = input.Col(i);
            if (!col.HasGalaxy())
            {
                output.Add(i);
            }
        }
        return [.. output];
    }

    /// <summary>
    /// Transpose a string array by flipping column and row
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string[] Transpose(this string[] input)
    {
        var output = new string[input[0].Length];
        for (int i = 0; i < input[0].Length; i++)
        {
            output[i] = input.Col(i);
        }
        return output;
    }

    public static bool HasGalaxy(this string line)
    {
        return line.Contains('#');
    }

    /// <summary>
    /// Get a column from a string array
    /// </summary>
    /// <param name="input"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string Col(this string[] input, int col)
    {
        var output = "";
        for (int i = 0; i < input.Length; i++)
        {
            output += input[i][col];
        }
        return output;
    }
}