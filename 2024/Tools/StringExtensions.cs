namespace AdventOfCode2024.Tools;

public static class StringExtensions
{
    public static (int left, int right) ToIntTouple(this string str, string separator)
    {
        string[] parts = str.Split(separator);
        return (int.Parse(parts[0]), int.Parse(parts[1]));
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
}