namespace Extensions;

public static class MappingExtensions
{
    public static void Deconstruct(this string[] strings, out string L, out string R)
    {
        L = strings[0];
        R = strings[1];
    }
}