namespace AdventOfCode2024.Tools;

public static class StringExtensions
{
    public static (int left, int right) ToIntTouple(this string str, string separator)
    {
        string[] parts = str.Split(separator);
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}