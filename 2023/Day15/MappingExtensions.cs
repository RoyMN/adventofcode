namespace Extensions;

public static class MappingExtensions
{
    public static int Hash(this string input)
    {
        int currValue = 0;
        foreach (char c in input)
        {
            currValue += c;
            currValue *= 17;
            currValue %= 256;
        }
        return currValue;
    }
}