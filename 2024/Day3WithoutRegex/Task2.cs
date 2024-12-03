using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day3WithoutRegex;

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        string fileName = "./Day3/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);
        var parser = new AdventDay3Parser(logger, includeControls: true);
        int sum = 0;

        while (sr.Peek() >= 0)
        {
            parser.Read((char)sr.Read());
            if (parser.Valid)
            {
                sum += parser.GetMultiplication();
            }
        }
        return $"Sum of all multiplications: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}