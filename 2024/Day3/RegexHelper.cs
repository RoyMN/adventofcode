using System.Text.RegularExpressions;

public static partial class RegexHelpers
{
    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    public static partial Regex multRegex();

    [GeneratedRegex(@"\b(do\(\)|don't\(\))")]
    public static partial Regex controlRegex();
}