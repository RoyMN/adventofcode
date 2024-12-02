public class Program
{
    public static void Main(string[] args)
    {
        Part1();
        //Part2();
    }

    public static void Part1()
    {
        string filePath = "./input.txt";
        var sum = 0;
        string[] lines = File.ReadAllLines(filePath);
        CubeLimits cubeLimits = new CubeLimits(12, 13, 14);

        foreach (string line in lines)
        {
            var gameInfo = line.Split(':');
            var gameId = int.Parse(gameInfo[0].Replace("Game ", ""));
            string[] sets = gameInfo[1].Split(';');
            if (sets.All(s => IsValidSet(s, cubeLimits)))
            {
                sum += gameId;
            }
        }
        System.Console.WriteLine(sum);
    }

    public static void Part2()
    {
        string filePath = "./input.txt";
        var sum = 0;
        string[] lines = File.ReadAllLines(filePath);

        foreach(string line in lines)
        {
            var gameInfo = line.Split(':');
            var gameId = int.Parse(gameInfo[0].Replace("Game ", ""));
            string[] sets = gameInfo[1].Split(';');
            var limits = GetLimits(sets);
            sum += CubeLimitPower(limits);
        }
        Console.WriteLine(sum);
    }

    public static int CubeLimitPower(CubeLimits cubeLimits)
    {
        return cubeLimits.Red * cubeLimits.Green * cubeLimits.Blue;
    }

    public static CubeLimits GetLimits(string[] sets)
    {
        var currentLimit = new CubeLimits(0, 0, 0);

        foreach (string set in sets)
        {
            string[] cubes = set.Split(',');

            // Iterate through the cubes
            foreach (string cube in cubes)
            {
                var localValues = cube.Trim().Split(' ');

                int number = int.Parse(localValues[0]);
                string color = localValues[1];

                if (color == "red" && number > currentLimit.Red)
                {
                    currentLimit = new CubeLimits(number, currentLimit.Green, currentLimit.Blue);
                }
                else if (color == "green" && number > currentLimit.Green)
                {
                    currentLimit = new CubeLimits(currentLimit.Red, number, currentLimit.Blue);
                }
                else if (color == "blue" && number > currentLimit.Blue)
                {
                    currentLimit = new CubeLimits(currentLimit.Red, currentLimit.Green, number);
                }
            }
        }
        return currentLimit;
    }

    public static bool IsValidSet(string set, CubeLimits cubeLimits)
    {
        string[] cubes = set.Split(',');

        // Iterate through the cubes
        foreach (string cube in cubes)
        {
            var localValues = cube.Trim().Split(' ');

            int number = int.Parse(localValues[0]);
            string color = localValues[1];

            if (color == "red" && number > cubeLimits.Red)
            {
                return false;
            }
            else if (color == "green" && number > cubeLimits.Green)
            {
                return false;
            }
            else if (color == "blue" && number > cubeLimits.Blue)
            {
                return false;
            }
        }
        return true;
    }
}

public record CubeLimits(int Red, int Green, int Blue);