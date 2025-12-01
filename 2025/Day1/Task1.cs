using AdventOfCodeTools;

namespace AdventOfCode2025.Day1;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        int zeroesSeen = 0;
        int value = 50;
        string fileName = sample ? "./2025/Day1/sample.txt" : "./2025/Day1/input.txt";
        StreamReader sr = new(fileName);
        ConsoleWriter logger = new(logLevel);
        string? line = sr.ReadLine();
        int sign = 1;
        while (line != null)
        {
            if (line.Length == 0)
            {
                logger.Info("Empty line encountered, stopping read.");
                break;
            }
            logger.Info($"Read line: {line}");
            var adjustment = int.Parse(line[1..]);
            if (line[0] == 'L')
            {
                sign = -1;
            } else if (line[0] == 'R')
            {
                sign = 1;
            }
            var oldVal = value;
            var diff = sign * adjustment;

            // Count every time we land on 0
            int steps = Math.Abs(diff);
            for (int i = 1; i <= steps; i++)
            {
                value += sign * 1;
                if (sign == 1 && value > 99)
                {
                    value = 0;
                } else if (sign == -1 && value < 0)
                {
                    value = 99;
                }
            }
            if (value == 0)
            {
                zeroesSeen++;
            }

            logger.Info($"Old value: {oldVal}, New value: {value}, Zeroes seen: {zeroesSeen}");
            line = sr.ReadLine();
        }
        return $"Zeroes seen: {zeroesSeen}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}
