using System.Diagnostics.CodeAnalysis;
using Point = (int x, int y);
using PointedChar = (char ch, (int x, int y) p);
using PointedString = (string s, (int x, int y) p);

namespace Extensions;

public static class MappingExtensions
{
    /// <summary>
    /// Returns the char at <c>colIdx</c> in <c>line</c>.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="colIdx"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static bool TryGet([NotNullWhen(true)] this string line, int colIdx, out char ch)
    {
        ch = char.MinValue;
        if (colIdx < 0 || colIdx > line.Length - 1) return false;
        ch = line[colIdx];
        return true;
    }

    /// <summary>
    /// Returns the circumference of <c>p</c>.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static IEnumerable<Point> Circumference(this Point p)
    {
        for (int i = p.y - 1; i < p.y + 2; i++)
        {
            for (int j = p.x - 1; j < p.x + 2; j++)
            {
                if (j == p.x && i == p.y) continue;
                yield return (j, i);
            }
        }
    }

    /// <summary>
    /// Returns the char at <c>p</c> in <c>lines</c>.
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="p"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static bool TryGet([NotNullWhen(true)] this string[] lines, Point p, out char ch)
    {
        ch = char.MinValue;
        if (p.y < 0 || p.y > lines.Length - 1) return false;
        if (!lines[p.y].TryGet(p.x, out ch)) return false;
        return true;
    }

    /// <summary>
    /// Enumerates all adjacent chars to <c>p</c> in <c>lines</c>.
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static IEnumerable<PointedChar> AdjacentChars(this string[] lines, Point p)
    {
        foreach(var point in p.Circumference())
        {
            if (lines.TryGet(point, out char ch))
            {
                yield return (ch, point);
            }
        }
    }

    /// <summary>
    /// Returns the string of a number spanning on <c>line</c> from <c>p</c>.
    /// </summary>
    /// <param name="line">A line containing the number.</param>
    /// <param name="p">A point on the line and the number.</param>
    /// <returns><see cref="(string number, (int x, int y))"/></returns>
    public static PointedString StringNumber(this string line, Point p)
    {
        if (p.x < 0 || p.x > line.Length - 1) throw new();
        if (!char.IsDigit(line[p.x])) throw new();
        var startIdx = p.x;
        while (line.TryGet(startIdx - 1, out char ch))
        {
            if (char.IsDigit(ch))
            {
                startIdx -= 1;
            }
            else break;
        }
        var endIdx = startIdx;
        var stringNumber = "";
        while (line.TryGet(endIdx, out char ch))
        {
            if (char.IsDigit(ch))
            {
                stringNumber += ch;
                endIdx += 1;
            }
            else break;
        }
        return (stringNumber, (startIdx, p.y));
    }
    
    /// <summary>
    /// Checks if the <c>PointedString</c> contains the given <c>Point</c>.
    /// </summary>
    /// <returns>boolean</returns>
    public static bool Contains(this PointedString str, Point p)
    {
        if (str.p.y != p.y) return false;
        else return str.p.x <= p.x && p.x <= str.p.x + str.s.Length;
    }

    /// <summary>
    /// Finds all the <c>symbol</c>'s in <c>lines</c>.
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="symbol"></param>
    /// <returns><c>Point</c> for each <c>symbol</c></returns>
    public static IEnumerable<Point> FindSymbols(this string[] lines, char? symbol)
    {
        if (symbol == null)
        {
            if (lines.Length == 0) throw new();
            var innerLength = lines[0].Length;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < innerLength; j++)
                {
                    if (lines[i][j] != '.' && !char.IsDigit(lines[i][j]))
                    {
                        yield return (j, i);
                    }
                }
            }
        }
        else
        {
            if (lines.Length == 0) throw new();
            var innerLength = lines[0].Length;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < innerLength; j++)
                {
                    if (lines[i][j] == symbol)
                    {
                        yield return (j, i);
                    }
                }
            }
        }
    }

    public static void DisplayLines(this string[] lines)
    {
        Console.WriteLine();
        foreach(var line in lines)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }
}