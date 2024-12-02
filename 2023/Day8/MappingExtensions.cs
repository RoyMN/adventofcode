namespace Extensions;

public static class MappingExtensions
{
    public static void Deconstruct(this string[] strings, out string L, out string R)
    {
        L = strings[0];
        R = strings[1];
    }

    public static char GetInstruction(this string instruction, int index = 0)
    {
        return instruction[index % instruction.Length];
    }

    // https://stackoverflow.com/questions/13569810/least-common-multiple
    public static int gcf(int a, int b)
    {
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
    }

    public static int lcm(int a, int b)
    {
        return (a / gcf(a, b)) * b;
    }
}