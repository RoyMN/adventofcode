namespace Extensions;

public static class MappingExtensions
{
    public static int GetFirstAndLastNumberSum(this string line, bool includeLiteral = false, bool includeStupidTest = false)
    {
        string part = string.Empty;
        int a = -1;
        int b = -1;
        for (int i = 0; i < line.Length; i++)
        {
            if (includeLiteral)
            {
                if (!part.IsLiteralNumberPart())
                {
                    // Drop first char
                    part = part[1..];
                }
                part += line[i];
                if (part.IsLiteralNumber())
                {
                    // conditionally set a
                    if (a == -1) a = part.FromLiteralNumber();
                    b = part.FromLiteralNumber();
                    // reset with end of literal
                    var tmp = part;
                    part = "";
                    part += tmp[^1];
                }
            }
            if (char.IsDigit(line[i]))
            {
                var val = int.Parse($"{line[i]}");
                // conditionally set a
                if (a == -1) a = val;

                // set b
                b = val;

                // reset part
                part = string.Empty;
            }
        }
        if (includeLiteral && includeStupidTest)
        {
            if (!line.StupidTest(a, b)) Console.WriteLine($"Failed test for {line} with {a} and {b}");
        }
        return a * 10 + b;
    }

    public static bool StupidTest(this string line, int first, int second)
    {
        var firstTest = new int[][]
        {
            new int[] { line.IndexOf("one"), line.IndexOf('1') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("two"), line.IndexOf('2') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("three"), line.IndexOf('3') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("four"), line.IndexOf('4') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("five"), line.IndexOf('5') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("six"), line.IndexOf('6') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("seven"), line.IndexOf('7') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("eight"), line.IndexOf('8') }.Where(x => x != -1).ToArray(),
            new int[] { line.IndexOf("nine"), line.IndexOf('9') }.Where(x => x != -1).ToArray(),
        };

        var secondTest = new int[][]
        {
            new int[] { line.LastIndexOf("one"), line.LastIndexOf('1') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("two"), line.LastIndexOf('2') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("three"), line.LastIndexOf('3') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("four"), line.LastIndexOf('4') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("five"), line.LastIndexOf('5') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("six"), line.LastIndexOf('6') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("seven"), line.LastIndexOf('7') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("eight"), line.LastIndexOf('8') }.Where(x => x != -1).ToArray(),
            new int[] { line.LastIndexOf("nine"), line.LastIndexOf('9') }.Where(x => x != -1).ToArray(),
        };

        if (firstTest[first-1].Length == 0) return false;
        var min = firstTest[first-1].Min();
        var others = firstTest.Where((x, idx) => idx != first-1 && x.Length > 0).Select(x => x.Min()).ToArray();
        if (others.Any(x => x < min)) return false;

        if (secondTest[second-1].Length == 0) return false;
        var max = secondTest[second-1].Max();
        others = secondTest.Where((x, idx) => idx != second-1 && x.Length > 0).Select(x => x.Max()).ToArray();
        if (others.Any(x => x > max)) return false;

        return true;
    }

    public static int FromLiteralNumber(this string part)
    {
        if (part == string.Empty) throw new ArgumentException("Invalid literal number!");
        return part switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => throw new ArgumentException("Invalid literal number!"),
        };
    }

    public static bool IsLiteralNumber(this string part)
    {
        if (part == string.Empty) return false;
        return part switch
        {
            "one" => true,
            "two" => true,
            "three" => true,
            "four" => true,
            "five" => true,
            "six" => true,
            "seven" => true,
            "eight" => true,
            "nine" => true,
            _ => false,
        };
    }

    private static readonly string[] literalNumberArray = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    public static bool IsLiteralNumberPart(this string part)
    {
        if (part == string.Empty) return true;
        return literalNumberArray.Any(x => {
            if (part.Length > x.Length) return false;
            for (int i = 0; i < part.Length; i++)
            {
                if (part[i] != x[i]) return false;
            }
            return true;
        });
    }
}