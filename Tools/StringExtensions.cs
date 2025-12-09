namespace AdventOfCodeTools;

public static class StringExtensions
{
    public static (int left, int right) ToIntTouple(this string str, string separator)
    {
        string[] parts = str.Split(separator);
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static (int left, int right) ToIntTouple(this string str, char separator)
    {
        string[] parts = str.Split(separator);
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static (long left, long right) ToLongTouple(this string str, char separator)
    {
        string[] parts = str.Split(separator);
        return (long.Parse(parts[0]), long.Parse(parts[1]));
    }

    public static (int x, int y, int z) TakeThree(this string str, char separator)
    {
        int i = 0;
        while (i < str.Length && str[i] != separator) i++;
        int x = int.Parse(str[..i]);
        int j = i + 1;
        while (j < str.Length && str[j] != separator) j++;
        int y = int.Parse(str[(i + 1)..j]);
        i = j + 1;
        while (i < str.Length && str[i] == separator)
        {
            if (i + 1 >= str.Length) break;
            i++;
        }
        int z = int.Parse(str[(j + 1)..]);
        return (x, y, z);
    }

    public static IEnumerable<int> ToInts(this string str, string separator)
    {
        return str.Split(separator).Select(int.Parse);
    }

    public static IEnumerable<int> ToInts(this string str, char separator)
    {
        return str.Split(separator).Select(int.Parse);
    }

    public static IEnumerable<IEnumerable<int>> ToCombinationsWithoutOne(this IEnumerable<int> ints)
    {
        return ints.Select((_, i) => ints.Where((_, j) => j != i).Select(value => value));
    }

    public static (int l, int r) TakeTwo(this string line, char delim)
    {
        var parts = line.Split(delim);
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static bool TryReadLine(StreamReader sr, out string line)
    {
        line = sr.ReadLine()!;
        return line != null;
    }

    public static bool TryParseInt(this StreamReader sr, out int value)
    {
        string? line = sr.ReadLine();
        if (line != null && int.TryParse(line, out value))
        {
            return true;
        }
        value = 0;
        return false;
    }
}