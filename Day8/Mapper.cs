namespace Utils;

public class Mapper(string[] StartKeys, Dictionary<string, (string L, string R)> map, Direction[] instructions)
{
    string[] currentKeys = StartKeys;
    int steps = 0;
    int instructionsEndIndex = instructions.Length;
    int nextStep { get => (steps + 1) % instructionsEndIndex; }
    public void Next()
    {
        var instruction = instructions[nextStep];
        var newKeys = new string[currentKeys.Length];
        for (int i = 0; i < currentKeys.Length; i++)
        {
            var key = currentKeys[i];
            var (L, R) = map[key];
            newKeys[i] = instruction switch
            {
                Direction.L => L,
                Direction.R => R,
                _ => throw new Exception("Invalid direction"),
            };
        }
        currentKeys = newKeys;
        steps++;
    }
}

public enum Direction
{
    L,
    R
}