using Utils;
using static System.Console;

namespace Extensions;

public static class MappingExtensions
{
    public static ConditionRecord[] ToConditionRecord(this string[] input)
    {
        var result = new ConditionRecord[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            var (springs, damagedGroups) = input[i].Deconstruct();
            result[i] = new ConditionRecord(springs, damagedGroups);
        }
        return result;
    }

    public static ConditionRecord UnfoldFiveTimes(this ConditionRecord conditionRecord)
    {
        var springs = $"{conditionRecord.Springs}?{conditionRecord.Springs}?{conditionRecord.Springs}?{conditionRecord.Springs}?{conditionRecord.Springs}";
        var nGroups = conditionRecord.DamagedGroups.Length;
        var damagedGroups = new int[nGroups * 5];
        for (int i = 0; i < 5; i++)
        {
            Array.Copy(conditionRecord.DamagedGroups, 0, damagedGroups, i * nGroups, nGroups);
        }
        return new ConditionRecord(springs, damagedGroups);
    }

    public static (string left, int[] right) Deconstruct(this string line)
    {
        var parts = line.Split(' ');
        return (parts[0], parts[1].Split(',').Select(int.Parse).ToArray());
    }

    public static void DisplayLine(this ConditionRecord record)
    {
        WriteLine($"{string.Join("", record.Springs)} {string.Join(",", record.DamagedGroups)}");
    }

    public static IEnumerable<char[]> AllInterpretations(this string str, int position)
    {
        if (position == str.Length) { yield return []; yield break; }

        foreach (var inner in AllInterpretations(str, position + 1))
        {
            if (str[position] == '?')
            {
                yield return ['.', .. inner];
                yield return ['#', .. inner];
            }
            else
            {
                yield return [str[position], .. inner];
            }
        }
    }

    public static List<string> GenerateCombinations(this string input)
    {
        List<string> result = new List<string>();
        GenerateCombinationsRecursive(input, 0, ref result);
        return result;
    }

    private static void GenerateCombinationsRecursive(string current, int index, ref List<string> result)
    {
        int questionMarkIndex = current.IndexOf('?', index);

        if (questionMarkIndex == -1)
        {
            result.Add(current);
            return;
        }

        // Replace '?' with '.'
        string withDot = current.Substring(0, questionMarkIndex) + "." + current.Substring(questionMarkIndex + 1);
        GenerateCombinationsRecursive(withDot, questionMarkIndex + 1, ref result);

        // Replace '?' with '#'
        string withHash = current.Substring(0, questionMarkIndex) + "#" + current.Substring(questionMarkIndex + 1);
        GenerateCombinationsRecursive(withHash, questionMarkIndex + 1, ref result);
    }

    public static bool IsValidConditionRecord(this ConditionRecord record)
    {
        var springGroups = record.Springs.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (springGroups.Length != record.DamagedGroups.Length) return false;
        for (int i = 0; i < springGroups.Length; i++)
        {
            if (springGroups[i].Length != record.DamagedGroups[i]) return false;
        }
        return true;
    }

    public static void Display(this int[] intArray)
    {
        WriteLine($"[{string.Join(",", intArray)}]");
    }

    public static void Display(this int number)
    {
        WriteLine(number);
    }

    public static void Display(this long number)
    {
        WriteLine(number);
    }
}